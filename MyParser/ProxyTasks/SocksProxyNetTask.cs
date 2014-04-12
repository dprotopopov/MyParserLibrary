using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Int32 = MyLibrary.Types.Int32;

namespace MyParser.ProxyTasks
{
    public sealed class SocksProxyNetTask : ProxyListTask
    {
        public SocksProxyNetTask()
        {
            Url = @"http://socks-proxy.net";
            Page = string.Empty;
            ReturnFieldInfos = new ReturnFieldInfos
            {
                new ReturnFieldInfo
                {
                    ReturnFieldId = Database.AddressColumn,
                    ReturnFieldXpathTemplate = @"/",
                    ReturnFieldResultTemplate = @"{{InnerHtml}}",
                    ReturnFieldRegexPattern =
                        @"\<tr[^\>]*\><td[^\>]*\>(\d+\.\d+\.\d+\.\d+)\<\/td\>\<td[^\>]*\>(\d+)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<\/tr\>",
                    ReturnFieldRegexReplacement = @"$1",
                },
                new ReturnFieldInfo
                {
                    ReturnFieldId = Database.PortColumn,
                    ReturnFieldXpathTemplate = @"/",
                    ReturnFieldResultTemplate = @"{{InnerHtml}}",
                    ReturnFieldRegexPattern =
                        @"\<tr[^\>]*\><td[^\>]*\>(\d+\.\d+\.\d+\.\d+)\<\/td\>\<td[^\>]*\>(\d+)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<td[^\>]*\>([^\<]*)\<\/td\>\<\/tr\>",
                    ReturnFieldRegexReplacement = @"$2",
                },
            };
        }

        public string Page { get; set; }

        public new void Reset()
        {
            base.Reset();
            Page = ReturnFields.MaxCount == 0
                ? string.Empty
                : string.Format("{0}", Int32.Parse(Page) + 1);
        }

        public override string ToSql()
        {
            Debug.Assert(!string.IsNullOrEmpty(InsertOrReplaceString));
            Values values = ReturnFields.ToValues();
            values.Add(Database.SchemaColumn, Enumerable.Repeat("HTTPS", values.MaxCount));
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("CREATE TABLE IF NOT EXISTS {0}(", Database.ProxyTable));
            sb.AppendLine(string.Format("{0} VARCHAR,", Database.SchemaColumn));
            sb.AppendLine(string.Format("{0} VARCHAR,", Database.AddressColumn));
            sb.AppendLine(string.Format("{0} INTEGER,", Database.PortColumn));
            sb.AppendLine(string.Format("PRIMARY KEY({0},{1},{2}));", Database.SchemaColumn, Database.AddressColumn,
                Database.PortColumn));
            sb.AppendLine("BEGIN;");

            sb.AppendLine(string.Join(Environment.NewLine,
                Transformation.ParseTemplate(InsertOrReplaceString, values.SqlEscape())));
            sb.AppendLine("COMMIT;");

            return sb.ToString();
        }
    }
}