using System.Collections.Generic;

namespace MyParser.Comparer
{
    public interface IPublicationComparer : IEqualityComparer<string>, IComparer<string>
    {
        bool IsValid(string s);
    }
}