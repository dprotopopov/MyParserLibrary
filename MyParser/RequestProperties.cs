using System;
using System.Collections.Generic;
using System.Reflection;
using MyLibrary.Attributes;

namespace MyParser
{
    public class RequestProperties : MyLibrary.Collections.Properties, IValueable
    {
        public RequestProperties(string s)
            : base(s)
        {
        }

        public RequestProperties()
        {
        }

        public RequestProperties(object obj, IEnumerable<string> propertyNames) : base(obj, propertyNames)
        {
        }

        [Value]
        public object Site
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, 0);
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
        public DateTime PublicationDatetime
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, MyLibrary.Types.DateTime.Default);
                return (DateTime) this[propertyName];
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
        public object PublicationId
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, 0);
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

        public void Add(Dictionary<string, object> dictionary)
        {
            foreach (var pair in dictionary)
            {
                if (ContainsKey(pair.Key)) this[pair.Key] = pair.Value;
                else Add(pair.Key, pair.Value);
            }
        }
    }
}