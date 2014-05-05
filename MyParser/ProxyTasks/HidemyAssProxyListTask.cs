using MyLibrary.Types;

namespace MyParser.ProxyTasks
{
    public class HidemyAssProxyListTask : ProxyListTask
    {
        public HidemyAssProxyListTask()
        {
            Url = @"https://hidemyass.com/proxy-list/";
            Page = string.Empty;
            ReturnFieldInfos = new ReturnFieldInfos
            {
                new ReturnFieldInfo
                {
                    ReturnFieldId = Database.AddressColumn,
                    ReturnFieldXpathTemplate = @"//table[@id='listtable']//tr",
                    ReturnFieldResultTemplate = @"{{HidemyAssOuterHtml}}",
                    ReturnFieldRegexPattern =
                        @"\<tr[^\>]*\>\s*\<td[^\>]*\>\s*[^\<]*\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*(\d+)\s*\<\/td[^\>]*\>([^\<]*\<[^\>]*\>[^\<]*){6}\<td[^\>]*\>\s*(\w+)\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*[^\<]*\s*\<\/td[^\>]*\>\s*\<\/tr[^\>]*\>",
                    ReturnFieldRegexReplacement = @"$1",
                },
                new ReturnFieldInfo
                {
                    ReturnFieldId = Database.PortColumn,
                    ReturnFieldXpathTemplate = @"//table[@id='listtable']//tr",
                    ReturnFieldResultTemplate = @"{{HidemyAssOuterHtml}}",
                    ReturnFieldRegexPattern =
                        @"\<tr[^\>]*\>\s*\<td[^\>]*\>\s*[^\<]*\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*(\d+)\s*\<\/td[^\>]*\>([^\<]*\<[^\>]*\>[^\<]*){6}\<td[^\>]*\>\s*(\w+)\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*[^\<]*\s*\<\/td[^\>]*\>\s*\<\/tr[^\>]*\>",
                    ReturnFieldRegexReplacement = @"$2",
                },
                new ReturnFieldInfo
                {
                    ReturnFieldId = Database.SchemaColumn,
                    ReturnFieldXpathTemplate = @"//table[@id='listtable']//tr",
                    ReturnFieldResultTemplate = @"{{HidemyAssOuterHtml}}",
                    ReturnFieldRegexPattern =
                        @"\<tr[^\>]*\>\s*\<td[^\>]*\>\s*[^\<]*\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*(\d+)\s*\<\/td[^\>]*\>([^\<]*\<[^\>]*\>[^\<]*){6}\<td[^\>]*\>\s*(\w+)\s*\<\/td[^\>]*\>\s*\<td[^\>]*\>\s*[^\<]*\s*\<\/td[^\>]*\>\s*\<\/tr[^\>]*\>",
                    ReturnFieldRegexReplacement = @"$4",
                },
            };
            Crawler = new Crawler
            {
                Compression = Defaults.Compression,
                Encoding = Defaults.Encoding,
                CompressionManager = Defaults.CompressionManager,
                Database = Defaults.Database,
                Edition = (int) DocumentEdition.Tided,
            };
        }


        public string Page { get; set; }

        public new void Reset()
        {
            base.Reset();
            Page = ReturnFields.MaxCount == 0
                ? string.Empty
                : string.Format("{0}", Int32.ParseAsString(Page) + 1);
        }
    }
}