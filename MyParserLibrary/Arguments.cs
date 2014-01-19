using System.Collections.Generic;
using System.Reflection;

namespace MyParserLibrary
{
    public class Arguments : Dictionary<string, string>
    {
        public Arguments(Arguments args)
        {
            InsertOrReplaceArguments(args);
        }

        public Arguments()
        {
        }

        public Arguments InsertOrReplaceArguments(Dictionary<string, string> dictionary)
        {
            foreach (var arg in dictionary)
                if (ContainsKey(arg.Key))
                    this[arg.Key] = arg.Value;
                else
                    Add(arg.Key, arg.Value);
            return this;
        }

        public string Url
        {
            get
            {
                string propertyName = @"\{\{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"\}\}";
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = @"\{\{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"\}\}";
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        public string Method
        {
            get
            {
                string propertyName = @"\{\{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"\}\}";
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = @"\{\{" + MethodBase.GetCurrentMethod().Name.Substring(4) + @"\}\}";
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
    }
}
