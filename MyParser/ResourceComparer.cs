using System.Collections.Generic;
using MyParser.Comparer;

namespace MyParser
{
    public class ResourceComparer : IEqualityComparer<Resource>, IComparer<Resource>
    {
        private static readonly ObjectComparer ObjectComparer = new ObjectComparer();
        public IPublicationComparer PublicationComparer { get; set; }
        public IEnumerable<string> Mapping { get; set; }

        public int Compare(Resource x, Resource y)
        {
            int value = ObjectComparer.Compare(x, y, Mapping);
            return value != 0 ? value : PublicationComparer.Compare(x.ToString(), y.ToString());
        }

        public bool Equals(Resource x, Resource y)
        {
            return PublicationComparer.Equals(x.ToString(), y.ToString()) && ObjectComparer.Equals(x, y, Mapping);
        }

        public int GetHashCode(Resource obj)
        {
            return PublicationComparer.GetHashCode(obj.ToString()) ^ ObjectComparer.GetHashCode(obj, Mapping);
        }
    }
}