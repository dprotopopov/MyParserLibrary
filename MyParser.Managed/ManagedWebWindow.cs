using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ManagedObject;
using MyParser.Library;

namespace MyParser.Managed
{
    public sealed class ManagedWebWindow : ManagedObject.ManagedObject, IWebWindow
    {
        public ManagedWebWindow(object objectInstance) : base(objectInstance)
        {
        }

        public IWebElement WindowFrameElement
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] { };
                return new ManagedWebElement(methodInfo.Invoke(ObjectInstance, parameters));
            }
        }

        public IWebWindow[] Frames
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] { };
                var collection = (IEnumerable) methodInfo.Invoke(ObjectInstance, parameters);
                return (from object item in collection select new ManagedWebWindow(item)).Cast<IWebWindow>().ToArray();
            }
        }

        public IWebDocument Document
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] { };
                return new ManagedWebDocument(methodInfo.Invoke(ObjectInstance, parameters));
            }
        }

        public bool Equals(IWebWindow obj)
        {
            Debug.Assert(obj is IManagedObject);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (ManagedObject.ManagedObject)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = { obj };
            return (bool) methodInfo.Invoke(this, objects);
        }
    }
}