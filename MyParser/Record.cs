using System.Linq;
using System.Reflection;
using MyLibrary.Attribute;

namespace MyParser
{
    public class Record : MyDatabase.Record, IValueable
    {
        public Record(MyLibrary.Collections.Properties properties)
            : base(properties)
        {
        }

        public Record()
        {
        }

        public new Values ToValues()
        {
            return new Values(this);
        }
    }
}