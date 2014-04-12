using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using HtmlAgilityPack;
using MyLibrary;
using MyLibrary.LastError;
using MyLibrary.Trace;

namespace MyParser
{
    /// <summary>
    ///     Класс вспомогательных алгоритмов
    /// </summary>
    public class Parser : ILastError, IParser, ITrace, IValueable
    {
        public const char SplitChar = '\\';

        private readonly MethodInfo[] _methodInfos =
        {
            typeof (Parser).GetMethod("InvokeNodeProperty"),
            typeof (Parser).GetMethod("AttributeValue"),
            typeof (Parser).GetMethod("CustomNodeProperty")
        };

        public Parser()
        {
            Transformation = Defaults.Transformation;
        }

        public ITransformation Transformation { get; set; }
        public object LastError { get; set; }

        /// <summary>
        ///     Поиск и формирование значений возвращаемых полей загруженного с сайта объявления
        /// </summary>
        public ReturnFields BuildReturnFields(IEnumerable<HtmlDocument> parentDocuments, Values parentValues,
            ReturnFieldInfos returnFieldInfos)
        {
            var returnFields = new ReturnFields();
            long current = 0;
            long total = returnFieldInfos.ToList().Count*(parentDocuments.Count() + 1);
            foreach (ReturnFieldInfo returnFieldInfo in returnFieldInfos.ToList())
            {
                var agregated = new Values();
                foreach (HtmlDocument document in parentDocuments)
                {
                    var values = new Values(parentValues);
                    IEnumerable<string> xpaths =
                        Transformation.ParseTemplate(returnFieldInfo.ReturnFieldXpathTemplate.ToString(),
                            parentValues);

                    foreach (
                        HtmlNode htmlNode in
                            xpaths.Select(xpath => document.DocumentNode.SelectNodes(xpath))
                                .Where(nodes => nodes != null)
                                .SelectMany(nodes => nodes))
                    {
                        values.Add(BuildValues(returnFieldInfo.ReturnFieldResultTemplate.ToString(), htmlNode));
                    }

                    foreach (var pair in values)
                        if (!agregated.ContainsKey((pair.Key))) agregated.Add(pair.Key, pair.Value);
                        else if (agregated[pair.Key].Count() < pair.Value.Count())
                            agregated[pair.Key] = pair.Value;

                    if (ProgressCallback != null) ProgressCallback(++current, total);
                    Thread.Yield();
                }


                var regex = new Regex(returnFieldInfo.ReturnFieldRegexPattern.ToString(),
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                IEnumerable<string> list =
                    Transformation.ParseTemplate(returnFieldInfo.ReturnFieldResultTemplate.ToString(), agregated)
                        .SelectMany(
                            replace =>
                                (from Match match in
                                    Regex.Matches(replace,
                                        returnFieldInfo.ReturnFieldRegexPattern.ToString())
                                    select match.Value.Trim()))
                        .Select(
                            input => regex.Replace(input, returnFieldInfo.ReturnFieldRegexReplacement.ToString()).Trim())
                        .Where(replace => !string.IsNullOrEmpty(replace));
                returnFields.Add(returnFieldInfo.ReturnFieldId.ToString(), list);
                if (ProgressCallback != null) ProgressCallback(++current, total);
            }

            if (CompliteCallback != null) CompliteCallback();
            Thread.Yield();
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
            var values = new Values();
            MatchCollection matches = Regex.Matches(template, Transformation.FieldPattern);
            long current = 0;
            long total = matches.Count*_methodInfos.Count();
            foreach (
                string name in
                    from Match match in matches
                    select match.Groups[MyLibrary.Transformation.NameGroup].Value)
                foreach (MethodInfo methodInfo in _methodInfos)
                    try
                    {
                        string key = string.Format("{0}", name);
                        if (values.ContainsKey(key)) continue;
                        object value = methodInfo.Invoke(null, new object[] {node, name});
                        if (value != null) values.Add(key, value.ToString());
                        if (ProgressCallback != null) ProgressCallback(++current, total);
                    }
                    catch (Exception exception)
                    {
                        LastError = exception;
                        Debug.WriteLine(LastError.ToString());
                    }
            if (CompliteCallback != null) CompliteCallback();
            Thread.Yield();
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
        public static string InvokeNodeProperty(HtmlNode node, string propertyName)
        {
            string[] names = propertyName.Split('.');
            object value = node;
            foreach (PropertyInfo propertyInfo in names.Select(name => typeof (HtmlNode).GetProperty(name)))
            {
                value = propertyInfo != null ? propertyInfo.GetValue(value, null) : null;
                if (value == null) return null;
            }
            return (string) value;
        }

        /// <summary>
        ///     Получение значения указанного свойства указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string CustomNodeProperty(HtmlNode node, string propertyName)
        {
            switch (propertyName)
            {
                case "VisibleInnerText":
                    return
                        new Regex("\\<[^\\>]+\\>", RegexOptions.Singleline).Replace(
                            CustomNodeProperty(node, "VisibleOuterHtml"),
                            string.Empty);

                case "VisibleInnerHtml":
                    return
                        new Regex("\\A\\<[^\\>]+\\>(.*)\\<[^\\>]+\\>\\Z", RegexOptions.Singleline).Replace(
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
                                        new Regex("(?<classes>[^\\{]+)\\{(?<style>[^\\}]*)\\}", RegexOptions.Singleline)
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
                    if (classes1 != null && new Regex(@"\s+").Split(classes1.Value)
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
                             || (classes != null && new Regex(@"\s+").Split(classes.Value)
                                 .Any(c => (visible.ContainsKey("." + c)) && !visible["." + c])))
                                ? string.Empty
                                : (replacement = CustomNodeProperty(child, propertyName)) == null
                                    ? string.Empty
                                    : replacement);
                    }
                    Debug.WriteLine(outerHtml);

                    return new Regex("\\v", RegexOptions.Singleline).Replace(outerHtml, string.Empty);
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
                        sb.AppendLine(new Regex("(\\s|\\<(\\/)?(div|span|img)[^\\>]*\\>)", RegexOptions.Singleline)
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
            if (string.IsNullOrEmpty(style)) return false;

            Dictionary<string, string> keys = ParseHtmlStyleString(style);

            return (keys.Keys.Contains("display")) || (keys.Keys.Contains("visibility"));
        }

        private static bool CheckStyleVisibility(string style)
        {
            if (string.IsNullOrWhiteSpace(style)) return true;
            if (string.IsNullOrEmpty(style)) return true;

            Dictionary<string, string> keys = ParseHtmlStyleString(style);

            if (keys.Keys.Contains("display") && keys["display"] == "none")
                return false;

            if (keys.Keys.Contains("visibility") && keys["visibility"] == "hidden")
                return false;

            return true;
        }

        public static Dictionary<string, string> ParseHtmlStyleString(string style)
        {
            style = new Regex("\\s+", RegexOptions.Singleline).Replace(style, string.Empty).ToLowerInvariant();
            string[] settings = style.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
            return (from s in settings where s.Contains(':') select s.Split(':')).ToDictionary(data => data[0],
                data => data[1]);
        }

        #endregion
    }
}