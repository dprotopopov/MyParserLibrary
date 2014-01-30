using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace MyParserLibrary
{
    public class WebElement : ObjectManager
    {
        public WebElement(object managedObject)
            : base(managedObject)
        {
        }

        public WebElement Parent
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { };
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public string TagName
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { };
                return methodInfo.Invoke(ManagedObject, parameters).ToString();
            }
        }

        public string OuterHtml
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { };
                return methodInfo.Invoke(ManagedObject, parameters).ToString();
            }
        }

        public string Style
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { };
                return methodInfo.Invoke(ManagedObject, parameters).ToString();
            }
            set
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { value };
                methodInfo.Invoke(ManagedObject, parameters);
            }
        }

        public WebElement[] Children
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { };
                var collection = (IEnumerable)methodInfo.Invoke(ManagedObject, parameters);
                List<WebElement> list = (from object item in collection select new WebElement(item)).ToList();
                return list.ToArray();
            }
        }

        public Rectangle OffsetRectangle
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { };
                return (Rectangle)methodInfo.Invoke(ManagedObject, parameters);
            }
        }

        public WebElement OffsetParent
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] { };
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public string XPath
        {
            get
            {
                string xpath = "";
                WebElement webElement = this;
                for (WebElement parent = webElement.Parent; !parent.IsNullOrEmpty(); parent = parent.Parent)
                {
                    int index = 0;
                    foreach (WebElement child in parent.Children)
                    {
                        if (String.Compare(child.TagName, webElement.TagName, StringComparison.OrdinalIgnoreCase) ==
                            0)
                            index++;
                        if (child.Equals(webElement))
                        {
                            xpath = @"/" + webElement.TagName + "[" + index + "]" + xpath;
                            break;
                        }
                    }
                    webElement = parent;
                }
                return @"/" + xpath.ToLower();
            }
        }

        public void Focus()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
            var parameters = new object[] { };
            methodInfo.Invoke(ManagedObject, parameters);
        }

        public void ScrollIntoView(bool b)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
            var parameters = new object[] { b };
            methodInfo.Invoke(ManagedObject, parameters);
        }
    }
}