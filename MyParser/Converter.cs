using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace MyParser
{
    public class Converter : IValueable
    {
        private const string ParseMethodName = "Parse";
        private const string ParseAsListMethodName = "ParseAsList";

        private static readonly Dictionary<Type, Type> BaseTypeDictionary = new Dictionary<Type, Type>
        {
            {typeof (string), typeof (String)},
            {typeof (bool), typeof (Boolean)},
            {typeof (int), typeof (Int32)},
            {typeof (uint), typeof (UInt32)},
            {typeof (decimal), typeof (Decimal)},
            {typeof (Uri), typeof (Uri)},
            {typeof (DateTime), typeof (DateTime)},
            {typeof (IList<string>), typeof (String)},
            {typeof (IList<Uri>), typeof (Uri)},
        };

        private static readonly Dictionary<Type, string> MethodNameDictionary = new Dictionary<Type, string>
        {
            {typeof (string), ParseMethodName},
            {typeof (bool), ParseMethodName},
            {typeof (int), ParseMethodName},
            {typeof (uint), ParseMethodName},
            {typeof (decimal), ParseMethodName},
            {typeof (Uri), ParseMethodName},
            {typeof (DateTime), ParseMethodName},
            {typeof (IList<string>), ParseAsListMethodName},
            {typeof (IList<Uri>), ParseAsListMethodName},
        };

        private static readonly Dictionary<Type, Type> FromDictionary = new Dictionary<Type, Type>
        {
            {typeof (string), typeof (string)},
            {typeof (IEnumerable<string>), typeof (IEnumerable<string>)},
            {typeof (IList<string>), typeof (IEnumerable<string>)},
            {typeof (List<string>), typeof (IEnumerable<string>)},
        };

        public Converter()
        {
            ModuleNamespace = GetType().Namespace;
        }

        public string ModuleNamespace { get; set; }

        public Values ToValues()
        {
            return new Values(this);
        }

        public object Convert(object obj, Type toType)
        {
            Debug.Assert(FromDictionary.ContainsKey(obj.GetType()));
            Debug.Assert(BaseTypeDictionary.ContainsKey(toType));
            Debug.Assert(MethodNameDictionary.ContainsKey(toType));
            string className = string.Format("{0}.Types.{1}", ModuleNamespace, BaseTypeDictionary[toType].Name);
            Type type = Type.GetType(className);
            MethodInfo parseMethodInfo = type.GetMethod(MethodNameDictionary[toType],
                new[] {FromDictionary[obj.GetType()]});

            Debug.WriteLine("Invoke {0} of {1}", parseMethodInfo.Name, type.Name);

            return parseMethodInfo.Invoke(null, new[] {obj});
        }
    }
}