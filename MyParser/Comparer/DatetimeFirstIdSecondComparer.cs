using MyLibrary.Comparer;

namespace MyParser.Comparer
{
    public class DatetimeFirstIdSecondComparer : IPublicationComparer
    {
        private static readonly OnlyDatetimeComparer DatetimeComparer = new OnlyDatetimeComparer();
        private static readonly OnlyDigitsDecimalComparer DecimalComparer = new OnlyDigitsDecimalComparer();
        private static readonly ObjectComparer ObjectComparer = new ObjectComparer();
        private readonly string[] _propertyNames = {"PublicationId", "PublicationDatetime"};

        public int Compare(string x, string y)
        {
            var resource = new Resource(x);
            var resource1 = new Resource(y);
            int value = DatetimeComparer.Compare(resource.PublicationDatetime.ToString(),
                resource1.PublicationDatetime.ToString());
            return value != 0
                ? value
                : DecimalComparer.Compare(resource.PublicationId.ToString(), resource1.PublicationId.ToString());
        }

        public bool IsValid(string s)
        {
            var resource = new Resource(s);
            return DecimalComparer.IsValid(resource.PublicationId.ToString()) &&
                   DatetimeComparer.IsValid(resource.PublicationDatetime.ToString());
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