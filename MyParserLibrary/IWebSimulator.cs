using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using HtmlAgilityPack;
using Jint;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace MyParserLibrary
{
    public interface IWebSimulator : IWebTask
    {
        HtmlDocument Document { get; }
        WebSimulator Simulator { get; set; }
        WebBrowser WebBrowser { get; set; }
        Uri Uri { get; set; }
        InputSimulator InputSimulator { get; set; }
        IMouseSimulator MouseSimulator { get; }
        IKeyboardSimulator KeyboardSimulator { get; }
        HtmlAgilityPack.HtmlDocument HtmlDocument { get; }
        JintEngine JintEngine { get; set; }
        string JavaScript { get; set; }
        Dictionary<EventInfo, string> ElementEvents { get; }
        Dictionary<MethodInfo, string> MouseMethods { get; }
        Dictionary<MethodInfo, string> KeyboardMethods { get; }
        Dictionary<MethodInfo, string> SimulatorMethodInfos { get; }
        int DocumentCompleted { get; set; }
        HtmlElement HighlightedElement { get; set; }
        void Focus(HtmlElement htmlElement);
        void Click(HtmlElement htmlElement);
        void DoubleClick(HtmlElement htmlElement);
        void KeyDown(HtmlElement htmlElement, VirtualKeyCode code);
        void KeyPress(HtmlElement htmlElement, VirtualKeyCode code);
        void KeyUp(HtmlElement htmlElement, VirtualKeyCode code);
        void TextEntry(HtmlElement htmlElement, string text);
        HtmlElement Select(HtmlElement htmlElement);
        object[] Focus(string xpath);
        object[] Click(string xpath);
        object[] DoubleClick(string xpath);
        object[] KeyDown(string xpath, VirtualKeyCode code);
        object[] KeyPress(string xpath, VirtualKeyCode code);
        object[] KeyUp(string xpath, VirtualKeyCode code);
        object[] TextEntry(string xpath, string text);
        object[] Select(string xpath);

        void Sleep(int millisecondsTimeout);
        bool AttachForegroundWindowThreadInput(bool fAttach);
        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e);
        void SetWebBrowserFormFocus();
        bool SetWebBrowserControlFocus();
        bool SetForegroundCurrentProcessMainWindow();
        string GetXPath(HtmlElement item);
        string XPathToMask(string mask);
        string XPathSanitize(string xpath);

        /// <summary>
        ///     Get HtmlElement by HtmlNode
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        List<HtmlElement> GetElementByNode(List<HtmlNode> nodes);

        List<HtmlNode> GetNodeByElement(List<HtmlElement> elements);
        void HighlightElement(HtmlElement element, bool highlight, bool scrollToElement);
        object SimulateTextEntry(HtmlElement htmlElement, List<object> parameters);
        object SimulateEvent(EventInfo eventInfo, HtmlElement htmlElement, List<object> parameters);
        void ScrollToElement(HtmlElement htmlElement);
        object RunScript();
    }
}