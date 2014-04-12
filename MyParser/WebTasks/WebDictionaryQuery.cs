using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MyParser.WebTasks
{
    public class WebDictionaryQuery : WebQuery, IWebDictionaryQuery
    {
        public WebDictionaryQuery()
        {
            Dictionary = new Dictionary<string, string>();
            Option = Defaults.ReturnFieldInfos.Option.First().ReturnFieldId.ToString();
            Value = Defaults.ReturnFieldInfos.Value.First().ReturnFieldId.ToString();
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Option);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Value);
        }

        public string Option { get; set; }
        public string Value { get; set; }

        public Dictionary<string, string> Dictionary { get; set; }

        public override void OnQueryComplite(IWebTask webTask)
        {
            Debug.Assert(webTask == this);
            IEnumerable<string> optionList = ReturnFields[Option];
            IEnumerable<string> valueList = ReturnFields[Value];
            for (int i = 0; i < optionList.Count() && i < valueList.Count(); i++)
            {
                string key = optionList.ToList()[i];
                string value = valueList.ToList()[i];
                if (!Dictionary.ContainsKey(key))
                {
                    Dictionary.Add(key, value);
                }
            }
            base.OnQueryComplite(webTask);
        }
    }
}