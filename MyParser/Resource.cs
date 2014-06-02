using System;
using System.Reflection;
using MyLibrary;
using MyLibrary.Attributes;
using String = MyLibrary.Types.String;

namespace MyParser
{
    public class Resource : RequestProperties, IValueable
    {
        public Resource(string s)
            : base(s)
        {
        }

        [Value]
        public new DateTime PublicationDatetime
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
        public new object PublicationId
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

        public override string ToString()
        {
            return String.Parse(new Transformation().ParseTemplate(ToValues()));
        }
    }
}