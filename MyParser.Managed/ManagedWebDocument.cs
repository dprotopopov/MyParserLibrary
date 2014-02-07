using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ManagedObject;
using MyParser.Library;

namespace MyParser.Managed
{
    public sealed class ManagedWebDocument : ManagedObject.ManagedObject, IWebDocument
    {
        public ManagedWebDocument(object objectInstance) : base(objectInstance)
        {
        }

        public IWebElement Body
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                return new ManagedWebElement(methodInfo.Invoke(ObjectInstance, parameters));
            }
        }

        public IWebElement[] All
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                var collection = (IEnumerable) methodInfo.Invoke(ObjectInstance, parameters);
                return (from object item in collection select new ManagedWebElement(item)).Cast<IWebElement>().ToArray();
            }
        }

        public IWebWindow Window
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                return new ManagedWebWindow(methodInfo.Invoke(ObjectInstance, parameters));
            }
        }

        public bool Equals(IWebDocument obj)
        {
            Debug.Assert(obj is IManagedObject);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (ManagedObject.ManagedObject)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {obj};
            return (bool) methodInfo.Invoke(this, objects);
        }
    }
}