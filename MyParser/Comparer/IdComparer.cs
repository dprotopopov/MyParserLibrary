using System.Linq;
using MyLibrary.Types;

namespace MyParser.Comparer
{
    public class IdComparer : IPublicationComparer
    {
        private static readonly OnlyDigitsDecimalComparer DecimalComparer = new OnlyDigitsDecimalComparer();
        private static readonly ObjectComparer ObjectComparer = new ObjectComparer();
        private readonly string[] _propertyNames = {"PublicationId"};

        public int Compare(string x, string y)
        {
            var resource = new Resource(x);
            var resource1 = new Resource(y);
            return DecimalComparer.Compare(resource.PublicationId.ToString(), resource1.PublicationId.ToString());
        }

        public bool IsValid(string s)
        {
            var properties = new MyLibrary.Collections.Properties(s);
            return _propertyNames.Aggregate(true, (current, p) => Boolean.And(current, properties[p] != null))
                   && DecimalComparer.IsValid(properties["PublicationId"].ToString());
        }

        public bool Equals(string x, string y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(string obj)
        {
            var resource = new Resource(obj);
            return ObjectComparer.GetHashCode(resource, _propertyNames);
        }
    }
}