﻿using System.Collections.Generic;
using System.Reflection;
using MyLibrary.Collections;

namespace MyParser
{
    /// <summary>
    ///     Вспомогательный класс
    ///     Используется для доступа к значениям словаря по ключу
    /// </summary>
    public class ReturnFields : MyLibrary.ReturnFields, IValueable
    {
        /// <summary>
        ///     Предопределённое имя возвращаемых полей
        /// </summary>
        public IEnumerable<string> PublicationId
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
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
        ///     Предопределённое имя возвращаемых полей
        /// </summary>
        public IEnumerable<string> Url
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
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
        ///     Предопределённое имя возвращаемых полей
        /// </summary>
        public IEnumerable<string> OtherPageUrl
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
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
        ///     Предопределённое имя возвращаемых полей
        /// </summary>
        public IEnumerable<string> Email
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
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
        ///     Предопределённое имя возвращаемых полей
        /// </summary>
        public IEnumerable<string> Phone
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
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
        ///     Предопределённое имя возвращаемых полей
        /// </summary>
        public new IEnumerable<string> Option
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
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
        ///     Предопределённое имя возвращаемых полей
        /// </summary>
        public new IEnumerable<string> Value
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
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
    }
}