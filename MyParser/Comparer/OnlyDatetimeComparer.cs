using System;
using System.Linq;
using System.Text.RegularExpressions;
using MyLibrary.Collections;

namespace MyParser.Comparer
{
    public class OnlyDatetimeComparer : IPublicationComparer
    {
        public const string DateTimePatten = @"\#\#(?<date>[^\#]+)\#\#";

        public int Compare(string x, string y)
        {
            var matches = new StackListQueue<Match>
            {
                Regex.Match(x, DateTimePatten),
                Regex.Match(y, DateTimePatten)
            };
            var values = new StackListQueue<DateTime>();
            foreach (Match match in matches)
                try
                {
                    values.Add(MyLibrary.Types.DateTime.Parse(match.Groups["date"].Value.Trim()));
                }
                catch (Exception exception)
                {
                    values.Add(MyLibrary.Types.DateTime.Default);
                }
            return values.First().CompareTo(values.Last());
        }

        public bool IsValid(string s)
        {
            Match match = Regex.Match(s, DateTimePatten);
            return match.Length > 0 && !string.IsNullOrEmpty(match.Groups["date"].Value.Trim());
        }

        public bool Equals(string x, string y)
        {
            return Compare(x, y) == 0;
        }

        public int GetHashCode(string obj)
        {
            Match match = Regex.Match(obj, DateTimePatten);
            DateTime dateTime;
            try
            {
                dateTime = MyLibrary.Types.DateTime.Parse(match.Groups["date"].Value.Trim());
            }
            catch (Exception exception)
            {
                dateTime = MyLibrary.Types.DateTime.Default;
            }
            return dateTime.GetHashCode();
        }
    }
}