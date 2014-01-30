using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MyParserLibrary
{
    public class WebDocument : ObjectManager, IWebDocument
    {
        public WebDocument(object managedObject) : base(managedObject)
        {
        }

        public IWebElement Body
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public IWebElement[] All
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

        public IWebWindow Window
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebWindow(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public bool Equals(IWebDocument obj)
        {
            Debug.Assert(obj is IObjectManager);
            return Equals(obj as IObjectManager);
        }
    }
}