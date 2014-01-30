using System.Drawing;

namespace MyParserLibrary
{
    public interface IWebElement
    {
        IWebElement Parent { get; }
        string TagName { get; }
        string OuterHtml { get; }
        string Style { get; set; }
        IWebElement[] Children { get; }
        Rectangle OffsetRectangle { get; }
        IWebElement OffsetParent { get; }
        string XPath { get; }
        void Focus();
        void ScrollIntoView(bool b);
        bool Equals(IWebElement obj);
        string ToString();
        bool IsNullOrEmpty();
        Rectangle Rectangle { get; }
    }
}