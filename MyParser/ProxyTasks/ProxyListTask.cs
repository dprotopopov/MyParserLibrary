using System;
using System.Diagnostics;
using System.Text;
using MyParser.WebTasks;

namespace MyParser.ProxyTasks
{
    public abstract class ProxyListTask : WebTask
    {
        public ProxyListTask()
        {
            InsertOrReplaceString = string.Format(
                "INSERT OR REPLACE INTO {0}({1},{2},{3}) VALUES ('{{{{{1}}}}}','{{{{{2}}}}}',{{{{{3}}}}});",
                Database.ProxyTable, Database.SchemaColumn, Database.AddressColumn, Database.PortColumn);
        }

        protected string InsertOrReplaceString { get; set; }


        public new void Reset()
        {
            base.Reset();
        }

        public override string ToSql()
        {
            Debug.Assert(!string.IsNullOrEmpty(InsertOrReplaceString));
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("CREATE TABLE IF NOT EXISTS {0}(", Database.ProxyTable));
            sb.AppendLine(string.Format("{0} VARCHAR,", Database.SchemaColumn));
            sb.AppendLine(string.Format("{0} VARCHAR,", Database.AddressColumn));
            sb.AppendLine(string.Format("{0} INTEGER,", Database.PortColumn));
            sb.AppendLine(string.Format("PRIMARY KEY({0},{1},{2}));", Database.SchemaColumn, Database.AddressColumn,
                Database.PortColumn));
            sb.AppendLine("BEGIN;");

            sb.AppendLine(string.Join(Environment.NewLine,
                Transformation.ParseTemplate(InsertOrReplaceString, ReturnFields.ToValues().SqlEscape())));
            sb.AppendLine("COMMIT;");

            return sb.ToString();
        }
    }
}