using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyLibrary;
using MyLibrary.Attributes;
using MyLibrary.Collections;
using String = MyLibrary.Types.String;

namespace MyParser
{
    [Serializable]
    public class ReturnFieldInfos : Dictionary<string, IEnumerable<ReturnFieldInfo>>, IValueable
    {
        public ReturnFieldInfos(IEnumerable<ReturnFieldInfo> list)
        {
            Add(list);
        }

        public ReturnFieldInfos()
        {
        }

        [Value]
        public IEnumerable<ReturnFieldInfo> OptionRedirect
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> ValueRedirect
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> Subdomain
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> Url
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> Email
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> Phone
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> PublicationId
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> PublicationLink
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> PublicationDatetime
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> OtherPageUrl
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> Option
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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
        public IEnumerable<ReturnFieldInfo> Value
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
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

        public Values ToValues()
        {
            return new Values(this);
        }

        public IEnumerable<ReturnFieldInfo> ToList()
        {
            var list = new StackListQueue<ReturnFieldInfo>();
            foreach (var value in Values)
                list.AddRange(value.ToList());
            return list;
        }

        public override string ToString()
        {
            var values = new Values();
            foreach (var pair in this)
                values.Add(new Values
                {
                    Key = Enumerable.Repeat(pair.Key, pair.Value.Count()),
                    Value = pair.Value.Select(item => item.ToString()),
                });
            return String.Parse(new Transformation().ParseTemplate(values));
        }

        public void Add(ReturnFieldInfo returnFieldInfo)
        {
            string key = returnFieldInfo.ReturnFieldId.ToString();
            if (!ContainsKey(key)) Add(key, new StackListQueue<ReturnFieldInfo> {returnFieldInfo});
            else this[key] = new StackListQueue<ReturnFieldInfo>(this[key]) {returnFieldInfo};
        }

        public void Add(IEnumerable<ReturnFieldInfo> returnFieldInfos)
        {
            foreach (ReturnFieldInfo returnFieldInfo in returnFieldInfos)
                Add(returnFieldInfo);
        }
    }
}