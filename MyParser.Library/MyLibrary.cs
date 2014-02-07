using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyWebCrawler;
using TidyManaged;

namespace MyParser.Library
{
    /// <summary>
    ///     Класс вспомогательных алгоритмов
    /// </summary>
    public class MyLibrary
    {
        protected const string FieldPattern = @"\{\{(?<name>[^\}]*)\}\}";
        public static object LastError { get; set; }

        public static string IntroText(string text, int introLength = 120)
        {
            if (text.Length < introLength || introLength <= 0) return text;
            return text.Substring(0, introLength) + " ...";
        }

        /// <summary>
        ///     Запрос к сайту с использованием MyWebCrawler
        /// </summary>
        public static async Task<HtmlDocument[]> WebRequestHtmlDocument(Uri uri, string method, string encoding)
        {
            Debug.Assert(uri != null);
            Debug.Assert(method != null);
            Debug.Assert(encoding != null);
            Debug.WriteLine("Begin " + MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine(uri.ToString());
            ICrawler crawler = new WebCrawler();
            var requestWeb = (HttpWebRequest) WebRequest.Create(uri);
            requestWeb.Method = method;
            var collection = new List<HtmlDocument>();
            WebResponse response = await crawler.GetResponse(requestWeb);
            if (response != null)
            {
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    Encoding encoder = String.IsNullOrEmpty(encoding)
                        ? Encoding.Default
                        : Encoding.GetEncoding(encoding);
                    var reader = new StreamReader(responseStream, encoder);
                    var memoryStreams = new List<MemoryStream>
                    {
                        new MemoryStream(Encoding.UTF8.GetBytes(reader.ReadToEnd()))
                    };

                    Debug.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                    memoryStreams.Last().Seek(0, SeekOrigin.Begin);
                    Debug.WriteLine(IntroText(new StreamReader(memoryStreams.Last()).ReadToEnd()));
                    Debug.WriteLine("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

                    memoryStreams.First().Seek(0, SeekOrigin.Begin);
                    Document tidy = Document.FromStream(memoryStreams.First());

                    tidy.ForceOutput = true;
                    tidy.PreserveEntities = true;
                    tidy.InputCharacterEncoding = EncodingType.Raw;
                    tidy.OutputCharacterEncoding = EncodingType.Raw;
                    tidy.CharacterEncoding = EncodingType.Raw;
                    tidy.ShowWarnings = false;
                    tidy.Quiet = true;
                    tidy.OutputXhtml = true;
                    tidy.CleanAndRepair();

                    memoryStreams.Add(new MemoryStream());
                    tidy.Save(memoryStreams.Last());

                    Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    memoryStreams.Last().Seek(0, SeekOrigin.Begin);
                    Debug.WriteLine(IntroText(new StreamReader(memoryStreams.Last()).ReadToEnd()));
                    Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

                    foreach (MemoryStream stream in memoryStreams)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        var edition = new HtmlDocument();
                        edition.LoadHtml(new StreamReader(stream, Encoding.UTF8).ReadToEnd());
                        collection.Add(edition);
                    }
                }
            }
            Debug.WriteLine("End " + MethodBase.GetCurrentMethod().Name);
            return collection.ToArray();
        }


        /// <summary>
        ///     Замена в строке-шаблоне идентификаторов-параметров на их значения
        /// </summary>
        public static List<string> ParseColTemplate(string template, ParametersValues parametersValues)
        {
            Debug.Assert(template != null);
            Debug.Assert(parametersValues != null);
            Debug.WriteLine("Begin " + MethodBase.GetCurrentMethod().Name + " : " + template);
            Debug.WriteLine("parametersValues :");
            Debug.WriteLine(parametersValues.ToString());
            var list = new List<string>();
            int maxCount = parametersValues.MaxCount;
            for (int index = 0; index < maxCount; index++)
            {
                string value = template.Trim();
                foreach (var pair in parametersValues)
                {
                    if (pair.Value != null && pair.Value.Count > index)
                    {
                        string replacement = pair.Value[index];
                        var regex = new Regex(pair.Key, RegexOptions.IgnoreCase);
                        value = regex.Replace(value, replacement).Trim();
                    }
                }
                var rgx = new Regex(FieldPattern, RegexOptions.IgnoreCase);
                value = rgx.Replace(value, @"").Trim();
                if (!String.IsNullOrEmpty(value))
                {
                    list.Add(value);
                    Debug.WriteLine("" + MethodBase.GetCurrentMethod().Name + " : " + template + " add value " +
                                    IntroText(value));
                }
            }
            return list;
        }

        public static string ParseRowTemplate(string template, ParametersValues parametersValues)
        {
            Debug.Assert(template != null);
            Debug.Assert(parametersValues != null);
            Debug.WriteLine("Begin " + MethodBase.GetCurrentMethod().Name + " : " + template);
            Debug.WriteLine("parametersValues :");
            Debug.WriteLine(parametersValues.ToString());
            string value = template.Trim();
            foreach (var pair in parametersValues)
            {
                MatchCollection matches = Regex.Matches(value, pair.Key);
                for (int i = matches.Count; i-- > 0;)
                {
                    Match match = matches[i];
                    if (i < pair.Value.Count)
                        value = value.Substring(0, match.Index) + pair.Value[i] +
                                value.Substring(match.Index + match.Length);
                }
            }
            var rgx = new Regex(FieldPattern, RegexOptions.IgnoreCase);
            value = rgx.Replace(value, @"").Trim();
            Debug.WriteLine("End " + MethodBase.GetCurrentMethod().Name + " : " + value);
            return value;
        }

        /// <summary>
        ///     Поиск и формирование значений возвращаемых полей загруженного с сайта объявления
        /// </summary>
        public static ReturnFields BuildReturnFields(HtmlNode[] parentNodes,
            ParametersValues parentParametersValues, ReturnFieldInfos returnFieldInfos)
        {
            Debug.Assert(parentNodes != null);
            Debug.Assert(parentParametersValues != null);
            Debug.Assert(returnFieldInfos != null);
            Debug.WriteLine("Begin " + MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine("parentNodes.Length = " + parentNodes.Length);
            Debug.WriteLine("parentParametersValues :");
            Debug.WriteLine(parentParametersValues.ToString());
            Debug.WriteLine("returnFieldInfos :");
            Debug.WriteLine(returnFieldInfos.ToString());

            var returnFields = new ReturnFields();
            foreach (ReturnFieldInfo returnFieldInfo in returnFieldInfos.Values)
            {
                Debug.WriteLine("returnFieldInfo :");
                Debug.WriteLine(returnFieldInfo.ToString());
                var agregated = new ParametersValues();
                foreach (HtmlNode parentNode in parentNodes)
                {
                    Debug.Assert(parentNode != null);
                    var parametersValues = new ParametersValues();
                    string xpath = ParseRowTemplate(returnFieldInfo.ReturnFieldXpathTemplate, parentParametersValues);
                    Debug.WriteLine("xpath : " + xpath);
                    try
                    {
                        HtmlNodeCollection nodes = parentNode.SelectNodes(xpath);
                        if (nodes != null)
                            foreach (HtmlNode node in nodes)
                            {
                                Debug.Assert(node != null);
                                ParametersValues nodeParametersValues = new ParametersValues(parentParametersValues)
                                    .InsertOrReplace(
                                        BuildParametersValues(returnFieldInfo.ReturnFieldResultTemplate, node));
                                Debug.Assert(nodeParametersValues != null);
                                parametersValues.InsertOrAppend(nodeParametersValues);
                            }
                        else
                        {
                            Debug.WriteLine(MethodBase.GetCurrentMethod().Name + " : No nodes for " + xpath);
                        }
                        foreach (var pair in parametersValues)
                            if (!agregated.ContainsKey((pair.Key))) agregated.Add(pair.Key, pair.Value);
                            else if (agregated[pair.Key].Count < pair.Value.Count)
                                agregated[pair.Key] = pair.Value;
                    }
                    catch (Exception exception)
                    {
                        LastError = exception;
                        Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
                    }
                }
                Debug.WriteLine("agregated :");
                Debug.WriteLine(agregated.ToString());
                var regexReplacement = new Regex(returnFieldInfo.ReturnFieldRegexPattern,
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                string replacement = returnFieldInfo.ReturnFieldRegexReplacement;
                string matchPattern = returnFieldInfo.ReturnFieldRegexMatchPattern;
                var list = new List<string>();
                foreach (
                    string replace in
                        ParseColTemplate(returnFieldInfo.ReturnFieldResultTemplate, agregated)
                            .Select(input => regexReplacement.Replace(input, replacement).Trim())
                            .Where(replace => !String.IsNullOrEmpty(replace)))
                {
                    list.AddRange(from Match match in Regex.Matches(replace, matchPattern)
                        select match.Value.Trim()
                        into s
                        where !String.IsNullOrEmpty(s)
                        select s);
                }
                returnFields.Add(returnFieldInfo.ReturnFieldId, list);
            }
            Debug.WriteLine("returnFields :");
            Debug.WriteLine(returnFields.ToString());
            Debug.WriteLine("End " + MethodBase.GetCurrentMethod().Name);
            return returnFields;
        }

        public static Rectangle get_Rectangle(IWebElement webElement)
        {
            Debug.Assert(webElement != null);
            var rect = new Rectangle(0, 0, webElement.OffsetRectangle.Width, webElement.OffsetRectangle.Height);
            for (IWebElement current = webElement; current != null; current = current.OffsetParent)
            {
                Rectangle currentRect = current.OffsetRectangle;
                rect.X += currentRect.X;
                rect.Y += currentRect.Y;
            }
            return rect;
        }

        public static string get_XPath(IWebElement webElement)
        {
            Debug.Assert(webElement != null);
            string xpath = "";
            for (IWebElement parent = webElement.Parent;
                parent != null && !parent.IsNullOrEmpty();
                parent = parent.Parent)
            {
                int index = 0;
                foreach (IWebElement child in parent.Children)
                {
                    if (String.Compare(child.TagName, webElement.TagName, StringComparison.OrdinalIgnoreCase) ==
                        0)
                        index++;
                    if (child.Equals(webElement))
                    {
                        xpath = @"/" + webElement.TagName + "[" + index + "]" + xpath;
                        break;
                    }
                }
                webElement = parent;
            }
            return @"/" + xpath.ToLower();
        }

        public static string XPathToMask(string mask)
        {
            Debug.Assert(mask != null);
            mask = new Regex(@"(\/|\[|\])").Replace(mask, @"\$&");
            mask = new Regex(
                @"(\\\/table\\\[(?<i1>\d+)\\\])(\\\/(tbody|thead|tfoot)\\\[(\d+)\\\])?(\\\/tr\\\[(?<i2>\d+)\\\])")
                .Replace(mask, @"\/table\[${i1}\](\/(tbody|thead|tfoot)\[\d+\])?\/tr\[${i2}\]");
            mask = new Regex(@".+").Replace(mask, @"\B$&\B");
            return mask;
        }

        public static string XPathSanitize(string xpath)
        {
            Debug.Assert(xpath != null);
            xpath =
                new Regex(@"(?<start>\/(ol|ul|li|tr|td)\[)(?<i1>\d+)((\]\k<start>)(?<i2>\d+))+(?<end>\])").Replace(
                    xpath,
                    DoubleTagMatchEvaluator);
            return xpath;
        }

        protected static string DoubleTagMatchEvaluator(Match m)
        {
            int i = Convert.ToInt32(m.Groups["i1"].Value) +
                    m.Groups["i2"].Captures.Cast<Capture>().Sum(capture => Convert.ToInt32(capture.Value));
            return m.Groups["start"].Value + i + m.Groups["end"].Value;
        }

        public static string RegexEscape(string text)
        {
            Debug.Assert(text != null);
            var regex = new Regex(@"(\{|\[|\]|\})");
            return regex.Replace(text, @"\$&").Trim();
        }

        public static DateTime DateTimeParse(string s)
        {
            Debug.Assert(s != null);
            return DateTime.Parse(s);
        }

        #region

        /// <summary>
        ///     Формирование пар идентификатор параметра - значение параметра
        ///     для замены в строке-шаблоне
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public static ParametersValues BuildParametersValues(long pageId)
        {
            Debug.Assert(pageId != 0);
            Debug.WriteLine("Begin " + MethodBase.GetCurrentMethod().Name + " : " + pageId);
            var parametersValues = new ParametersValues();
            if (pageId > 1)
                parametersValues.Add(RegexEscape(@"{{PageId}}"), pageId.ToString(CultureInfo.InvariantCulture));
            Debug.WriteLine("parametersValues :");
            Debug.WriteLine(parametersValues.ToString());
            Debug.WriteLine("End " + MethodBase.GetCurrentMethod().Name);
            return parametersValues;
        }

        /// <summary>
        ///     Формирование пар идентификатор параметра - значение параметра
        ///     для замены в строке-шаблоне
        /// </summary>
        /// <param name="template"></param>
        /// <param name="webTask"></param>
        /// <returns></returns>
        public static ParametersValues BuildParametersValues(string template, IWebTask webTask)
        {
            Debug.Assert(template != null);
            Debug.Assert(webTask != null);
            Debug.WriteLine("Begin " + MethodBase.GetCurrentMethod().Name + " : " + template);
            var parametersValues = new ParametersValues();
            foreach (
                string name in
                    from Match match in Regex.Matches(template, FieldPattern)
                    select match.Groups["name"].Value)
            {
                try
                {
                    PropertyInfo propInfo = webTask.GetType().GetProperty(name);
                    string key = RegexEscape(@"{{" + name + @"}}");
                    string value = propInfo.GetValue(webTask, null).ToString();
                    Debug.WriteLine(key + " -> " + value);
                    parametersValues.Add(key, value);
                }
                catch (Exception exception)
                {
                    LastError = exception;
                    Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
                }
            }
            Debug.WriteLine("parametersValues :");
            Debug.WriteLine(parametersValues.ToString());
            Debug.WriteLine("End " + MethodBase.GetCurrentMethod().Name);
            return parametersValues;
        }

        /// <summary>
        ///     Формирование пар идентификатор параметра - значение параметра
        ///     для замены в строке-шаблоне
        /// </summary>
        /// <param name="template"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ParametersValues BuildParametersValues(string template, HtmlNode node)
        {
            Debug.Assert(template != null);
            Debug.Assert(node != null);
            Debug.WriteLine("Begin " + MethodBase.GetCurrentMethod().Name + " : " + template);
            var parametersValues = new ParametersValues();
            MethodInfo[] methodInfos =
            {
                typeof (MyLibrary).GetMethod("InvokeNodeProperty"),
                typeof (MyLibrary).GetMethod("AttributeValue")
            };

            foreach (
                string name in
                    from Match match in Regex.Matches(template, FieldPattern)
                    select match.Groups["name"].Value)
            {
                Debug.WriteLine(template + " <- " + name);
                foreach (MethodInfo methodInfo in methodInfos)
                {
                    try
                    {
                        Debug.WriteLine(MethodBase.GetCurrentMethod().Name + " invoke " + methodInfo.Name);
                        object value = methodInfo.Invoke(null, new object[] {node, name});
                        if (value != null) parametersValues.Add(RegexEscape(@"{{" + name + @"}}"), value.ToString());
                    }
                    catch (Exception exception)
                    {
                        LastError = exception;
                        Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
                    }
                }
            }

            Debug.WriteLine("parametersValues :");
            Debug.WriteLine(parametersValues.ToString());
            Debug.WriteLine("End " + MethodBase.GetCurrentMethod().Name);
            return parametersValues;
        }

        #endregion

        #region Получение значения параметра

        /// <summary>
        ///     Получение значения указанного аттрибута указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string AttributeValue(HtmlNode node, string attributeName)
        {
            Debug.Assert(node != null);
            Debug.Assert(attributeName != null);
            HtmlAttribute attribute = node.Attributes[attributeName];
            if (attribute != null) return attribute.Value;
            return null;
        }

        /// <summary>
        ///     Получение значения указанного свойства указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string InvokeNodeProperty(HtmlNode node, string propertyName)
        {
            Debug.Assert(node != null);
            Debug.Assert(propertyName != null);
            PropertyInfo propertyInfo = typeof (HtmlNode).GetProperty(propertyName);
            if (propertyInfo != null) return (string) propertyInfo.GetValue(node, null);
            return null;
        }

        #endregion
    }
}