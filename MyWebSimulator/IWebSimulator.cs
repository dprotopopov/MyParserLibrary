using System.Collections.Generic;
using System.Reflection;
using WindowsInput;
using WindowsInput.Native;
using HtmlAgilityPack;
using Jint;
using MyParser.WebTasks;

namespace MyWebSimulator
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
        HtmlDocument HtmlDocument { get; }
        JintEngine JintEngine { get; set; }
        string JavaScript { get; set; }
        Dictionary<EventInfo, string> ElementEvents { get; }
        Dictionary<MethodInfo, string> MouseMethods { get; }
        Dictionary<MethodInfo, string> KeyboardMethods { get; }
        Dictionary<MethodInfo, string> SimulatorMethodInfos { get; }
        int DocumentCompleted { get; set; }
        IWebElement HighlightedElement { get; set; }
        Dictionary<IWebWindow, string> Windows(IWebWindow window);

        void Sleep(int millisecondsTimeout);
        bool AttachForegroundWindowThreadInput(bool fAttach);
        void DocumentLoadCompleted(params object[] parameters);
        void SetWebBrowserFormFocus();
        bool SetWebBrowserControlFocus();
        bool SetForegroundCurrentProcessMainWindow();

        /// <summary>
        ///     Get HtmlElement by HtmlNode
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        IEnumerable<IWebElement> GetElementByNode(IEnumerable<HtmlNode> nodes);

        HtmlNode[] GetNodeByElement(IEnumerable<IWebElement> elements);
        void HighlightElement(IWebElement webElement, bool highlight, bool scrollToElement);
        object SimulateTextEntry(IWebElement webElement, IEnumerable<object> parameters);
        object SimulateEvent(EventInfo eventInfo, IWebElement webElement, IEnumerable<object> parameters);
        void ScrollToElement(IWebElement webElement);
        object RunScript();

        #region

        void Focus(IWebElement webElement);
        void Click(IWebElement webElement);
        void DoubleClick(IWebElement webElement);
        void KeyDown(IWebElement webElement, VirtualKeyCode code);
        void KeyPress(IWebElement webElement, VirtualKeyCode code);
        void KeyUp(IWebElement webElement, VirtualKeyCode code);
        void TextEntry(IWebElement webElement, string text);
        IWebElement Select(IWebElement webElement);

        #endregion

        #region

        object[] Focus(params object[] arguments);
        object[] Click(params object[] arguments);
        object[] DoubleClick(params object[] arguments);
        object[] KeyDown(params object[] arguments);
        object[] KeyPress(params object[] arguments);
        object[] KeyUp(params object[] arguments);
        object[] TextEntry(params object[] arguments);
        object[] Select(params object[] arguments);

        #endregion
    }
}