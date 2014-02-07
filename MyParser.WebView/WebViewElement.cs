using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WindowsInput.Native;
using MyParser.Library;

namespace MyParser.WebView
{
    public class WebViewElement : WebViewImpl, IWebElement
    {
        public IWebWindow Window
        {
            get
            {
                MatchCollection matches = Regex.Matches(XPath, FramePattern);
                string xpath = (matches.Count > 0)
                    ? XPath.Substring(0, matches[matches.Count - 1].Index + matches[matches.Count - 1].Length)
                    : @"";
                return new WebViewWindow {WebView = WebView, XPath = xpath};
            }
        }

        public IWebElement Parent
        {
            get
            {
                MatchCollection matches = Regex.Matches(XPath, ElementPattern);
                if (matches.Count > 0)
                {
                    return new WebViewElement
                    {
                        WebView = WebView,
                        XPath = XPath.Substring(0, matches[matches.Count - 1].Index)
                    };
                }
                return null;
            }
        }

        public string TagName
        {
            get
            {
                MatchCollection matches = Regex.Matches(XPath, ElementPattern);
                return (matches.Count > 0) ? matches[matches.Count - 1].Groups["tag"].Value : @"";
            }
        }

        public string OuterHtml
        {
            get
            {
                var arguments = new ParametersValues {{PropertyArgument, OuterHtmlProperty}};
                var sb = new StringBuilder(GetElementProperty);
                sb.Append(ReturnQueueJoin);
                return (string) EvaluateScript(sb.ToString(), arguments);
            }
        }

        public string Style
        {
            get
            {
                var parametersValues = new ParametersValues {{AttributeArgument, StyleAttribute}};
                var parameters = new List<string>
                {
                    GetAttributeMethodParameters,
                };
                var sb = new StringBuilder(GetAttributeMethod);
                sb.Append(ReturnQueueJoin);
                return (string) EvaluateInvokeScript(sb.ToString(), parameters, parametersValues);
            }
            set
            {
                var parametersValues = new ParametersValues
                {
                    {AttributeArgument, StyleAttribute},
                    {ValueArgument, value}
                };
                var parameters = new List<string>
                {
                    SetAttributeMethodParameters,
                };
                var sb = new StringBuilder(SetAttributeMethod);
                EvaluateInvokeScript(sb.ToString(), parameters, parametersValues);
            }
        }

        public IWebElement[] Children
        {
            get
            {
                var parametersValues = new ParametersValues();
                var sb = new StringBuilder(GetElementChildNodes);
                sb.Append(ConvertToXPaths);
                sb.Append(ReturnQueueJoin);
                string windowXpath = ((WebViewWindow) Window).XPath;
                string[] xpaths = ((string) EvaluateScript(sb.ToString(), parametersValues)).Split(',');
                return xpaths.Select(xpath => (IWebElement) new WebViewElement
                {
                    WebView = WebView,
                    XPath = windowXpath + xpath
                }).ToArray();
            }
        }

        public Rectangle OffsetRectangle
        {
            get
            {
                var parametersValues = new ParametersValues
                {
                    {PropertyArgument, OffsetLeftProperty},
                    {PropertyArgument, OffsetTopProperty},
                    {PropertyArgument, OffsetWidthProperty},
                    {PropertyArgument, OffsetHeightProperty},
                };
                var sb = new StringBuilder();
                sb.Append(CloneQueueLast);
                sb.Append(CloneQueueLast);
                sb.Append(CloneQueueLast);
                sb.Append(GetElementProperty);
                sb.Append(GetElementProperty);
                sb.Append(GetElementProperty);
                sb.Append(GetElementProperty);
                sb.Append(ReturnQueueJoin);
                string[] values = ((string) EvaluateScript(sb.ToString(), parametersValues)).Split(',');
                return new Rectangle(
                    Convert.ToInt32(values[0]),
                    Convert.ToInt32(values[1]),
                    Convert.ToInt32(values[2]),
                    Convert.ToInt32(values[3]));
            }
        }

        public IWebElement OffsetParent
        {
            get
            {
                var parametersValues = new ParametersValues {{PropertyArgument, OffsetParentProperty}};
                var sb = new StringBuilder(GetElementProperty);
                sb.Append(ConvertToXPaths);
                sb.Append(ReturnQueueJoin);
                var xpath = (string) EvaluateScript(sb.ToString(), parametersValues);
                if (!String.IsNullOrEmpty(xpath))
                    return new WebViewElement
                    {
                        WebView = WebView,
                        XPath = ((WebViewWindow) Window).XPath + xpath
                    };
                return null;
            }
        }


        public bool Equals(IWebElement obj)
        {
            Debug.Assert(obj is WebViewImpl);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (WebViewImpl)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {obj};
            return (bool) methodInfo.Invoke(this, objects);
        }

        public Rectangle Rectangle
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof (MyLibrary).GetMethod(methodName);
                var parameters = new object[] {this};
                return (Rectangle) methodInfo.Invoke(null, parameters);
            }
        }

        #region

        public void Focus()
        {
            var parametersValues = new ParametersValues();
            var parameters = new List<string>
            {
                FocusMethodParameters,
            };
            var sb = new StringBuilder(FocusMethod);
            EvaluateInvokeScript(sb.ToString(), parameters, parametersValues);
        }

        public void ScrollIntoView(bool b)
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            FireMouseEvent(ClickEventtype, 0);
        }

        public void DoubleClick()
        {
            FireMouseEvent(ClickEventtype, 0);
            FireMouseEvent(ClickEventtype, 0);
        }

        public void KeyDown(VirtualKeyCode code)
        {
            FireKeyboardEvent(KeydownEventtype, (uint) code);
        }

        public void KeyPress(VirtualKeyCode code)
        {
            FireKeyboardEvent(KeypressEventtype, (uint) code);
        }

        public void KeyUp(VirtualKeyCode code)
        {
            FireKeyboardEvent(KeyupEventtype, (uint) code);
        }

        public void TextEntry(string text)
        {
            throw new NotImplementedException();
        }

        protected void FireEvent(string eventName)
        {
            var parametersValues = new ParametersValues {{EventArgument, eventName}};
            var parameters = new List<string>
            {
                FocusMethodParameters,
            };
            var sb = new StringBuilder(FireEventMethod);
            EvaluateInvokeScript(sb.ToString(), parameters, parametersValues);
        }

        protected void FireKeyboardEvent(string eventType, uint code)
        {
            var parametersValues = new ParametersValues
            {
                {EventtypeArgument, eventType},
                {CodeArgument, code.ToString(CultureInfo.InvariantCulture)}
            };
            EvaluateScript(ElementFireKeyboardEvent, parametersValues);
        }

        protected void FireMouseEvent(string eventType, int button)
        {
            var parametersValues = new ParametersValues
            {
                {EventtypeArgument, eventType},
                {ButtonArgument, button.ToString(CultureInfo.InvariantCulture)}
            };
            EvaluateScript(ElementFireMouseEvent, parametersValues);
        }

        #endregion
    }
}