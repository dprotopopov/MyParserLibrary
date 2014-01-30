using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace MyParserLibrary
{
    public class WebElement : ObjectManager, IWebElement
    {
        public WebElement(object managedObject)
            : base(managedObject)
        {
        }

        public IWebElement Parent
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public string TagName
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return methodInfo.Invoke(ManagedObject, parameters).ToString();
            }
        }

        public string OuterHtml
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return methodInfo.Invoke(ManagedObject, parameters).ToString();
            }
        }

        public string Style
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return methodInfo.Invoke(ManagedObject, parameters).ToString();
            }
            set
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {value};
                methodInfo.Invoke(ManagedObject, parameters);
            }
        }

        public IWebElement[] Children
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                var collection = (IEnumerable) methodInfo.Invoke(ManagedObject, parameters);
                List<IWebElement> list =
                    (from object item in collection select (IWebElement) new WebElement(item)).ToList();
                return list.ToArray();
            }
        }

        public Rectangle OffsetRectangle
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return (Rectangle) methodInfo.Invoke(ManagedObject, parameters);
            }
        }

        public IWebElement OffsetParent
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public string XPath
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof(MyParserLibrary).GetMethod(methodName);
                var parameters = new object[] { this };
                return (string) methodInfo.Invoke(null, parameters);
            }
        }

        public void Focus()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
            var parameters = new object[] {};
            methodInfo.Invoke(ManagedObject, parameters);
        }

        public void ScrollIntoView(bool b)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
            var parameters = new object[] {b};
            methodInfo.Invoke(ManagedObject, parameters);
        }

        public bool Equals(IWebElement obj)
        {
            Debug.Assert(obj is IObjectManager);
            return Equals(obj as IObjectManager);
        }

        public Rectangle Rectangle
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof(MyParserLibrary).GetMethod(methodName);
                var parameters = new object[] { this };
                return (Rectangle) methodInfo.Invoke(null, parameters);
            }
        }
    }
}