using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyLibrary.Attribute;

namespace MyParser
{
    public class LinkComparer : IEqualityComparer<Link>, IComparer<Link>, IValueable
    {
        public int Compare(Link x, Link y)
        {
            return string.CompareOrdinal(x.ToString().ToLower(), y.ToString().ToLower());
        }

        public bool Equals(Link x, Link y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(Link obj)
        {
            return obj.ToString().ToLower().GetHashCode();
        }

        public Values ToValues()
        {
            return new Values(this);
        }
    }
}