using System.Collections;

namespace MyParser.Comparer
{
    public class AscComparer : IPublicationComparer
    {
        public int Compare(string x, string y)
        {
            return ((new CaseInsensitiveComparer()).Compare(x, y));
        }

        public bool IsValid(string s)
        {
            return true;
        }

        public bool Equals(string x, string y)
        {
            return x.ToLower().Equals(y.ToLower());
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}