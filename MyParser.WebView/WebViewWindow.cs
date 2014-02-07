using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using MyParser.Library;

namespace MyParser.WebView
{
    public class WebViewWindow : WebViewElement, IWebWindow
    {
        public IWebElement WindowFrameElement
        {
            get { return this; }
        }

        public IWebWindow[] Frames
        {
            get
            {
                var arguments = new ParametersValues
                {
                    {TagArgument, @"frame"},
                    {TagArgument, @"iframe"},
                };
                var sb = new StringBuilder();
                sb.Append(GetElementsByTagName);
                sb.Append(GetElementsByTagName);
                sb.Append(ConvertToXPaths);
                sb.Append(ReturnQueueJoin);
                string windowXpath = ((WebViewWindow) Window).XPath;
                string[] xpaths =
                    ((string) EvaluateScript(sb.ToString(), arguments)).Split
                        (',');
                return xpaths.Select(xpath => (IWebWindow) new WebViewWindow
                {
                    WebView = WebView,
                    XPath = windowXpath + xpath
                }).ToArray();
            }
        }

        public IWebDocument Document
        {
            get { return new WebViewDocument {WebView = WebView, XPath = XPath}; }
        }

        public bool Equals(IWebWindow obj)
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