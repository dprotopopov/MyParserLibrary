using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using MyParser.Library;

namespace MyParser.WebView
{
    public class WebViewDocument : WebViewWindow, IWebDocument
    {
        public IWebElement Body
        {
            get
            {
                var arguments = new ParametersValues {{XpathArgument, @"/body[1]"}};
                var sb = new StringBuilder(GetElementByXPath);
                sb.Append(ConvertToXPaths);
                sb.Append(ReturnQueueJoin);
                var xpath = (string) EvaluateScript(sb.ToString(), arguments);
                return new WebViewElement {WebView = WebView, XPath = XPath + xpath};
            }
        }

        public IWebElement[] All
        {
            get
            {
                var arguments = new ParametersValues {{TagArgument, @"*"}};
                var sb = new StringBuilder(GetElementsByTagName);
                sb.Append(ConvertToXPaths);
                sb.Append(ReturnQueueJoin);
                string windowXpath = ((WebViewWindow) Window).XPath;
                string[] xpaths =
                    ((string) EvaluateScript(sb.ToString(), arguments)).Split
                        (',');
                return xpaths.Select(xpath => (IWebElement) new WebViewElement
                {
                    WebView = WebView,
                    XPath = windowXpath + xpath
                }).ToArray();
            }
        }

        public new IWebWindow Window
        {
            get { return this; }
        }

        public bool Equals(IWebDocument obj)
        {
            Debug.Assert(obj is WebViewImpl);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (WebViewImpl)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {obj};
            return (bool) methodInfo.Invoke(this, objects);
        }
    }
}