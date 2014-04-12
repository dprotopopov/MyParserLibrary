using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using MyLibrary.ManagedObject;
using MyParser;
using MyWebSimulator.WebView;
using String = MyLibrary.Types.String;

namespace MyWebSimulator.Managed
{
    public sealed class ManagedWebBrowser : ManagedObject, IWebBrowser
    {
        public ManagedWebBrowser(object objectInstance)
            : base(objectInstance)
        {
        }

        public IWebDocument Document
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                if (ObjectInstance is CefSharp.WinForms.WebView)
                {
                    var value = new WebViewDocument {WebView = (CefSharp.WinForms.WebView) ObjectInstance, XPath = @""};
                    Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                    Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                    return value;
                }
                if (ObjectInstance is WebBrowser)
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                    Debug.Assert(methodInfo != null);
                    var parameters = new object[] {};
                    var value = new ManagedWebDocument(methodInfo.Invoke(ObjectInstance, parameters));
                    Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                    Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                    return value;
                }
                throw new NotImplementedException();
            }
        }

        public Uri Url
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                if (ObjectInstance is CefSharp.WinForms.WebView)
                {
                    var value = new Uri((ObjectInstance as CefSharp.WinForms.WebView).Address);
                    Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                    Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                    return value;
                }
                if (ObjectInstance is WebBrowser)
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                    Debug.Assert(methodInfo != null);
                    var parameters = new object[] {};
                    object value = methodInfo.Invoke(ObjectInstance, parameters);
                    Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                    Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                    return (Uri) value;
                }
                throw new NotImplementedException();
            }
            set
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                if (ObjectInstance is CefSharp.WinForms.WebView)
                {
                    (ObjectInstance as CefSharp.WinForms.WebView).Address = value.ToString();
                }
                else if (ObjectInstance is WebBrowser)
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                    Debug.Assert(methodInfo != null);
                    var parameters = new object[] {value};
                    methodInfo.Invoke(ObjectInstance, parameters);
                }
                else
                    throw new NotImplementedException();
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
        }

        public IntPtr Handle
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                object value = methodInfo.Invoke(ObjectInstance, parameters);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return (IntPtr) value;
            }
        }

        public void Navigate(string url)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine("url = " + url);
            if (ObjectInstance is CefSharp.WinForms.WebView)
            {
                (ObjectInstance as CefSharp.WinForms.WebView).Load(url);
            }
            else if (ObjectInstance is WebBrowser)
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                var types = new[] {typeof (string)};
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName, types);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {url};
                methodInfo.Invoke(ObjectInstance, parameters);
            }
            else
                throw new NotImplementedException();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public Form FindForm()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {};
            object value = methodInfo.Invoke(ObjectInstance, parameters);
            Debug.WriteLine("value = " + String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return (Form) value;
        }

        public bool Focus()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {};
            object value = methodInfo.Invoke(ObjectInstance, parameters);
            Debug.WriteLine("value = " + String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return (bool) value;
        }
    }
}