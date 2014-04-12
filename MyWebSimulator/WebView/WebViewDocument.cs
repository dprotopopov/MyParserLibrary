using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using MyLibrary;
using MyLibrary.Types;
using Type = System.Type;

namespace MyWebSimulator.WebView
{
    public class WebViewDocument : WebViewImpl, IWebDocument
    {
        public IWebElement RootElement
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var value = new WebViewElement {WebView = WebView, XPath = XPath + @"/html[1]"};
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebElement Body
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var value = new WebViewElement {WebView = WebView, XPath = XPath + @"/html[1]/body[1]"};
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebElement[] All
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var dictionaryOfList = new Values {{JavaScript.TagArgument, @"*"}};
                var sb = new StringBuilder(JavaScript.GetElementsByTagName);
                sb.Append(JavaScript.ConvertToXPaths);
                string[] xpaths =
                    ((string) EvaluateScript(sb.ToString(), dictionaryOfList)).Split
                        (',');
                IWebElement[] value = (from xpath in xpaths
                    where !System.String.IsNullOrEmpty(xpath)
                    select new WebViewElement
                    {
                        WebView = WebView,
                        XPath = XPath + xpath
                    }).Cast<IWebElement>().ToArray();
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebWindow Window
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                var value = new WebViewWindow {WebView = WebView, XPath = XPath};
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public bool Equals(IWebDocument obj)
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