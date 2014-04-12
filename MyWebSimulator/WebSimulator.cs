using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using MyLibrary;
using MyLibrary.Collections;
using MyParser.WebTasks;
using MyWebSimulator.Managed;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using String = MyLibrary.Types.String;
using Values = MyParser.Values;

namespace MyWebSimulator
{
    public class WebSimulator : WebTask, IWebSimulator
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
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                IWebWindow value = WebBrowser.Document.Window;
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebWindow Window { get; set; }

        public Dictionary<IWebWindow, string> Windows(IWebWindow window)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(window != null && !window.IsNullOrEmpty());

            var dictionary = new Dictionary<IWebWindow, string>();
            var queue = new Queue<KeyValuePair<IWebWindow, string>>();
            queue.Enqueue(new KeyValuePair<IWebWindow, string>(window, ""));
            do
            {
                KeyValuePair<IWebWindow, string> item = queue.Dequeue();
                window = item.Key;
                string xpath = item.Value;
                Debug.Assert(window != null && !window.IsNullOrEmpty());
                dictionary.Add(window, xpath);
                IWebWindow[] frames = window.Frames;
                foreach (IWebWindow frame in frames)
                {
                    queue.Enqueue(new KeyValuePair<IWebWindow, string>(frame, xpath + frame.WindowFrameElement.XPath));
                }
            } while (queue.Any());
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return dictionary;
        }

        public IWebDocument WebDocument
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                IWebDocument value = Window.Document;
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
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
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var document = new HtmlDocument();
                IWebElement rootElement = WebDocument.RootElement;
                if (rootElement != null && !rootElement.IsNullOrEmpty())
                    document.LoadHtml(rootElement.OuterHtml);
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
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
                string url = This.Transformation.ParseRowTemplate(This.Url, This.ToValues());
                var values = new Values
                {
                    Url = new StackListQueue<string> {url}
                };
                This.WebBrowser.Navigate(url);
                This.RunScript();
                IEnumerable<HtmlDocument> docs = new StackListQueue<HtmlDocument>(This.HtmlDocument);
                This.ReturnFields = This.Parser.BuildReturnFields(docs, values,
                    This.ReturnFieldInfos);
                This.Status = WebTaskStatus.Finished;
                This.Thread = null;
                if (This.OnCompliteCallback != null) This.OnCompliteCallback(This);
                Thread.Sleep(0);
            }
            catch (Exception)
            {
                This.Status = WebTaskStatus.Error;
                This.Thread = null;
                if (This.OnErrorCallback != null) This.OnErrorCallback(This);
                Thread.Sleep(0);
            }
        }

        public bool AttachForegroundWindowThreadInput(bool fAttach)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
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
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return result;
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return false;
        }

        public void DocumentLoadCompleted(params object[] parameters)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Window = TopmostWindow;
            DocumentCompleted++;
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void SetWebBrowserFormFocus()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Form form = WebBrowser.FindForm();
            if (form != null && Application.OpenForms[form.Name] == null)
            {
                form.Show();
            }
            else
            {
                if (form != null) Application.OpenForms[form.Name].Activate();
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public bool SetWebBrowserControlFocus()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            bool result = WebBrowser.Focus();
            Debug.WriteLine("SetWebBrowserControlFocus WebBrowser.Handle = " + WebBrowser.Handle + " result = " + result);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return result;
        }

        public bool SetForegroundCurrentProcessMainWindow()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Process process = Process.GetCurrentProcess();
            bool result = SetForegroundWindow(process.MainWindowHandle);
            Debug.WriteLine("SetForegroundCurrentProcessMainWindow process.MainWindowHandle = " +
                            process.MainWindowHandle + " result = " + result);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return result;
        }

        /// <summary>
        ///     Get ManagedWebElement by HtmlNode
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public IEnumerable<IWebElement> GetElementByNode(IEnumerable<HtmlNode> nodes)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var elements = new List<IWebElement>();

            var dictionaryElement = new Dictionary<IWebElement, string>();
            foreach (IWebElement item in WebDocument.All)
            {
                string xpath = XPath.Sanitize(item.XPath.ToLower());
                dictionaryElement.Add(item, xpath);
            }
            var dictionaryNode = new Dictionary<HtmlNode, string>();
            foreach (HtmlNode item in nodes)
            {
                string xpath = XPath.Sanitize(item.XPath.ToLower());
                dictionaryNode.Add(item, xpath);
            }

            foreach (var item in dictionaryNode.Select(node => XPath.ToRegexPattern(node.Value))
                .SelectMany(mask => (from item in dictionaryElement
                    let matches = Regex.Matches(item.Value, mask)
                    where matches.Count == 1 && !elements.Contains(item.Key)
                    select item)))
            {
                Debug.WriteLine(item.Value);
                elements.Add(item.Key);
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return elements.ToArray();
        }

        public HtmlNode[] GetNodeByElement(IEnumerable<IWebElement> elements)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var nodes = new List<HtmlNode>();

            var dictionaryElement = new Dictionary<IWebElement, string>();
            foreach (IWebElement item in elements)
            {
                string xpath = XPath.Sanitize(item.XPath.ToLower());
                dictionaryElement.Add(item, xpath);
            }
            var dictionaryNode = new Dictionary<HtmlNode, string>();
            foreach (HtmlNode item in HtmlDocument.DocumentNode.SelectNodes(@"//*"))
            {
                string xpath = XPath.Sanitize(item.XPath.ToLower());
                dictionaryNode.Add(item, xpath);
            }

            foreach (var item in dictionaryElement.Select(element => XPath.ToRegexPattern(element.Value))
                .SelectMany(mask => (from item in dictionaryNode
                    let matches = Regex.Matches(item.Value, mask)
                    where matches.Count == 1 && !nodes.Contains(item.Key)
                    select item)))
            {
                Debug.WriteLine(item.Value);
                nodes.Add(item.Key);
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return nodes.ToArray();
        }

        public void HighlightElement(IWebElement webElement, bool highlight, bool scrollToElement)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            if (scrollToElement) ScrollToElement(webElement);

            var styles = new Dictionary<string, string>
            {
                {@"border:red 4px solid;", @"border(-(left|right|top|bottom))?\s*:(\s*(red|4px|solid)){1,3}(\s*;)?"},
                {@"border-color:red;", @"border(-(left|right|top|bottom))?-color\s*:\s*red(\s*;)?"},
                {@"border-style:solid;", @"border(-(left|right|top|bottom))?-style\s*:\s*solid(\s*;)?"},
                {@"border-width:4px;", @"border(-(left|right|top|bottom))?-width\s*:\s*4px(\s*;)?"},
            };
            if (webElement != null && !webElement.IsNullOrEmpty())
            {
                Debug.Assert(!(webElement is IWebDocument));
                Debug.Assert(!(webElement is IWebWindow));
                Debug.WriteLine("webElement = " + webElement.ToString());
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
            else
            {
                Debug.WriteLine("webElement IsNullOrEmpty");
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public object SimulateTextEntry(IWebElement webElement, IEnumerable<object> parameters)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
            Type[] types = {typeof (ManagedWebElement), typeof (string)};
            MethodInfo methodInfo = GetType().GetMethod("TextEntry", types);
            var objects = new List<object> {webElement};
            objects.AddRange(parameters);
            object value = methodInfo.Invoke(this, objects.ToArray());
            Debug.WriteLine("value = " + String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object SimulateEvent(EventInfo eventInfo, IWebElement webElement, IEnumerable<object> parameters)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
                object value = methodInfo.Invoke(this, objects.ToArray());
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
            throw new NotImplementedException();
        }

        public void ScrollToElement(IWebElement webElement)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
            try
            {
                if (webElement != null && !webElement.IsNullOrEmpty()) webElement.ScrollIntoView(true);
            }
            catch (Exception exception)
            {
                LastError = exception;
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public object RunScript()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type type = GetType();
            foreach (PropertyInfo prop in type.GetProperties())
            {
                // Setting the externals parameters of the context
                JintEngine.SetParameter(prop.Name, prop.GetValue(this, null));
            }

            // Running the script
            object value = JintEngine.Run(JavaScript);
            Debug.WriteLine("value = " + String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
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
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void Click(IWebElement webElement)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void DoubleClick(IWebElement webElement)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void KeyDown(IWebElement webElement, VirtualKeyCode code)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void KeyPress(IWebElement webElement, VirtualKeyCode code)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void KeyUp(IWebElement webElement, VirtualKeyCode code)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void TextEntry(IWebElement webElement, string text)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public IWebElement Select(IWebElement webElement)
        {
            Debug.Assert(!(webElement is IWebDocument));
            Debug.Assert(!(webElement is IWebWindow));
            return webElement;
        }

        #endregion

        #region

        public object[] Focus(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object[] Click(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object[] DoubleClick(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object[] KeyDown(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.Assert(arguments[1] is VirtualKeyCode);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object[] KeyPress(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.Assert(arguments[1] is VirtualKeyCode);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object[] KeyUp(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.Assert(arguments[1] is VirtualKeyCode);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object[] TextEntry(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.Assert(arguments[1] is string);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        public object[] Select(params object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Type[] types = arguments.Select(argument => argument.GetType()).ToArray();
            types[0] = typeof (ManagedWebElement);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types);
            Debug.Assert(methodInfo != null);
            object[] value = InvokeMethod(methodInfo, arguments);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        #endregion

        private object[] InvokeMethod(MethodInfo methodInfo, object[] arguments)
        {
            Debug.Assert(arguments[0] is string);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            HtmlNode[] nodes = HtmlDocument.DocumentNode.SelectNodes((string) arguments[0]).ToArray();
            IEnumerable<IWebElement> elements = GetElementByNode(nodes);
            object[] objects = arguments.ToArray();
            var results = new List<object>();
            foreach (IWebElement element in elements)
            {
                objects[0] = element;
                results.Add(methodInfo.Invoke(this, objects));
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return results.ToArray();
        }

        public static Rectangle get_Rectangle(IWebElement webElement)
        {
            Debug.Assert(webElement != null);
            var rect = new Rectangle(0, 0, webElement.OffsetRectangle.Width, webElement.OffsetRectangle.Height);
            for (IWebElement current = webElement; current != null; current = current.OffsetParent)
            {
                Thread.Sleep(0);
                Rectangle currentRect = current.OffsetRectangle;
                rect.X += currentRect.X;
                rect.Y += currentRect.Y;
            }
            return rect;
        }

        public static string get_XPath(IWebElement webElement)
        {
            Debug.Assert(webElement != null);
            string xpath = "";
            for (IWebElement parent = webElement.Parent;
                parent != null && !parent.IsNullOrEmpty();
                parent = parent.Parent)
            {
                Thread.Sleep(0);
                int index = 0;
                foreach (IWebElement child in parent.Children)
                {
                    Thread.Sleep(0);
                    if (string.Compare(child.TagName, webElement.TagName, StringComparison.OrdinalIgnoreCase) ==
                        0)
                        index++;
                    if (child.Equals(webElement))
                    {
                        xpath = string.Format(@"/{0}[{1}]{2}", webElement.TagName, index, xpath);
                        break;
                    }
                }
                webElement = parent;
            }
            xpath = string.Format(@"/{0}[{1}]{2}", webElement.TagName, 1, xpath);
            return xpath.ToLower();
        }
    }
}