using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using MyLibrary;
using MyLibrary.Types;
using Type = System.Type;

namespace MyWebSimulator.WebView
{
    public class WebViewWindow : WebViewImpl, IWebWindow
    {
        public IWebElement WindowFrameElement
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var value = new WebViewElement {WebView = WebView, XPath = XPath};
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebWindow[] Frames
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var dictionaryOfList = new Values
                {
                    {JavaScript.TagArgument, @"frame"},
                    {JavaScript.TagArgument, @"iframe"},
                };
                var sb = new StringBuilder(JavaScript.CloneQueueLast);
                sb.Append(JavaScript.GetElementsByTagName);
                sb.Append(JavaScript.GetElementsByTagName);
                sb.Append(JavaScript.ConvertToXPaths);
                string[] xpaths =
                    ((string) EvaluateScript(sb.ToString(), dictionaryOfList)).Split
                        (',');
                IWebWindow[] value = (from xpath in xpaths
                    where !System.String.IsNullOrEmpty(xpath)
                    select new WebViewWindow
                    {
                        WebView = WebView,
                        XPath = XPath + xpath
                    }).Cast<IWebWindow>().ToArray();
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebDocument Document
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var value = new WebViewDocument {WebView = WebView, XPath = XPath};
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public bool Equals(IWebWindow obj)
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
    }
}