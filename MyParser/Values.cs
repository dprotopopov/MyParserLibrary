using System;
using System.Collections.Generic;
using System.Reflection;
using MyLibrary.Attributes;
using MyLibrary.Collections;

namespace MyParser
{
    public class Values : MyLibrary.Values, IValueable
    {
        public Values(IEnumerable<KeyValuePair<string, IEnumerable<string>>> list)
            : base(list)
        {
        }

        public Values()
        {
        }

        public Values(Object obj)
            : base(obj)
        {
        }

        public Values(IEnumerable<string> keys, IEnumerable<string> values)
            : base(keys, values)
        {
        }

        public Values(IEnumerable<KeyValuePair<string, IEnumerable<string>>> slice, int count)
            : base(slice, count)
        {
        }

        [Value]
        public new IEnumerable<string> Url
        {
            get
            {
                string propertyName =
                    MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
                return this[propertyName];
            }
            set
            {
                string propertyName =
                    MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }

        [Value]
        public IEnumerable<string> Method
        {
            get
            {
                string propertyName =
                    MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
                return this[propertyName];
            }
            set
            {
                string propertyName =
                    MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }

        [Value]
        public new IEnumerable<string> Id
        {
            get
            {
                string propertyName =
                    MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, new StackListQueue<string>());
                return this[propertyName];
            }
            set
            {
                string propertyName =
                    MethodBase.GetCurrentMethod().Name.Substring(4);
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

        public new Values SqlEscape()
        {
            return new Values(base.SqlEscape());
        }

        public new Values Slice(int row)
        {
            return new Values(base.Slice(row));
        }

        public new Values Slice(IEnumerable<string> keys)
        {
            return new Values(base.Slice(keys));
        }
    }
}