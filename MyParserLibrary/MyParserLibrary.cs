using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace MyParserLibrary
{
    /// <summary>
    /// Статический класс вспомогательных алгоритмов
    /// </summary>
    internal static class MyParserLibrary
    {

        /// <summary>
        /// Запрос к сайту с использованием HtmlWeb
        /// </summary>
        public static HtmlDocument WebRequestHtmlDocument(string url, string method)
        {
            HtmlWeb hw = new HtmlWeb { AutoDetectEncoding = true };
            return hw.Load(url, method);
        }


        /// <summary>
        /// Замена в строке-шаблоне идентификаторов-параметров на их значения
        /// </summary>        
        public static string ParseTemplate(string template, Arguments args)
        {
            foreach (KeyValuePair<string, string> pair in args)
            {
                Debug.Assert(pair.Key != null);
                Debug.Assert(pair.Value != null);

                //Debug.WriteLine("ParseTemplate: /" + pair.Key + "/ -> /" + pair.Value.Substring(0, Math.Min(30, pair.Value.Length)) + ((pair.Value.Length > 30) ? ".../" : "/"));
                Regex regex = new Regex(pair.Key, RegexOptions.IgnoreCase);
                template = regex.Replace(template, pair.Value);
            }
            Regex rgx = new Regex(@"\{\{[^\}]*\}\}", RegexOptions.IgnoreCase);
            template = rgx.Replace(template, @"");
            Debug.WriteLine("ParseTemplate: " + template);
            return template;
        }

        /// <summary>
        /// Поиск и формирование значений возвращаемых полей загруженного с сайта объявления
        /// </summary>        
        public static ReturnFields BuildReturnFields(
            HtmlNode parentNode,
            Arguments parentArguments,
            ReturnFieldInfos returnFieldInfos)
        {
            ReturnFields returnFields = new ReturnFields();
            foreach (var returnFieldInfo in returnFieldInfos)
            {
                Regex rgxReplace = new Regex(returnFieldInfo.ReturnFieldRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex rgxSelect = new Regex(returnFieldInfo.ReturnFieldRegexSelect, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var nodes = parentNode.SelectNodes(ParseTemplate(returnFieldInfo.ReturnFieldXpathTemplate, parentArguments));
                var list = new List<string>();
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        Arguments arguments = new Arguments(parentArguments);
                        arguments.InsertOrReplaceArguments(
                            BuildArguments(returnFieldInfo.ReturnFieldResultTemplate, node));
                        string value = rgxReplace.Replace(
                                ParseTemplate(returnFieldInfo.ReturnFieldResultTemplate, arguments),
                                        returnFieldInfo.ReturnFieldRegexReplacement);

                        list.AddRange(from Match match in rgxSelect.Matches(value) select match.Value);
                    }
                }
                else
                {
                    Debug.WriteLine("BuildReturnFields: parentNode.SelectNodes: No nodes found");
                }
                returnFields.Add(returnFieldInfo.ReturnFieldId, list);
            }
            return returnFields;
        }

        #region

        /// <summary>
        /// Формирование значений идентификаторов-параметров
        /// для замены в строке-шаблоне
        /// </summary>        
        public static Arguments BuildArguments(long pageId)
        {
            Arguments arguments = new Arguments();
            if (pageId > 1) arguments.Add(@"\{\{PageId\}\}", pageId.ToString(CultureInfo.InvariantCulture));
            return arguments;
        }

        /// <summary>
        /// Формирование значений идентификаторов-параметров
        /// для замены в строке-шаблоне
        /// </summary>        
        public static Arguments BuildArguments(string url, string method)
        {
            Arguments arguments = new Arguments { { @"\{\{Url\}\}", url }, { @"\{\{Method\}\}", method } };
            return arguments;
        }

        /// <summary>
        /// Формирование значений идентификаторов-параметров
        /// для замены в строке-шаблоне
        /// </summary>        
        public static Arguments BuildArguments(string template, HtmlNode node)
        {
            Debug.Assert(node != null);
            try
            {
                Arguments args = new Arguments();

                Regex regex = new Regex(@"\{\{[^\}]+\}\}");
                foreach (Match match in regex.Matches(template))
                {
                    string name = match.Value.Replace(@"{{", @"").Replace(@"}}", @"");
                    try
                    {
                        args.Add(@"\{\{" + name + @"\}\}", InvokeNodeProperty(node, name));
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        args.Add(@"\{\{" + name + @"\}\}", AttributeValue(node, name));
                    }
                    catch (Exception)
                    {
                    }
                }
                
                return args;
            }
            catch (Exception)
            {
                return new Arguments();
            }
        }

        #endregion


        #region 

        /// <summary>
        /// Получение значения указанного аттрибута указанного нода
        /// </summary>        
        public static string AttributeValue(HtmlNode node, string attributeName)
        {
            try
            {
                HtmlAttribute attribute = node.Attributes[attributeName];
                return attribute.Value;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Получение значения указанного свойства указанного нода
        /// </summary>        
        public static string InvokeNodeProperty(HtmlNode node, string propertyName)
        {
            Type type = typeof (HtmlNode);
            Debug.Assert(type != null, "type != null");
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            Debug.Assert(propertyInfo != null, "propertyInfo != null");
            return (string) propertyInfo.GetValue(node, null);
        }

        #endregion

    }
}
