using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using MyParser.Library;
using MyParser.WebView;

namespace MyParser.Managed
{
    public sealed class ManagedWebBrowser : ManagedObject.ManagedObject, IWebBrowser
    {
        public ManagedWebBrowser(object objectInstance)
            : base(objectInstance)
        {
        }

        public IWebDocument Document
        {
            get
            {
                if (ObjectInstance is CefSharp.WinForms.WebView)
                {
                    return new WebViewDocument {WebView = (CefSharp.WinForms.WebView) ObjectInstance};
                }
                if (ObjectInstance is WebBrowser)
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                    Debug.Assert(methodInfo != null);
                    var parameters = new object[] {};
                    return new ManagedWebDocument(methodInfo.Invoke(ObjectInstance, parameters));
                }
                throw new NotImplementedException();
            }
        }

        public Uri Url
        {
            get
            {
                if (ObjectInstance is CefSharp.WinForms.WebView)
                {
                    return new Uri((ObjectInstance as CefSharp.WinForms.WebView).Address);
                }
                if (ObjectInstance is WebBrowser)
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                    Debug.Assert(methodInfo != null);
                    var parameters = new object[] {};
                    return (Uri) methodInfo.Invoke(ObjectInstance, parameters);
                }
                throw new NotImplementedException();
            }
            set
            {
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
            }
        }

        public IntPtr Handle
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                return (IntPtr) methodInfo.Invoke(ObjectInstance, parameters);
            }
        }

        public void Navigate(string url)
        {
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
        }

        public Form FindForm()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {};
            return (Form) methodInfo.Invoke(ObjectInstance, parameters);
        }

        public bool Focus()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {};
            return (bool) methodInfo.Invoke(ObjectInstance, parameters);
        }
    }
}