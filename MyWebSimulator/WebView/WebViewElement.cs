using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WindowsInput.Native;
using MyLibrary;
using Regex = System.Text.RegularExpressions.Regex;
using String = MyLibrary.Types.String;

namespace MyWebSimulator.WebView
{
    public class WebViewElement : WebViewImpl, IWebElement
    {
        private WebViewWindow Window
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                MatchCollection matches = Regex.Matches(XPath, MyLibrary.XPath.FramePattern);
                string xpath = (matches.Count > 0)
                    ? XPath.Substring(0, matches[matches.Count - 1].Index + matches[matches.Count - 1].Length)
                    : @"";
                var value = new WebViewWindow {WebView = WebView, XPath = xpath};
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebElement Parent
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                MatchCollection matches = Regex.Matches(XPath, MyLibrary.XPath.ElementPattern);
                WebViewElement value = (matches.Count > 0)
                    ? new WebViewElement
                    {
                        WebView = WebView,
                        XPath = XPath.Substring(0, matches[matches.Count - 1].Index)
                    }
                    : null;
                if (value != null)
                {
                    Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                }
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public string TagName
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                MatchCollection matches = Regex.Matches(XPath, MyLibrary.XPath.ElementPattern);
                string value = (matches.Count > 0) ? matches[matches.Count - 1].Groups["tag"].Value : @"";
                Debug.WriteLine("value = " + String.IntroText(value));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public string OuterHtml
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var dictionaryOfList = new Values
                {
                    {JavaScript.PropertyArgument, JavaScript.OuterHtmlProperty}
                };
                var sb = new StringBuilder(JavaScript.GetElementProperty);
                object value = EvaluateScript(sb.ToString(), dictionaryOfList);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return (string) value;
            }
        }


        public string Style
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var dictionaryOfList = new Values
                {
                    {JavaScript.MethodArgument, JavaScript.GetAttributeMethod},
                    {JavaScript.ParametersArgument, JavaScript.GetAttributeMethodParameters},
                    {JavaScript.AttributeArgument, JavaScript.StyleAttribute},
                };
                object value = EvaluateInvokeScript(dictionaryOfList);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return (string) value;
            }
            set
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                Debug.WriteLine("value = " + String.IntroText(value));
                var dictionaryOfList = new Values
                {
                    {JavaScript.MethodArgument, JavaScript.SetAttributeMethod},
                    {JavaScript.ParametersArgument, JavaScript.SetAttributeMethodParameters},
                    {JavaScript.AttributeArgument, JavaScript.StyleAttribute},
                    {JavaScript.ValueArgument, value}
                };
                EvaluateInvokeScript(dictionaryOfList);
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
        }

        public IWebElement[] Children
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var dictionaryOfList = new Values();
                var sb = new StringBuilder(JavaScript.GetElementChildNodes);
                sb.Append(JavaScript.ConvertToXPaths);
                string windowXpath = Window.XPath;
                string[] xpaths = ((string) EvaluateScript(sb.ToString(), dictionaryOfList)).Split(',');
                IWebElement[] value = (from xpath in xpaths
                    where !System.String.IsNullOrEmpty(xpath)
                    select new WebViewElement
                    {
                        WebView = WebView,
                        XPath = windowXpath + xpath
                    }).Cast<IWebElement>().ToArray();
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public Rectangle OffsetRectangle
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var dictionaryOfList = new Values
                {
                    {JavaScript.PropertyArgument, JavaScript.OffsetLeftProperty},
                    {JavaScript.PropertyArgument, JavaScript.OffsetTopProperty},
                    {JavaScript.PropertyArgument, JavaScript.OffsetWidthProperty},
                    {JavaScript.PropertyArgument, JavaScript.OffsetHeightProperty},
                };
                var sb = new StringBuilder();
                sb.Append(JavaScript.CloneQueueLast);
                sb.Append(JavaScript.CloneQueueLast);
                sb.Append(JavaScript.CloneQueueLast);
                sb.Append(JavaScript.GetElementProperty);
                sb.Append(JavaScript.GetElementProperty);
                sb.Append(JavaScript.GetElementProperty);
                sb.Append(JavaScript.GetElementProperty);
                string[] values = ((string) EvaluateScript(sb.ToString(), dictionaryOfList)).Split(',');
                var value = new Rectangle(
                    Convert.ToInt32(values[0]),
                    Convert.ToInt32(values[1]),
                    Convert.ToInt32(values[2]),
                    Convert.ToInt32(values[3]));
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebElement OffsetParent
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var dictionaryOfList = new Values
                {
                    {JavaScript.PropertyArgument, JavaScript.OffsetParentProperty}
                };
                var sb = new StringBuilder(JavaScript.GetElementProperty);
                sb.Append(JavaScript.ConvertToXPaths);
                var xpath = (string) EvaluateScript(sb.ToString(), dictionaryOfList);
                WebViewElement value = (!System.String.IsNullOrEmpty(xpath))
                    ? new WebViewElement
                    {
                        WebView = WebView,
                        XPath = Window.XPath + xpath
                    }
                    : null;
                if (value != null)
                {
                    Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                }
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }


        public bool Equals(IWebElement obj)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(obj is WebViewImpl);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (WebViewImpl)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {obj};
            object value = methodInfo.Invoke(this, objects);
            Debug.WriteLine("value = " + String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return (bool) value;
        }

        public Rectangle Rectangle
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof (WebSimulator).GetMethod(methodName);
                object[] objects = {this};
                object value = methodInfo.Invoke(null, objects);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return (Rectangle) value;
            }
        }

        #region

        public void Focus()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var dictionaryOfList = new Values
            {
                {JavaScript.MethodArgument, JavaScript.FocusMethod},
                {JavaScript.ParametersArgument, JavaScript.FocusMethodParameters},
            };
            EvaluateInvokeScript(dictionaryOfList);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void ScrollIntoView(bool b)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var dictionaryOfList = new Values
            {
                {JavaScript.MethodArgument, JavaScript.ScrollIntoViewMethod},
                {JavaScript.ParametersArgument, JavaScript.ScrollIntoViewMethodParameters},
                {JavaScript.BoolArgument, b ? "true" : "false"},
            };
            EvaluateInvokeScript(dictionaryOfList);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void Click()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            FireMouseEvent(JavaScript.ClickEventtype, 0);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void DoubleClick()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            FireEvent(JavaScript.OndblclickEvent);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void KeyDown(VirtualKeyCode code)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            FireKeyboardEvent(JavaScript.KeydownEventtype, (uint) code);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void KeyPress(VirtualKeyCode code)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            FireKeyboardEvent(JavaScript.KeypressEventtype, (uint) code);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void KeyUp(VirtualKeyCode code)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            FireKeyboardEvent(JavaScript.KeyupEventtype, (uint) code);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void TextEntry(string text)
        {
            throw new NotImplementedException();
        }

        private void FireEvent(string eventName)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var dictionaryOfList = new Values
            {
                {JavaScript.MethodArgument, JavaScript.FireEventMethod},
                {JavaScript.ParametersArgument, JavaScript.FireEventMethodParameters},
                {JavaScript.EventArgument, eventName}
            };
            EvaluateInvokeScript(dictionaryOfList);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void FireKeyboardEvent(string eventType, uint code)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var dictionaryOfList = new Values
            {
                {JavaScript.EventtypeArgument, eventType},
                {JavaScript.CodeArgument, code.ToString(CultureInfo.InvariantCulture)}
            };
            EvaluateScript(JavaScript.ElementFireKeyboardEvent, dictionaryOfList);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void FireMouseEvent(string eventType, int button)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var dictionaryOfList = new Values
            {
                {JavaScript.EventtypeArgument, eventType},
                {JavaScript.ButtonArgument, button.ToString(CultureInfo.InvariantCulture)}
            };
            EvaluateScript(JavaScript.ElementFireMouseEvent, dictionaryOfList);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        #endregion
    }
}