﻿using System;
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
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace MyParserLibrary
{
    public class WebSimulator : WebTask, IWebSimulator
    {
        public WebSimulator()
        {
            Simulator = this;
            InputSimulator = new InputSimulator();
            DocumentCompleted = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"while(Simulator.get_DocumentCompleted()==0) Simulator.Sleep(1000);");
            sb.AppendLine(@"return 0;");
            JavaScript = sb.ToString();
            JintEngine = new JintEngine();
            JintEngine.SetFunction("alert", new Action<object>(s => Debug.WriteLine(s)));
        }

        public HtmlDocument Document
        {
            get { return WebBrowser.Document; }
        }

        public WebSimulator Simulator { get; set; }
        public WebBrowser WebBrowser { get; set; }

        public Uri Uri
        {
            get { return WebBrowser.Url; }
            set { WebBrowser.Url = value; }
        }

        public InputSimulator InputSimulator { get; set; }

        public IMouseSimulator MouseSimulator
        {
            get { return InputSimulator.Mouse; }
        }

        public IKeyboardSimulator KeyboardSimulator
        {
            get { return InputSimulator.Keyboard; }
        }

        public HtmlAgilityPack.HtmlDocument HtmlDocument
        {
            get
            {
                var document = new HtmlAgilityPack.HtmlDocument();
                if (Document.Body != null)
                    if (Document.Body.Parent != null)
                        document.LoadHtml(Document.Body.Parent.OuterHtml);
                return document;
            }
        }

        public JintEngine JintEngine { get; set; }
        public string JavaScript { get; set; }

        public Dictionary<EventInfo, string> ElementEvents
        {
            get
            {
                Type type = typeof(HtmlElement);
                return type.GetEvents().ToDictionary(item => item, item => item.Name);
            }
        }

        public Dictionary<MethodInfo, string> MouseMethods
        {
            get
            {
                Type type = typeof(MouseSimulator);
                return type.GetMethods().ToDictionary(item => item, item => item.Name);
            }
        }

        public Dictionary<MethodInfo, string> KeyboardMethods
        {
            get
            {
                Type type = typeof(KeyboardSimulator);
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
        public HtmlElement HighlightedElement { get; set; }

        #region

        public void Focus(HtmlElement htmlElement)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = htmlElement, true, true);
            htmlElement.Focus();
        }

        public void Click(HtmlElement htmlElement)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = htmlElement, true, true);
            htmlElement.Focus();
            InputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
        }

        public void DoubleClick(HtmlElement htmlElement)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = htmlElement, true, true);
            htmlElement.Focus();
            InputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
            InputSimulator.Keyboard.KeyDown(VirtualKeyCode.RETURN);
        }

        public void KeyDown(HtmlElement htmlElement, VirtualKeyCode code)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = htmlElement, true, true);
            htmlElement.Focus();
            InputSimulator.Keyboard.KeyDown(code);
        }

        public void KeyPress(HtmlElement htmlElement, VirtualKeyCode code)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = htmlElement, true, true);
            htmlElement.Focus();
            InputSimulator.Keyboard.KeyPress(code);
        }

        public void KeyUp(HtmlElement htmlElement, VirtualKeyCode code)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = htmlElement, true, true);
            htmlElement.Focus();
            InputSimulator.Keyboard.KeyUp(code);
        }

        public void TextEntry(HtmlElement htmlElement, string text)
        {
            HighlightElement(HighlightedElement, false, false);
            HighlightElement(HighlightedElement = htmlElement, true, true);
            htmlElement.Focus();
            InputSimulator.Keyboard.TextEntry(text);
        }

        public HtmlElement Select(HtmlElement htmlElement)
        {
            return htmlElement;
        }

        #endregion

        #region

        public object[] Focus(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] Click(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] DoubleClick(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] KeyDown(string xpath, VirtualKeyCode code)
        {
            var parameters = new List<object> { code };
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] KeyPress(string xpath, VirtualKeyCode code)
        {
            var parameters = new List<object> { code };
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] KeyUp(string xpath, VirtualKeyCode code)
        {
            var parameters = new List<object> { code };
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] TextEntry(string xpath, string text)
        {
            var parameters = new List<object> { text };
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        public object[] Select(string xpath)
        {
            var parameters = new List<object>();
            var types = new List<Type> { typeof(HtmlElement) };
            ParameterInfo[] parameterInfos = MethodBase.GetCurrentMethod().GetParameters();
            for (int i = 1; i < parameterInfos.Count(); i++) types.Add(parameterInfos[i].ParameterType);
            MethodInfo methodInfo = GetType().GetMethod(MethodBase.GetCurrentMethod().Name, types.ToArray());
            return InvokeMethod(methodInfo, xpath, parameters).ToArray();
        }

        #endregion

        public static Rectangle GetElementRectangle(HtmlElement htmlElement)
        {
            var rect = new Rectangle(0, 0, htmlElement.OffsetRectangle.Width, htmlElement.OffsetRectangle.Height);
            for (HtmlElement current = htmlElement; current != null; current = current.OffsetParent)
            {
                Rectangle currentRect = current.OffsetRectangle;
                rect.X += currentRect.X;
                rect.Y += currentRect.Y;
            }
            return rect;
        }

        /// <summary>
        ///     Возвращает строку, которая представляет текущий объект.
        /// </summary>
        /// <returns>
        ///     Строка, представляющая текущий объект.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Uri.ToString();
        }

        public void Sleep(int millisecondsTimeout)
        {
            Thread.Sleep(millisecondsTimeout);
        }

        public override void ThreadProc(object obj)
        {
            var This = obj as WebSimulator;
            Debug.Assert(This != null, "This != null");
            Arguments arguments = This.ParserLibrary.BuildArguments(This);
            string url = This.ParserLibrary.ParseTemplate(This.Url, arguments);
            This.WebBrowser.Navigate(url);
            This.RunScript();
            HtmlAgilityPack.HtmlDocument doc = This.HtmlDocument;
            try
            {
                This.ReturnFields = This.ParserLibrary.BuildReturnFields(doc.DocumentNode, arguments,
                    This.ReturnFieldInfos);
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

        public string GetXPath(HtmlElement item)
        {
            string xpath = "";

            for (HtmlElement parent = item.Parent; parent != null; parent = parent.Parent)
            {
                int index = 0;
                foreach (HtmlElement child in parent.Children)
                {
                    if (String.Compare(child.TagName, item.TagName, StringComparison.OrdinalIgnoreCase) == 0)
                        index++;
                    if (child == item)
                    {
                        xpath = @"/" + item.TagName + "[" + index + "]" + xpath;
                        break;
                    }
                }
                item = parent;
            }
            return @"/" + xpath.ToLower();
        }

        public string XPathToMask(string mask)
        {
            mask = new Regex(@"(\/|\[|\])").Replace(mask, @"\$&");
            mask = new Regex(
                @"(\\\/table\\\[(?<i1>\d+)\\\])(\\\/(tbody|thead|tfoot)\\\[(\d+)\\\])?(\\\/tr\\\[(?<i2>\d+)\\\])")
                .Replace(mask, @"\/table\[${i1}\](\/(tbody|thead|tfoot)\[\d+\])?\/tr\[${i2}\]");
            mask = new Regex(@".+").Replace(mask, @"\B$&\B");
            return mask;
        }

        public string XPathSanitize(string xpath)
        {
            xpath =
                new Regex(@"(?<start>\/(ol|ul|li|tr|td)\[)(?<i1>\d+)((\]\k<start>)(?<i2>\d+))+(?<end>\])").Replace(
                    xpath,
                    DoubleTagMatchEvaluator);
            return xpath;
        }

        private static string DoubleTagMatchEvaluator(Match m)
        {
            int i = Convert.ToInt32(m.Groups["i1"].Value) +
                    m.Groups["i2"].Captures.Cast<Capture>().Sum(capture => Convert.ToInt32(capture.Value));
            return m.Groups["start"].Value + i + m.Groups["end"].Value;
        }

        /// <summary>
        ///     Get HtmlElement by HtmlNode
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public List<HtmlElement> GetElementByNode(List<HtmlNode> nodes)
        {
            var elements = new List<HtmlElement>();

            var dictionaryNode = new Dictionary<HtmlNode, string>();
            var dictionaryElement = new Dictionary<HtmlElement, string>();
            foreach (HtmlElement item in Document.All)
            {
                string xpath = XPathSanitize(GetXPath(item));
                dictionaryElement.Add(item, xpath);
            }
            foreach (HtmlNode item in nodes)
            {
                string xpath = new Regex(@"\B\/html\[\d+\]").Replace(XPathSanitize(item.XPath.ToLower()), @"/");
                dictionaryNode.Add(item, xpath);
            }

            foreach (var item in dictionaryNode.Select(node => XPathToMask(node.Value))
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

        public List<HtmlNode> GetNodeByElement(List<HtmlElement> elements)
        {
            var nodes = new List<HtmlNode>();

            var dictionaryNode = new Dictionary<HtmlNode, string>();
            var dictionaryElement = new Dictionary<HtmlElement, string>();
            foreach (HtmlElement item in elements)
            {
                string xpath = XPathSanitize(GetXPath(item));
                dictionaryElement.Add(item, xpath);
            }
            foreach (HtmlNode item in HtmlDocument.DocumentNode.SelectNodes(@"//*").ToList())
            {
                string xpath = new Regex(@"\B\/html\[\d+\]").Replace(XPathSanitize(item.XPath.ToLower()), @"/");
                dictionaryNode.Add(item, xpath);
            }

            foreach (var item in dictionaryElement.Select(element => XPathToMask(element.Value))
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

        public void HighlightElement(HtmlElement element, bool highlight, bool scrollToElement)
        {
            if (scrollToElement) ScrollToElement(element);

            var styles = new Dictionary<string, string>
            {
                {@"border:red 4px solid;", @"border(-(left|right|top|bottom))?\s*:(\s*(red|4px|solid)){1,3}(\s*;)?"},
                {@"border-color:red;", @"border(-(left|right|top|bottom))?-color\s*:\s*red(\s*;)?"},
                {@"border-style:solid;", @"border(-(left|right|top|bottom))?-style\s*:\s*solid(\s*;)?"},
                {@"border-width:4px;", @"border(-(left|right|top|bottom))?-width\s*:\s*4px(\s*;)?"},
            };
            if (element != null)
            {
                if (highlight)
                {
                    try
                    {
                        element.Style += styles.Keys.Aggregate((i, j) => i + j);
                    }
                    catch (Exception)
                    {
                        element.Style = styles.Keys.Aggregate((i, j) => i + j);
                    }
                }
                else
                {
                    try
                    {
                        foreach (
                            Regex regex in styles.Values.Select(pattern => new Regex(pattern, RegexOptions.IgnoreCase)))
                        {
                            element.Style = regex.Replace(element.Style, @"");
                        }
                    }
                    catch (Exception)
                    {
                        element.Style = @"";
                    }
                }
            }
        }

        private List<object> InvokeMethod(MethodInfo methodInfo, string xpath, List<object> args)
        {
            var results = new List<object>();
            List<HtmlNode> nodes = HtmlDocument.DocumentNode.SelectNodes(xpath).ToList();
            List<HtmlElement> elements = GetElementByNode(nodes);
            SetForegroundCurrentProcessMainWindow();
            SetWebBrowserFormFocus();
            SetWebBrowserControlFocus();
            foreach (var objects in elements.Select(element => new List<object> { element }))
            {
                objects.AddRange(args);
                results.Add(methodInfo.Invoke(this, objects.ToArray()));
            }
            return results;
        }

        public object SimulateTextEntry(HtmlElement htmlElement, List<object> parameters)
        {
            MethodInfo methodInfo = GetType().GetMethod("TextEntry", new[] { typeof(HtmlElement), typeof(string) });
            var objects = new List<object> { htmlElement };
            objects.AddRange(parameters);
            SetForegroundCurrentProcessMainWindow();
            SetWebBrowserFormFocus();
            SetWebBrowserControlFocus();
            return methodInfo.Invoke(this, objects.ToArray());
        }

        public object SimulateEvent(EventInfo eventInfo, HtmlElement htmlElement, List<object> parameters)
        {
            var dictionary = new Dictionary<EventInfo, MethodInfo>
            {
                {typeof (HtmlElement).GetEvent("GotFocus"), GetType().GetMethod("Focus", new[] {typeof (HtmlElement)})},
                {typeof (HtmlElement).GetEvent("Click"), GetType().GetMethod("Click", new[] {typeof (HtmlElement)})},
                {
                    typeof (HtmlElement).GetEvent("DoubleClick"),
                    GetType().GetMethod("DoubleClick", new[] {typeof (HtmlElement)})
                },
                {
                    typeof (HtmlElement).GetEvent("KeyDown"),
                    GetType().GetMethod("KeyDown", new[] {typeof (HtmlElement), typeof (VirtualKeyCode)})
                },
                {
                    typeof (HtmlElement).GetEvent("KeyPress"),
                    GetType().GetMethod("KeyPress", new[] {typeof (HtmlElement), typeof (VirtualKeyCode)})
                },
                {
                    typeof (HtmlElement).GetEvent("KeyUp"),
                    GetType().GetMethod("KeyUp", new[] {typeof (HtmlElement), typeof (VirtualKeyCode)})
                },
            };
            if (dictionary.ContainsKey(eventInfo))
            {
                MethodInfo methodInfo = dictionary[eventInfo];
                var objects = new List<object> { htmlElement };
                objects.AddRange(parameters);
                SetForegroundCurrentProcessMainWindow();
                SetWebBrowserFormFocus();
                SetWebBrowserControlFocus();
                return methodInfo.Invoke(this, objects.ToArray());
            }
            throw new NotImplementedException();
        }

        public void ScrollToElement(HtmlElement htmlElement)
        {
            if (htmlElement != null)
            {
                htmlElement.ScrollIntoView(true);
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
    }
}