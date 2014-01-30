using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyParserLibrary
{
    public class WebDocument : ObjectManager
    {
        public WebDocument(object managedObject) : base(managedObject)
        {
        }

        public WebElement Body
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public WebElement[] All
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                var collection = (IEnumerable) methodInfo.Invoke(ManagedObject, parameters);
                List<WebElement> list = (from object item in collection select new WebElement(item)).ToList();
                return list.ToArray();
            }
        }

        public WebWindow Window
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebWindow(methodInfo.Invoke(ManagedObject, parameters));
            }
        }
    }
}