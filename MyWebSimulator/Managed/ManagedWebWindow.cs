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
    public sealed class ManagedWebWindow : ManagedObject, IWebWindow
    {
        public ManagedWebWindow(object objectInstance) : base(objectInstance)
        {
        }

        public IWebElement WindowFrameElement
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                var value = new ManagedWebElement(methodInfo.Invoke(ObjectInstance, parameters));
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebWindow[] Frames
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                var collection = (IEnumerable) methodInfo.Invoke(ObjectInstance, parameters);
                IWebWindow[] value =
                    (from object item in collection select new ManagedWebWindow(item)).Cast<IWebWindow>().ToArray();
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public IWebDocument Document
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                var value = new ManagedWebDocument(methodInfo.Invoke(ObjectInstance, parameters));
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public bool Equals(IWebWindow obj)
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