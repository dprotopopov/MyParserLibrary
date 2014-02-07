using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using HtmlAgilityPack;
using Jint;
using MyParser.Library;
using MyParser.Managed;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MyWebSimulator
{
    public sealed class WebSimulator : WebTask, IWebSimulator
    {
        public WebSimulator()
        {
            Simulator = this;
            InputSimulator = new InputSimulator();
            DocumentCompleted = 0;
            var sb = new StringBuilder();
            sb.AppendLine(@"while(Simulator.get_DocumentCompleted()==0) Simulator.Sleep(1000);");
            sb.AppendLine(@"return 0;");
            JavaScript = sb.ToString();
            JintEngine = new JintEngine();
            JintEngine.SetFunction("alert", new Action<object>(s => Debug.WriteLine(s)));
        }

        public IWebWindow TopmostWindow
        {
            get { return WebBrowser.Document.Window; }
        }

        public IWebWindow Window { get; set; }

        public Dictionary<IWebWindow, string> Windows(IWebWindow window)
        {
            Debug.Assert(!window.IsNullOrEmpty());

            var dictionary = new Dictionary<IWebWindow, string>();
            var stack = new Stack<KeyValuePair<IWebWindow, string>>();
            stack.Push(new KeyValuePair<IWebWindow, string>(window, ""));
            while (stack.Any())
            {
                try
                {
                    KeyValuePair<IWebWindow, string> item = stack.Pop();
                    string xpath = item.Value;
                    if (!item.Key.WindowFrameElement.IsNullOrEmpty())
                    {
                        xpath += item.Key.WindowFrameElement.XPath;
                    }
                    dictionary.Add(item.Key, xpath);
                    if (item.Key.Frames != null)
                        foreach (IWebWindow child in item.Key.Frames.Reverse())
                        {
                            stack.Push(new KeyValuePair<IWebWindow, string>(child, xpath));
                        }
                }
                catch (Exception exception)
                {
                    LastError = exception;
                }
            }
            return dictionary;
        }

        public IWebDocument WebDocument
        {
            get { return Window.Document; }
        }

        public IWebSimulator Simulator { get; set; }
        public IWebBrowser WebBrowser { get; set; }

        public InputSimulator InputSimulator { get; set; }

        public IMouseSimulator MouseSimulator
        {
            get { return InputSimulator.Mouse; }
        }

        public IKeyboardSimulator KeyboardSimulator
        {
            get { return InputSimulator.Keyboard; }
        }

        public HtmlDocument HtmlDocument
        {
            get
            {
                var document = new HtmlDocument();
                if (WebDocument.Body != null && !WebDocument.Body.IsNullOrEmpty())
                    if (WebDocument.Body.Parent != null && !WebDocument.Body.Parent.IsNullOrEmpty())
                        document.LoadHtml(WebDocument.Body.Parent.OuterHtml);
                return document;
            }
        }

        public JintEngine JintEngine { get; set; }
        public string JavaScript { get; set; }

        public Dictionary<EventInfo, string> ElementEvents
        {
            get
            {
                Type type = typeof (HtmlElement);
                return type.GetEvents().ToDictionary(item => item, item => item.Name);
            }
        }

        public Dictionary<MethodInfo, string> MouseMethods
        {
            get
            {
                Type type = typeof (MouseSimulator);
                return type.GetMethods().ToDictionary(item => item, item => item.Name);
            }
        }

        public Dictionary<MethodInfo, string> KeyboardMethods
        {
            get
            {
                Type type = typeof (KeyboardSimulator);
                return type.GetMethods().ToDictionary(item => item, item => item.Name);
            }
        }

        public Dictionary<MethodInfo, string> SimulatorMethodInfos
        {
            get
            {
                Type type = GetType();
                return type.GetMethods().ToDictionary(item => item, item => item.Name);
            }
        }

        public int DocumentCompleted { get; set; }
        public IWebElement HighlightedElement { get; set; }

        /// <summary>
        ///     Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        ///     Строка, представляющая текущий объект.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return WebBrowser.Url.ToString();
        }

        public void Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        public override void ThreadProc(object obj)
        {
            var This = obj as IWebSimulator;
            Debug.Assert(This != null, "This != null");
            try
            {
                ParametersValues parametersValues = MyLibrary.BuildParametersValues(This.Url, This);
                string url = MyLibrary.ParseRowTemplate(Url, parametersValues);
                This.WebBrowser.Navigate(url);
                This.RunScript();
                HtmlDocument[] docs = {This.HtmlDocument};
                var returnFields = new ReturnFields();
                HtmlNode[] nodes = docs.Select(doc => doc.DocumentNode).ToArray();

                returnFields.InsertOrAppend(MyLibrary.BuildReturnFields(nodes, parametersValues,
                    This.ReturnFieldInfos));
                This.ReturnFields = returnFields;
                This.Status = WebTaskStatus.Finished;
                This.Thread = null;
                if (This.OnCompliteCallback != null) This.OnCompliteCallback(This);
            }
            catch (Exception)
            {
                This.Status = WebTaskStatus.Error;
                This.Thread = null;
                if (This.OnErrorCallback != null) This.OnErrorCallback(This);
            }
        }

        public bool AttachForegroundWindowThreadInput(bool fAttach)
        {
            Form form = WebBrowser.FindForm();
            IntPtr foregroundWindowHandle = GetForegroundWindow();
            if (form != null)
            {
                uint lpdwProcessId;
                uint threadId = GetWindowThreadProcessId(form.Handle, out lpdwProcessId);
                uint foregroundThreadId = GetWindowThreadProcessId(foregroundWindowHandle, out lpdwProcessId);
                bool result = AttachThreadInput(foregroundThreadId, threadId, fAttach);
                Debug.WriteLine("AttachForegroundWindowThreadInput foregroundThreadId = " + foregroundThreadId +
                                " threadId = " + threadId + " fAttach = " + fAttach + " result = " + result);
                return result;
            }
            return false;
        }

        public void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Window = TopmostWindow;
            DocumentCompleted++;
        }

        public void SetWebBrowserFormFocus()
        {
            Form form = WebBrowser.FindForm();
            if (form != null && Application.OpenForms[form.Name] == null)
            {
                form.Show();
            }
            else
            {
                if (form != null) Application.OpenForms[form.Name].Activate();
            }
        }

        public bool SetWebBrowserControlFocus()
        {
            bool result = WebBrowser.Focus();
            Debug.WriteLine("SetWebBrowserControlFocus WebBrowser.Handle = " + WebBrowser.Handle + " result = " + result);
            return result;
        }

        public bool SetForegroundCurrentProcessMainWindow()
        {
            Process process = Process.GetCurrentProcess();
            bool result = SetForegroundWindow(process.MainWindowHandle);
            Debug.WriteLine("SetForegroundCurrentProcessMainWindow process.MainWindowHandle = " +
                            process.MainWindowHandle + " result = " + result);
            return result;
        }

        /// <summary>
        ///     Get ManagedWebElement by HtmlNode
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<IWebElement> GetElementByNode(List<HtmlNode> nodes)
        {
            var elements = new List<IWebElement>();

            var dictionaryNode = new Dictionary<HtmlNode, string>();
            var dictionaryElement = new Dictionary<IWebElement, string>();
            foreach (IWebElement item in WebDocument.All)
            {
                string xpath = MyLibrary.XPathSanitize(item.XPath.ToLower());
                dictionaryElement.Add(item, xpath);
            }
            foreach (HtmlNode item in nodes)
            {
                string xpath = new Regex(@"\B\/html\[\d+\]").Replace(
                    MyLibrary.XPathSanitize(item.XPath.ToLower()), @"/");
                dictionaryNode.Add(item, xpath);
            }

            foreach (var item in dictionaryNode.Select(node => MyLibrary.XPathToMask(node.Value))
                .SelectMany(mask => (from item in dictionaryElement
                    let matches = Regex.Matches(item.Value, mask)
                    where matches.Count == 1 && !elements.Contains(item.Key)
                    select item)))
            {
                Debug.WriteLine(item.Value);
                elements.Add(item.Key);
            }
            return elements;
        }

        public List<HtmlNode> GetNodeByElement(List<IWebElement> elements)
        {
            var nodes = new List<HtmlNode>();

            var dictionaryNode = new Dictionary<HtmlNode, string>();
            var dictionaryElement = new Dictionary<IWebElement, string>();
            foreach (IWebElement item in elements)
            {
                string xpath = MyLibrary.XPathSanitize(item.XPath.ToLower());
                dictionaryElement.Add(item, xpath);
            }
            foreach (HtmlNode item in HtmlDocument.DocumentNode.SelectNodes(@"//*").ToList())
            {
                string xpath = new Regex(@"\B\/html\[\d+\]").Replace(
                    MyLibrary.XPathSanitize(item.XPath.ToLower()), @"/");
                dictionaryNode.Add(item, xpath);
            }

            foreach (var item in dictionaryElement.Select(element => MyLibrary.XPathToMask(element.Value))
                .SelectMany(mask => (from item in dictionaryNode
                    let matches = Regex.Matches(item.Value, mask)
                    where matches.Count == 1 && !nodes.Contains(item.Key)
                    select item)))
            {
                Debug.WriteLine(item.Value);
                nodes.Add(item.Key);
            }
            return nodes;
        }

        public void HighlightElement(IWebElement webElement, bool highlight, bool scrollToElement)
        {
            if (scrollToElement) ScrollToElement(webElement);

            var styles = new Dictionary<string, string>
            {
                {@"border:red 4px solid;", @"border(-(left|right|top|bottom))?\s*:(\s*(red|4px|solid)){1,3}(\s*;)?"},
                {@"border-color:red;", @"border(-(left|right|top|bottom))?-color\s*:\s*red(\s*;)?"},
                {@"border-style:solid;", @"border(-(left|right|top|bottom))?-style\s*:\s*solid(\s*;)?"},
                {@"border-width:4px;", @"border(-(left|right|top|bottom))?-width\s*:\s*4px(\s*;)?"},
            };
            if (webElement != null)
            {
                if (highlight)
                {
                    try
                    {
                        webElement.Style += styles.Keys.Aggregate((i, j) => i + j);
                    }
                    catch (Exception)
                    {
                        webElement.Style = styles.Keys.Aggregate((i, j) => i + j);
                    }
                }
                else
                {
                    try
                    {
                        foreach (
                            Regex regex in styles.Values.Select(pattern => new Regex(pattern, RegexOptions.IgnoreCase)))
                        {
                            webElement.Style = regex.Replace(webElement.Style, @"");
                        }
                    }
                    catch (Exception)
                    {
                        webElement.Style = @"";
                    }
                }
            }
        }

        public object SimulateTextEntry(IWebElement webElement, List<object> parameters)
        {
            Type[] types = {typeof (ManagedWebElement), typeof (string)};
            MethodInfo methodInfo = GetType().GetMethod("TextEntry", types);
            var objects = new List<object> {webElement};
            objects.AddRange(parameters);
            return methodInfo.Invoke(this, objects.ToArray());
        }

        public object SimulateEvent(EventInfo eventInfo, IWebElement webElement, List<object> parameters)
        {
            Type[] typesIWebElement = {typeof (IWebElement)};
            Type[] typesIWebElementVirtualKeyCode = {typeof (IWebElement), typeof (VirtualKeyCode)};
            var dictionary = new Dictionary<EventInfo, MethodInfo>
            {
                {typeof (HtmlElement).GetEvent("GotFocus"), GetType().GetMethod("Focus", typesIWebElement)},
                {typeof (HtmlElement).GetEvent("Click"), GetType().GetMethod("Click", typesIWebElement)},
                {
                    typeof (HtmlElement).GetEvent("DoubleClick"),
                    GetType().GetMethod("DoubleClick", typesIWebElement)
                },
                {
                    typeof (HtmlElement).GetEvent("KeyDown"),
                    GetType().GetMethod("KeyDown", typesIWebElementVirtualKeyCode)
                },
                {
                    typeof (HtmlElement).GetEvent("KeyPress"),
                    GetType().GetMethod("KeyPress", typesIWebElementVirtualKeyCode)
                },
                {
                    typeof (HtmlElement).GetEvent("KeyUp"),
                    GetType().GetMethod("KeyUp", typesIWebElementVirtualKeyCode)
                },
            };
            if (dictionary.ContainsKey(eventInfo))
            {
                MethodInfo methodInfo = dictionary[eventInfo];
                var objects = new List<object> {webElement};
                objects.AddRange(parameters);
                return methodInfo.Invoke(this, objects.ToArray());
            }
            throw new NotImplementedException();
        }

        public void ScrollToElement(IWebElement webElement)
        {
            try
            {
                webElement.ScrollIntoView(true);
            }
            catch (Exception exception)
            {
                LastError = exception;
            }
        }

        public object RunScript()
        {
            Type type = GetType();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                // Setting the externals parameters of the context
                JintEngine.SetParameter(prop.Name, prop.GetValue(this));
            }

            // Running the script
            return JintEngine.Run(JavaScript);
        }

        #region

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        #endregion

        #region

        public void Focus(IWebElement webElement)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = webElement, true, true);
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = webElement.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {};
                methodInfo.Invoke(webElement, parameters);
            }
            catch (NotImplementedException)
            {
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                webElement.Focus();
            }
        }

        public void Click(IWebElement webElement)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = webElement, true, true);
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = webElement.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {};
                methodInfo.Invoke(webElement, parameters);
            }
            catch (NotImplementedException)
            {
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                webElement.Focus();
                InputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
            }
        }

        public void DoubleClick(IWebElement webElement)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = webElement, true, true);
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = webElement.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {};
                methodInfo.Invoke(webElement, parameters);
            }
            catch (NotImplementedException)
            {
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                webElement.Focus();
                InputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
                InputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
            }
        }

        public void KeyDown(IWebElement webElement, VirtualKeyCode code)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = webElement, true, true);
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = webElement.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {code};
                methodInfo.Invoke(webElement, parameters);
            }
            catch (NotImplementedException)
            {
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                webElement.Focus();
                InputSimulator.Keyboard.KeyDown(code);
            }
        }

        public void KeyPress(IWebElement webElement, VirtualKeyCode code)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = webElement, true, true);
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = webElement.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {code};
                methodInfo.Invoke(webElement, parameters);
            }
            catch (NotImplementedException)
            {
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                webElement.Focus();
                InputSimulator.Keyboard.KeyPress(code);
            }
        }

        public void KeyUp(IWebElement webElement, VirtualKeyCode code)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = webElement, true, true);
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = webElement.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {code};
                methodInfo.Invoke(webElement, parameters);
            }
            catch (NotImplementedException)
            {
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                webElement.Focus();
                InputSimulator.Keyboard.KeyUp(code);
            }
        }

        public void TextEntry(IWebElement webElement, string text)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = webElement, true, true);
            try
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = webElement.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {text};
                methodInfo.Invoke(webElement, parameters);
            }
            catch (NotImplementedException)
            {
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                webElement.Focus();
                InputSimulator.Keyboard.TextEntry(text);
            }
        }

        public IWebElement Select(IWebElement webElement)
        {
            return webElement;
        }

        #endregion

        #region

        public object[] Focus(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] Click(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] DoubleClick(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] KeyDown(string xpath, VirtualKeyCode code)
        {
            var parameters = new List<object> {code};
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] KeyPress(string xpath, VirtualKeyCode code)
        {
            var parameters = new List<object> {code};
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] KeyUp(string xpath, VirtualKeyCode code)
        {
            var parameters = new List<object> {code};
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] TextEntry(string xpath, string text)
        {
            var parameters = new List<object> {text};
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] Select(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> {typeof (ManagedWebElement)};
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            Debug.Assert(methodInfo != null);
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        #endregion

        private List<object> InvokeMethod(MethodInfo methodInfo, string xpath, List<object> arguments)
        {
            var results = new List<object>();
            List<HtmlNode> nodes = HtmlDocument.DocumentNode.SelectNodes(xpath).ToList();
            List<IWebElement> elements = GetElementByNode(nodes);
            foreach (var objects in elements.Select(element => new List<object> {element}))
            {
                objects.AddRange(arguments);
                results.Add(methodInfo.Invoke(this, objects.ToArray()));
            }
            return results;
        }
    }
}