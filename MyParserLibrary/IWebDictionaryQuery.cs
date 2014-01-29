using System.Collections.Generic;

namespace MyParserLibrary
{
    public interface IWebDictionaryQuery : IWebQuery
    {
        Dictionary<string, string> Dictionary { get; set; }
        void OnQueryComplite(WebTask task);
        void Query();
    }
}