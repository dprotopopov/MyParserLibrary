using System.Collections.Generic;
using System.Reflection;

namespace MyParserLibrary
{
    /// <summary>
    /// Параметры для процедуры возвращающей вычисленные поля при разборе страницы
    /// </summary>
    public class ReturnFieldInfo : Dictionary<string, string>
    {
        /// <summary>
        /// Стандартный конвертер содержания в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ReturnFieldId;
        }

        /// <summary>
        /// Идентификатор возвращаемого поля
        /// </summary>
        public string ReturnFieldId
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Xpath для нахождения поля на загруженной странице
        /// </summary>
        public string ReturnFieldXpathTemplate
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон возвращаемого найденого текста 
        /// </summary>
        public string ReturnFieldResultTemplate
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон регулярного выражения, 
        /// используемого при замене найденого текста
        /// </summary>
        public string ReturnFieldRegexPattern
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон для замены у регулярного выражения, 
        /// используемого при замене найденого текста 
        /// </summary>
        public string ReturnFieldRegexReplacement
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон регулярного выражения для выборки из найденого текста 
        /// </summary>
        public string ReturnFieldRegexSelect
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
    }
}
