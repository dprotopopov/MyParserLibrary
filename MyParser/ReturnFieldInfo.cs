using System;
using System.Reflection;
using System.Windows.Forms;

namespace MyParser
{
    /// <summary>
    ///     Параметры для процедуры возвращающей вычисленные поля при разборе страницы
    /// </summary>
    [Serializable]
    public class ReturnFieldInfo : MyLibrary.Collections.Properties, IValueable
    {
        /// <summary>
        ///     Идентификатор сайта
        /// </summary>
        public object SiteId
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, string.Empty);
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
        ///     Идентификатор возвращаемого поля
        /// </summary>
        public object ReturnFieldId
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, string.Empty);
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
        ///     Xpath для нахождения поля на загруженной странице
        /// </summary>
        public object ReturnFieldXpathTemplate
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, string.Empty);
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
        ///     Шаблон возвращаемого найденого текста
        /// </summary>
        public object ReturnFieldResultTemplate
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, string.Empty);
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
        ///     Шаблон регулярного выражения,
        ///     используемого при замене найденого текста
        /// </summary>
        public object ReturnFieldRegexPattern
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, string.Empty);
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
        ///     Шаблон для замены у регулярного выражения,
        ///     используемого при замене найденого текста
        /// </summary>
        public object ReturnFieldRegexReplacement
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, string.Empty);
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


        public new Values ToValues()
        {
            return new Values(this);
        }

        public ListViewItem ToListViewItem()
        {
            var viewItem = new ListViewItem(ReturnFieldId.ToString());
            viewItem.SubItems.Add(ReturnFieldXpathTemplate.ToString());
            viewItem.SubItems.Add(ReturnFieldResultTemplate.ToString());
            viewItem.SubItems.Add(ReturnFieldRegexPattern.ToString());
            viewItem.SubItems.Add(ReturnFieldRegexReplacement.ToString());
            return viewItem;
        }

        /// <summary>
        ///     Стандартный конвертер содержания в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", SiteId, ReturnFieldId, ReturnFieldXpathTemplate,
                ReturnFieldResultTemplate, ReturnFieldRegexPattern, ReturnFieldRegexReplacement);
        }
    }
}