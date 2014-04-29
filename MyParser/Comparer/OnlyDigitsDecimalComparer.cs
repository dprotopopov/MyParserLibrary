using System;
using System.Diagnostics;
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
                parse = MyLibrary.Types.Decimal.Parse(_regex.Replace(x, string.Empty).Trim());
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                parse = MyLibrary.Types.Decimal.Default;
            }
            try
            {
                value = MyLibrary.Types.Decimal.Parse(_regex.Replace(y, string.Empty).Trim());
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                value = MyLibrary.Types.Decimal.Default;
            }

            return parse.CompareTo(value);
        }

        public bool IsValid(string s)
        {
            return !string.IsNullOrWhiteSpace(s);
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
                parse = MyLibrary.Types.Decimal.Parse(_regex.Replace(obj, string.Empty).Trim());
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
                parse = MyLibrary.Types.Decimal.Default;
            }
            return parse.GetHashCode();
        }
    }
}