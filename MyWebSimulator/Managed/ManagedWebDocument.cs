using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MyLibrary.ManagedObject;
using MyParser;
using String = MyLibrary.Types.String;

namespace MyWebSimulator.Managed
{
    public sealed class ManagedWebDocument : ManagedObject, IWebDocument
    {
        public ManagedWebDocument(object objectInstance) : base(objectInstance)
        {
        }

        public IWebElement RootElement
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                IWebElement value = Body.Parent;
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebElement Body
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {};
                var value = new ManagedWebElement(methodInfo.Invoke(ObjectInstance, parameters));
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebElement[] All
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {};
                var collection = (IEnumerable) methodInfo.Invoke(ObjectInstance, parameters);
                IWebElement[] value =
                    (from object item in collection select new ManagedWebElement(item)).Cast<IWebElement>().ToArray();
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebWindow Window
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                object[] parameters = {};
                var value = new ManagedWebWindow(methodInfo.Invoke(ObjectInstance, parameters));
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public bool Equals(IWebDocument obj)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.Assert(obj is IManagedObject);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (ManagedObject)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {obj};
            object value = methodInfo.Invoke(this, objects);
            Debug.WriteLine("value = " + String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return (bool) value;
        }
    }
}