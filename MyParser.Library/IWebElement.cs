using System.Drawing;
using WindowsInput.Native;

namespace MyParser.Library
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
        void ScrollIntoView(bool b);
        bool Equals(IWebElement obj);
        string ToString();
        bool IsNullOrEmpty();
        Rectangle Rectangle { get; }

        #region 

        void Focus();
        void Click();
        void DoubleClick();
        void KeyDown(VirtualKeyCode code);
        void KeyPress(VirtualKeyCode code);
        void KeyUp(VirtualKeyCode code);
        void TextEntry(string text);

        #endregion
    }
}