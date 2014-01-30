using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MyParserLibrary
{
    public class WebWindow : ObjectManager, IWebWindow
    {
        public WebWindow(object managedObject) : base(managedObject)
        {
        }

        public IWebElement WindowFrameElement
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebElement(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public IWebWindow[] Frames
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

        public IWebDocument Document
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ManagedObject.GetType().GetMethod(methodName);
                var parameters = new object[] {};
                return new WebDocument(methodInfo.Invoke(ManagedObject, parameters));
            }
        }

        public bool Equals(IWebWindow obj)
        {
            Debug.Assert(obj is IObjectManager);
            return Equals(obj as IObjectManager);
        }
    }
}