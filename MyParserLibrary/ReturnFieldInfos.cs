using System.Collections.Generic;

namespace MyParserLibrary
{
    public class ReturnFieldInfos : Dictionary<string, ReturnFieldInfo>
    {
        public void Add(ReturnFieldInfo info)
        {
            string key = info.ReturnFieldId;
            if (ContainsKey(key))
            {
                this[key] = info;
            }
            else
            {
                Add(key, info);
            }
        }
    }
}
