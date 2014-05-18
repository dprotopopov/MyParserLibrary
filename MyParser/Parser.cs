using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyLibrary;
using MyLibrary.Collections;
using MyLibrary.LastError;
using MyLibrary.Trace;
using Regex = MyLibrary.Types.Regex;

namespace MyParser
{
    /// <summary>
    ///     Класс вспомогательных алгоритмов
    /// </summary>
    public class Parser : ILastError, IParser, ITrace, IValueable
    {
        public Parser()
        {
            Transformation = Defaults.Transformation;
            SplitChar = '\\';
        }

        public char SplitChar { get; set; }

        public ITransformation Transformation { get; set; }
        public object LastError { get; set; }

        /// <summary>
        ///     Поиск и формирование значений возвращаемых полей загруженного с сайта объявления
        /// </summary>
        public ReturnFields BuildReturnFields(IEnumerable<MemoryStream> streams, Values parents,
            IEnumerable<ReturnFieldInfo> returnFieldInfos)
        {
            var progress = new object();
            var returnFields = new ReturnFields();
            long current = 0;
            long total = returnFieldInfos.Count()*streams.Count()*parents.MaxCount;
            var sources = new StackListQueue<string>();
            var documents = new StackListQueue<HtmlDocument>();
            foreach (MemoryStream stream in streams)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var edition = new HtmlDocument();
                edition.Load(stream, Encoding.Default);
                documents.Add(edition);
            }
            foreach (MemoryStream stream in streams)
            {
                stream.Seek(0, SeekOrigin.Begin);
                // stream: Считываемый поток.
                // encoding: Кодировка символов, которую нужно использовать.
                // detectEncodingFromByteOrderMarks: Значение true для поиска меток порядка следования байтов в начале файла; в противном случае — значение false.
                // bufferSize: Минимальный размер буфера.
                // leaveOpen: Значение true, чтобы оставить поток открытым после удаления объекта System.IO.StreamReader; в противном случае — значение false.
                using (var sr = new StreamReader(stream, Encoding.Default, true, 1 << 10, true))
                    sources.Add(sr.ReadToEnd());
            }

            Parallel.ForEach(returnFieldInfos, returnFieldInfo =>
            {
                IEnumerable<string> ids =
                    Transformation.ParseTemplate(returnFieldInfo.ReturnFieldId.ToString(),
                        parents, false);
                IEnumerable<string> xpaths =
                    Transformation.ParseTemplate(returnFieldInfo.ReturnFieldXpathTemplate.ToString(),
                        parents, false);
                IEnumerable<string> results =
                    Transformation.ParseTemplate(returnFieldInfo.ReturnFieldResultTemplate.ToString(),
                        parents, true);
                IEnumerable<string> patterns =
                    Transformation.ParseTemplate(returnFieldInfo.ReturnFieldRegexPattern.ToString(),
                        parents, true);
                IEnumerable<string> replacements =
                    Transformation.ParseTemplate(returnFieldInfo.ReturnFieldRegexReplacement.ToString(),
                        parents, true);
                for (int i = 0; i < parents.MaxCount; i++)
                {
                    string id = ids.ElementAt(i);
                    string xpath = xpaths.ElementAt(i);
                    string result = results.ElementAt(i);
                    string pattern = patterns.ElementAt(i);
                    string replacement = replacements.ElementAt(i);
                    IEnumerable<string> list;
                    if (!string.IsNullOrEmpty(xpath))
                    {
                        var agregated = new Values();
                        foreach (HtmlDocument document in documents)
                        {
                            var values = new Values(parents);

                            try
                            {
                                foreach (HtmlNode htmlNode in document.DocumentNode.SelectNodes(xpath))
                                    values.Add(BuildValues(result, htmlNode));
                            }
                            catch (Exception exception)
                            {
                                Debug.WriteLine(exception);
                                LastError = exception;
                            }
                            foreach (var pair in values)
                                if (!agregated.ContainsKey((pair.Key))) agregated.Add(pair.Key, pair.Value);
                                else if (agregated[pair.Key].Count() < pair.Value.Count())
                                    agregated[pair.Key] = pair.Value;

                            if (ProgressCallback != null) lock (progress) ProgressCallback(++current, total);
                        }
                        list = new StackListQueue<string>(Transformation.ParseTemplate(result, agregated)
                            .SelectMany(s => Regex.MatchReplace(s, pattern, replacement)));
                    }
                    else
                    {
                        if (ProgressCallback != null)
                            lock (progress) ProgressCallback(current += streams.Count(), total);
                        list =
                            new StackListQueue<string>(Regex.MatchReplace(sources.Last(), pattern, replacement));
                    }
                    if (string.IsNullOrEmpty(returnFieldInfo.JoinSeparator.ToString()))
                        lock (returnFields)
                            returnFields.Add(id, list);
                    else
                        lock (returnFields)
                            returnFields.Add(id, string.Join(returnFieldInfo.JoinSeparator.ToString(), list));
                }
            });

            if (CompliteCallback != null) CompliteCallback();
            return returnFields;
        }

        /// <summary>
        ///     Формирование пар идентификатор параметра - значение параметра
        ///     для замены в строке-шаблоне
        /// </summary>
        /// <param name="template"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public Values BuildValues(string template, HtmlNode node)
        {
            IEnumerable<string> names =
                from Match match in System.Text.RegularExpressions.Regex.Matches(template, Transformation.FieldPattern)
                select match.Groups[MyLibrary.Transformation.NameGroup].Value;
            IEnumerable<MethodInfo> methods = GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static)
                .Where(p => p.GetCustomAttributes(typeof (NodeProperty), false).Any());

            var values = new Values();
            Parallel.ForEach(from name in names
                from method in methods
                select new KeyValuePair<string, MethodInfo>(name, method), pair =>
                {
                    lock (values) if (values.ContainsKey(pair.Key)) return;
                    object value = pair.Value.Invoke(null, new object[] {node, pair.Key});
                    if (value == null) return;
                    lock (values) values.Add(pair.Key, value.ToString());
                });

            if (CompliteCallback != null) CompliteCallback();
            return values;
        }

        public ProgressCallback ProgressCallback { get; set; }
        public AppendLineCallback AppendLineCallback { get; set; }
        public CompliteCallback CompliteCallback { get; set; }

        public Values ToValues()
        {
            return new Values(this);
        }

        #region Получение значения параметра

        /// <summary>
        ///     Получение значения указанного аттрибута указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        [NodeProperty]
        public static string AttributeValue(HtmlNode node, string attributeName)
        {
            HtmlAttribute attribute = node.Attributes[attributeName];
            string value = (attribute != null) ? attribute.Value : null;
            return (value != null) ? Uri.UnescapeDataString(value) : null;
        }

        /// <summary>
        ///     Получение значения указанного свойства указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [NodeProperty]
        public static string InvokeNodeProperty(HtmlNode node, string propertyName)
        {
            object value = node;
            foreach (
                PropertyInfo propertyInfo in propertyName.Split('.')
                    .Select(name => typeof (HtmlNode).GetProperty(name))
                )
            {
                value = propertyInfo != null ? propertyInfo.GetValue(value, null) : null;
                if (value == null) return null;
            }
            return WebUtility.HtmlDecode(value.ToString().Trim());
        }

        /// <summary>
        ///     Получение значения указанного свойства указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        [NodeProperty]
        public static string CustomNodeProperty(HtmlNode node, string propertyName)
        {
            switch (propertyName)
            {
                case "VisibleInnerText":
                    return
                        new System.Text.RegularExpressions.Regex("\\<[^\\>]+\\>", RegexOptions.Singleline).Replace(
                            CustomNodeProperty(node, "VisibleOuterHtml"),
                            string.Empty);

                case "VisibleInnerHtml":
                    return
                        new System.Text.RegularExpressions.Regex("\\A\\<[^\\>]+\\>(.*)\\<[^\\>]+\\>\\Z",
                            RegexOptions.Singleline).Replace(
                                CustomNodeProperty(node, "VisibleOuterHtml"),
                                "$1");

                case "VisibleOuterHtml":
                {
                    HtmlAttribute style1 = node.Attributes["style"];
                    HtmlAttribute id1 = node.Attributes["id"];
                    HtmlAttribute classes1 = node.Attributes["class"];
                    if (style1 != null && !CheckStyleVisibility(style1.Value)) return null;
                    var visible = new Dictionary<string, bool>();
                    HtmlNodeCollection styles = node.SelectNodes("//style");
                    if (styles != null)
                        foreach (
                            MatchCollection matches in
                                styles.Select(
                                    styleNode =>
                                        new System.Text.RegularExpressions.Regex(
                                            "(?<classes>[^\\{]+)\\{(?<style>[^\\}]*)\\}", RegexOptions.Singleline)
                                            .Matches(
                                                styleNode.InnerText))
                            )
                            foreach (Match match in matches)
                            {
                                IEnumerable<string> classes =
                                    match.Groups["classes"].Value.Split(',').Select(s => s.Trim());
                                foreach (
                                    string c in classes.Where(c => StyleModifyVisibility(match.Groups["style"].Value)))
                                    if (visible.ContainsKey(c))
                                        visible[c] = CheckStyleVisibility(match.Groups["style"].Value);
                                    else visible.Add(c, CheckStyleVisibility(match.Groups["style"].Value));
                            }
                    if (id1 != null &&
                        ((visible.ContainsKey("#" + id1.Value.Trim())) && !visible["#" + id1.Value.Trim()]))
                        return null;
                    if (classes1 != null && new System.Text.RegularExpressions.Regex(@"\s+").Split(classes1.Value)
                        .Any(c => (visible.ContainsKey("." + c)) && !visible["." + c])) return null;
                    string outerHtml = node.OuterHtml;
                    HtmlNodeCollection children = node.SelectNodes(node.XPath + @"/*");
                    if (children == null) return outerHtml;
                    Debug.WriteLine("======================================");
                    Debug.WriteLine(string.Join(Environment.NewLine, visible.Select(pair => pair.Key + ":" + pair.Value)));
                    for (int index = children.Count; index-- > 0;)
                    {
                        Debug.WriteLine(outerHtml);
                        HtmlNode child = children[index];
                        Debug.WriteLine("=============" + child.OuterHtml + "===========");
                        HtmlAttribute style = child.Attributes["style"];
                        HtmlAttribute id = child.Attributes["id"];
                        HtmlAttribute classes = child.Attributes["class"];
                        string replacement;
                        outerHtml = outerHtml.Replace(child.OuterHtml,
                            ((style != null && !CheckStyleVisibility(style.Value))
                             || (id != null &&
                                 ((visible.ContainsKey("#" + id.Value.Trim())) && !visible["#" + id.Value.Trim()]))
                             ||
                             (classes != null && new System.Text.RegularExpressions.Regex(@"\s+").Split(classes.Value)
                                 .Any(c => (visible.ContainsKey("." + c)) && !visible["." + c])))
                                ? string.Empty
                                : (replacement = CustomNodeProperty(child, propertyName)) == null
                                    ? string.Empty
                                    : replacement);
                    }
                    Debug.WriteLine(outerHtml);

                    return new System.Text.RegularExpressions.Regex("\\v", RegexOptions.Singleline).Replace(outerHtml,
                        string.Empty);
                }

                case "HidemyAssOuterHtml":
                {
                    HtmlNode node1 = node.Clone();
                    for (int index = 0;
                        node1.SelectNodes(@"./td[2]//*") != null &&
                        index < node1.SelectNodes(@"./td[2]//*").Count;
                        )
                    {
                        HtmlNode child = node1.SelectNodes(@"./td[2]//*")[index];
                        HtmlAttribute style = child.Attributes["style"];
                        if ((style != null && !CheckStyleVisibility(style.Value)))
                            child.ParentNode.RemoveChild(child);
                        else index++;
                    }
                    Debug.Assert(node1.SelectNodes(@"./td[2]//*") == null ||
                                 node1.SelectNodes(@"./td[2]//*").Count <= node.SelectNodes(@"./td[2]//*").Count);
                    if (node1.SelectNodes(@"./td[2]//*") == null) return null;
                    IEnumerable<string> classes1 =
                        node1.SelectNodes(@"./td[2]//*")
                            .Select(n => n.Attributes["class"])
                            .Where(a => a != null)
                            .Select(a => a.Value)
                            .Distinct();
                    var sb = new StringBuilder();
                    Debug.WriteLine("======================================");
                    int i = 1 << classes1.Count();
                    for (HtmlNode node2 = node1.Clone(); i-- > 0; node2 = node1.Clone())
                    {
                        for (int index1 = 0;
                            index1 < classes1.Count() &&
                            node2.SelectNodes(@"./td[2]//*") != null;
                            index1++)
                            if (((i >> index1) & 1) == 0)
                            {
                                string value = classes1.ElementAt(index1);
                                for (int index2 = 0;
                                    node2.SelectNodes(@"./td[2]//*") != null &&
                                    index2 < node2.SelectNodes(@"./td[2]//*").Count;
                                    )
                                {
                                    HtmlNode child2 = node2.SelectNodes(@"./td[2]//*")[index2];
                                    HtmlAttribute c2 = child2.Attributes["class"];
                                    if ((c2 != null && c2.Value.CompareTo(value) == 0))
                                        child2.ParentNode.RemoveChild(child2);
                                    else index2++;
                                }
                            }
                        //Debug.WriteLine(outerHtml);
                        sb.AppendLine(new System.Text.RegularExpressions.Regex(
                            "(\\s|\\<(\\/)?(div|span|img)[^\\>]*\\>)", RegexOptions.Singleline)
                            .Replace(
                                node2.OuterHtml, string.Empty));
                    }
                    Debug.WriteLine(sb.ToString());
                    return sb.ToString();
                }
            }
            return null;
        }

        #endregion

        #region

        private static bool StyleModifyVisibility(string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return false;
            if (string.IsNullOrWhiteSpace(style)) return false;

            Dictionary<string, string> keys = ParseHtmlStyleString(style);

            return (keys.Keys.Contains("display")) || (keys.Keys.Contains("visibility"));
        }

        private static bool CheckStyleVisibility(string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return true;
            if (string.IsNullOrWhiteSpace(style)) return true;

            Dictionary<string, string> keys = ParseHtmlStyleString(style);

            if (keys.Keys.Contains("display") && keys["display"] == "none")
                return false;

            if (keys.Keys.Contains("visibility") && keys["visibility"] == "hidden")
                return false;

            return true;
        }

        public static Dictionary<string, string> ParseHtmlStyleString(string style)
        {
            style =
                new System.Text.RegularExpressions.Regex("\\s+", RegexOptions.Singleline).Replace(style, string.Empty)
                    .ToLowerInvariant();
            string[] settings = style.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            return (from s in settings where s.Contains(':') select s.Split(':')).ToDictionary(data => data[0],
                data => data[1]);
        }

        #endregion

        protected class NodeProperty : Attribute
        {
        }
    }
}