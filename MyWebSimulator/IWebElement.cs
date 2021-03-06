﻿using System.Drawing;
using WindowsInput.Native;

namespace MyWebSimulator
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

        void ScrollIntoView(bool b);
        bool Equals(IWebElement obj);
        string ToString();
        bool IsNullOrEmpty();
    }
}