using System.Collections.Generic;
using System.Diagnostics;

namespace MyParser.Library
{
    public class WebDictionaryQuery : WebQuery, IWebDictionaryQuery
    {
        public WebDictionaryQuery()
        {
            Dictionary = new Dictionary<string, string>();
            Option = Defaults.ReturnFieldInfos.Option.ReturnFieldId;
            Value = Defaults.ReturnFieldInfos.Value.ReturnFieldId;
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Option);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Value);
        }

        public string Option { get; set; }
        public string Value { get; set; }

        public Dictionary<string, string> Dictionary { get; set; }

        public override void OnQueryComplite(IWebTask webTask)
        {
            Debug.Assert(webTask == this);
            List<string> optionList = ReturnFields[Option];
            List<string> valueList = ReturnFields[Value];
            for (int i = 0; i < optionList.Count && i < valueList.Count; i++)
            {
                string key = optionList[i];
                string value = valueList[i];
                if (!Dictionary.ContainsKey(key))
                {
                    Dictionary.Add(key, value);
                }
            }
            base.OnQueryComplite(webTask);
        }
    }
}