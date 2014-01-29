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
    /// Класс вспомогательных алгоритмов
    /// </summary>
    public class MyParserLibrary
    {
        public object LastError { get; set; }
        /// <summary>
        /// Запрос к сайту с использованием HtmlWeb
        /// </summary>
        public HtmlDocument WebRequestHtmlDocument(string url, string method)
        {
            HtmlWeb hw = new HtmlWeb { AutoDetectEncoding = true };
            return hw.Load(url, method);
        }


        /// <summary>
        /// Замена в строке-шаблоне идентификаторов-параметров на их значения
        /// </summary>        
        public string ParseTemplate(string template, Arguments args)
        {
            foreach (KeyValuePair<string, string> pair in args)
            {
                Debug.Assert(pair.Key != null);
                Debug.Assert(pair.Value != null);
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
        public ReturnFields BuildReturnFields(
            HtmlNode parentNode,
            Arguments parentArguments,
            ReturnFieldInfos returnFieldInfos)
        {
            ReturnFields returnFields = new ReturnFields();
            foreach (var returnFieldInfo in returnFieldInfos.Values)
            {
                Regex rgxReplace = new Regex(returnFieldInfo.ReturnFieldRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex rgxSelect = new Regex(returnFieldInfo.ReturnFieldRegexSelect, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var nodes = parentNode.SelectNodes(ParseTemplate(returnFieldInfo.ReturnFieldXpath, parentArguments));
                var list = new List<string>();
                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        Arguments arguments = new Arguments(parentArguments);
                        arguments.InsertOrReplaceArguments(
                            BuildArguments(returnFieldInfo.ReturnFieldResult, node));
                        string value = rgxReplace.Replace(
                                ParseTemplate(returnFieldInfo.ReturnFieldResult, arguments),
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
        /// Формирование пар идентификатор параметра <-> значение параметра
        /// для замены в строке-шаблоне
        /// </summary>
        /// <param name="pageId"></param>
        /// <returns></returns>
        public Arguments BuildArguments(long pageId)
        {
            Arguments arguments = new Arguments();
            if (pageId > 1) arguments.Add(@"\{\{PageId\}\}", pageId.ToString(CultureInfo.InvariantCulture));
            return arguments;
        }

        /// <summary>
        /// Формирование пар идентификатор параметра <-> значение параметра
        /// для замены в строке-шаблоне
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public Arguments BuildArguments(WebTask task)
        {
            Arguments arguments = new Arguments();
            foreach (var prop in task.GetType().GetProperties())
            {
                try
                {
                    string propName = @"\{\{" + prop.Name + @"\}\}";
                    if (!arguments.ContainsKey(propName))
                    {
                        string value = prop.GetValue(task, null).ToString();
                        Debug.WriteLine(propName + " -> " + value);
                        arguments.Add(propName, value);
                    }
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
            }
            return arguments;
        }

        /// <summary>
        /// Формирование пар идентификатор параметра <-> значение параметра
        /// для замены в строке-шаблоне
        /// </summary>
        /// <param name="template"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public Arguments BuildArguments(string template, HtmlNode node)
        {
            Debug.Assert(node != null);
            Arguments args = new Arguments();

            Regex regex = new Regex(@"\{\{[^\}]+\}\}");
            foreach (string name in from Match match in regex.Matches(template) select match.Value.Replace(@"{{", @"").Replace(@"}}", @""))
            {
                Debug.WriteLine(template + " <- " + name);
                try
                {
                    args.Add(@"\{\{" + name + @"\}\}", InvokeNodeProperty(node, name));
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
                try
                {
                    args.Add(@"\{\{" + name + @"\}\}", AttributeValue(node, name));
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
            }

            return args;
        }

        #endregion


        #region Получение значения параметра

        /// <summary>
        /// Получение значения указанного аттрибута указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        private static string AttributeValue(HtmlNode node, string attributeName)
        {
            HtmlAttribute attribute = node.Attributes[attributeName];
            return attribute.Value;
        }

        /// <summary>
        /// Получение значения указанного свойства указанного нода
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static string InvokeNodeProperty(HtmlNode node, string propertyName)
        {
            Type type = typeof(HtmlNode);
            Debug.Assert(type != null, "type != null");
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            return (string)propertyInfo.GetValue(node, null);
        }

        #endregion

    }
}
