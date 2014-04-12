namespace MyParser
{
    public class Record : MyLibrary.Collections.Properties, IValueable
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