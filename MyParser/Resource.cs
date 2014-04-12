namespace MyParser
{
    public class Resource : RequestProperties, IValueable
    {
        public Resource(string s)
            : base(s)
        {
            Text = s;
        }

        private string Text { get; set; }

        public new Values ToValues()
        {
            return new Values(this);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}