using System;
using System.ComponentModel;
using System.Reflection;
using MyLibrary.Attributes;

namespace MyParser
{
    [Serializable]
    [TypeConverter(typeof (ExpandableObjectConverter))]
    public class TableBuilderInfo : MyLibrary.Collections.Properties, IValueable
    {
        [Value]
        [Description("������������� ����� � ���� ������")]
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

        [Value]
        [Description("����� �������� ������� � �����")]
        public object Method
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
        ///     ������������� ��������� �����
        ///     windows-1351
        ///     utf-8
        ///     ...
        /// </summary>
        [Value]
        [Description("������������� ��������� �����")]
        public object Encoding
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

        [Value]
        [Description("������������� ����������� ���������� ����������� ������")]
        public object Compression
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

        [Value]
        [Description("������� ����� ������������ ������������ �������� ��������")]
        public object Edition
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

        [Value]
        [Description("������ ������, ������������ � POST �������")]
        public object RequestTemplate
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

        [Value]
        [Description("������������� �������")]
        public object TableName
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

        [Value]
        [Description("������������� ������� � �������")]
        public object FieldName
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

        [Value]
        [Description("������ �������")]
        public object UrlTemplate
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

        [Value]
        [Description("������ xpath ��� ��������")]
        public object XpathTemplate
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


        [Value]
        [Description("������ ��������")]
        public object ResultTemplate
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

        [Value]
        [Description("������ ����������� ��������� ��� ��������")]
        public object RegexPattern
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

        [Value]
        [Description("������ ������ ����������� ��������� ��� ��������")]
        public object RegexReplacement
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
        [Description("������ ��� ���������� ��������� �������")]
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
    }
}