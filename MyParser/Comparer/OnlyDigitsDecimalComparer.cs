using System;
using System.Text.RegularExpressions;

namespace MyParser.Comparer
{
    internal class OnlyDigitsDecimalComparer : IPublicationComparer
    {
        private const string NonDigitPattern = @"\D+";

        private readonly Regex _regex =
            new Regex(NonDigitPattern);

        public int Compare(string x, string y)
        {
            Decimal parse;
            Decimal value;
            try
            {
                parse = MyLibrary.Types.Decimal.Parse(_regex.Replace(x, @"").Trim());
            }
            catch (Exception exception)
            {
                parse = MyLibrary.Types.Decimal.Default;
            }
            try
            {
                value = MyLibrary.Types.Decimal.Parse(_regex.Replace(y, @"").Trim());
            }
            catch (Exception exception)
            {
                value = MyLibrary.Types.Decimal.Default;
            }

            return parse.CompareTo(value);
        }

        public bool IsValid(string s)
        {
            return true;
        }

        public bool Equals(string x, string y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(string obj)
        {
            Decimal parse;
            try
            {
                parse = MyLibrary.Types.Decimal.Parse(_regex.Replace(obj, @"").Trim());
            }
            catch (Exception exception)
            {
                parse = MyLibrary.Types.Decimal.Default;
            }
            return parse.GetHashCode();
        }
    }
}