using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyParserLibrary
{
    public class WebWindow : ObjectManager
    {
        public WebWindow(object managedObject) : base(managedObject)
        {
        }

        public WebElement WindowFrameElement
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public WebWindow[] Frames
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                var collection = (IEnumerable) methodInfo.Invoke(ManagedObject, parameters);
                List<WebWindow> list = (from object item in collection select new WebWindow(item)).ToList();
                return list.ToArray();
            }
        }

        public WebDocument Document
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebDocument(methodInfo.Invoke(ManagedObject, parameters));
            }
        }
    }
}