namespace MyParser.Comparer
{
    public class NoComparer : IPublicationComparer
    {
        public int Compare(string x, string y)
        {
            return 0;
        }

        public bool IsValid(string s)
        {
            return true;
        }

        public bool Equals(string x, string y)
        {
            return true;
        }

        public int GetHashCode(string obj)
        {
            return 0;
        }
    }
}