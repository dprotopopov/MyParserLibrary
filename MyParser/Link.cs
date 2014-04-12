namespace MyParser
{
    public class Link : IValueable
    {
        public Link(string s)
        {
            Url = s;
        }

        private string Url { get; set; }

        public Values ToValues()
        {
            return new Values(this);
        }

        public override string ToString()
        {
            return Url;
        }
    }
}