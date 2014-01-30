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
        WebWindow TopmostWindow { get; }
        WebWindow Window { get; set; }
        WebDocument WebDocument { get; }
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
        WebElement HighlightedElement { get; set; }
        Dictionary<WebWindow, string> Windows(WebWindow window);
        void Focus(WebElement webElement);
        void Click(WebElement webElement);
        void DoubleClick(WebElement webElement);
        void KeyDown(WebElement webElement, VirtualKeyCode code);
        void KeyPress(WebElement webElement, VirtualKeyCode code);
        void KeyUp(WebElement webElement, VirtualKeyCode code);
        void TextEntry(WebElement webElement, string text);
        WebElement Select(WebElement webElement);
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
        List<WebElement> GetElementByNode(List<HtmlNode> nodes);

        List<HtmlNode> GetNodeByElement(List<WebElement> elements);
        void HighlightElement(WebElement webElement, bool highlight, bool scrollToElement);
        object SimulateTextEntry(WebElement webElement, List<object> parameters);
        object SimulateEvent(EventInfo eventInfo, WebElement webElement, List<object> parameters);
        void ScrollToElement(WebElement webElement);
        object RunScript();
    }
}