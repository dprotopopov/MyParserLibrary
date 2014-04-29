using System;
using System.Reflection;
using System.Windows.Forms;
using MyLibrary.Attribute;

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
        [Value]
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
        [Value]
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
        [Value]
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
        [Value]
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
        [Value]
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
        [Value]
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
    }
}