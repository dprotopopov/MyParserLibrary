using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace MyParser.Library
{
    public class ParametersValues : DictionaryOfList
    {
        public ParametersValues(ParametersValues parametersValues)
        {
            InsertOrReplace(parametersValues);
        }

        public ParametersValues()
        {
        }

        public ParametersValues(ReturnFields returnFields)
        {
            foreach (var returnField in returnFields)
            {
                Add(MyLibrary.RegexEscape(@"{{" + returnField.Key + @"}}"), returnField.Value);
            }
        }

        public List<string> Url
        {
            get
            {
                string propertyName =
                    MyLibrary.RegexEscape(@"{{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"}}");
                if (!ContainsKey(propertyName)) Add(propertyName, new List<string>());
                return this[propertyName];
            }
            set
            {
                string propertyName =
                    MyLibrary.RegexEscape(@"{{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"}}");
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }

        public List<string> Method
        {
            get
            {
                string propertyName =
                    MyLibrary.RegexEscape(@"{{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"}}");
                if (!ContainsKey(propertyName)) Add(propertyName, new List<string>());
                return this[propertyName];
            }
            set
            {
                string propertyName =
                    MyLibrary.RegexEscape(@"{{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"}}");
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }

        public List<string> Id
        {
            get
            {
                string propertyName =
                    MyLibrary.RegexEscape(@"{{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"}}");
                if (!ContainsKey(propertyName)) Add(propertyName, new List<string>());
                return this[propertyName];
            }
            set
            {
                string propertyName =
                    MyLibrary.RegexEscape(@"{{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"}}");
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }


        public ParametersValues InsertOrAppend(ParametersValues dictionary)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (DictionaryOfList)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {dictionary};
            return (ParametersValues) methodInfo.Invoke(this, objects);
        }

        public ParametersValues InsertOrReplace(ParametersValues dictionary)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (DictionaryOfList)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = { dictionary };
            return (ParametersValues) methodInfo.Invoke(this, objects);
        }
    }
}