using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using HtmlAgilityPack;
using Jint;

namespace MyParserLibrary
{
    public interface IWebSimulator : IWebTask
    {
        IWebWindow TopmostWindow { get; }
        IWebWindow Window { get; set; }
        IWebDocument WebDocument { get; }
        IWebSimulator Simulator { get; set; }
        IWebBrowser WebBrowser { get; set; }
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
        IWebElement HighlightedElement { get; set; }
        Dictionary<IWebWindow, string> Windows(IWebWindow window);
        void Focus(IWebElement webElement);
        void Click(IWebElement webElement);
        void DoubleClick(IWebElement webElement);
        void KeyDown(IWebElement webElement, VirtualKeyCode code);
        void KeyPress(IWebElement webElement, VirtualKeyCode code);
        void KeyUp(IWebElement webElement, VirtualKeyCode code);
        void TextEntry(IWebElement webElement, string text);
        IWebElement Select(IWebElement webElement);
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
        string XPathToMask(string mask);
        string XPathSanitize(string xpath);

        /// <summary>
        ///     Get HtmlElement by HtmlNode
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        List<IWebElement> GetElementByNode(List<HtmlNode> nodes);

        List<HtmlNode> GetNodeByElement(List<IWebElement> elements);
        void HighlightElement(IWebElement webElement, bool highlight, bool scrollToElement);
        object SimulateTextEntry(IWebElement webElement, List<object> parameters);
        object SimulateEvent(EventInfo eventInfo, IWebElement webElement, List<object> parameters);
        void ScrollToElement(IWebElement webElement);
        object RunScript();
    }
}