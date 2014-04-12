using System.Collections.Generic;

namespace MyParser.WebTasks
{
    public interface IWebDictionaryQuery : IWebQuery
    {
        Dictionary<string, string> Dictionary { get; set; }
        string Option { get; set; }
        string Value { get; set; }
        void OnQueryComplite(IWebTask webTask);
        void Query();
    }
}