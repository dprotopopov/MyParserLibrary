using System.Collections.Generic;
using System.Diagnostics;

namespace MyParserLibrary
{
    public class WebDictionaryQuery : WebQuery, IWebDictionaryQuery
    {
        public WebDictionaryQuery()
        {
            Dictionary = new Dictionary<string, string>();
            ReturnFieldInfos.Add(Defaults.DefaultOptionlInfo);
            ReturnFieldInfos.Add(Defaults.DefaultValuelInfo);
        }

        public Dictionary<string, string> Dictionary { get; set; }

        public override void OnQueryComplite(WebTask task)
        {
            Debug.Assert(task == this);
            List<string> optionList = ReturnFields.Option;
            List<string> valueList = ReturnFields.Value;
            for (int i = 0; i < optionList.Count && i < valueList.Count; i++)
            {
                string key = optionList[i];
                string value = valueList[i];
                if (!Dictionary.ContainsKey(key))
                {
                    Dictionary.Add(key, value);
                }
            }
            base.OnQueryComplite(task);
        }
    }
}