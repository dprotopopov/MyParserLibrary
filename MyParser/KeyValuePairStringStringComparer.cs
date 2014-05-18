using System.Collections.Generic;

namespace MyParser
{
    public class KeyValuePairStringStringComparer : IComparer<KeyValuePair<string, string>>, IEqualityComparer<KeyValuePair<string, string>>
    {
        public int Compare(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            int value = string.CompareOrdinal(x.Key, y.Key);
            return value != 0 ? value : string.CompareOrdinal(x.Value, y.Value);
        }

        public bool Equals(KeyValuePair<string, string> x, KeyValuePair<string, string> y)
        {
            return Compare(x,y) == 0;
        }

        public int GetHashCode(KeyValuePair<string, string> obj)
        {
            return obj.Key.GetHashCode() ^ obj.Value.GetHashCode();
        }
    }
}