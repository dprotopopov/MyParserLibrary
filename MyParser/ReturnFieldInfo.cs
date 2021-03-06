﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using MyLibrary.Attributes;

namespace MyParser
{
    /// <summary>
    ///     Параметры для процедуры возвращающей вычисленные поля при разборе страницы
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ReturnFieldInfo : MyLibrary.Collections.Properties, IValueable
    {
        /// <summary>
        ///     Идентификатор сайта
        /// </summary>
        [Value]
        [Description("Идентификатор сайта в базе данных")]
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
        [Description("Идентификатор возвращаемого поля")]
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
        [Description("Шаблон xpath для возвращаемого поля")]
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
        [Description("Шаблон текста возвращаемого поля")]
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
        [Description("Шаблон регулярного выражения, используемого при замене найденого текста")]
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
        [Description("Шаблон для замены у регулярного выражения, используемого при замене найденого текста")]
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

        /// <summary>
        /// </summary>
        [Value]
        [Description("Строка для соединения найденных текстов")]
        public object JoinSeparator
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