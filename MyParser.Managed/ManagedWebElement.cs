using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using WindowsInput.Native;
using ManagedObject;
using MyParser.Library;

namespace MyParser.Managed
{
    public sealed class ManagedWebElement : ManagedObject.ManagedObject, IWebElement
    {
        public ManagedWebElement(object objectInstance)
            : base(objectInstance)
        {
        }

        public IWebElement Parent
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

        public string TagName
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                return methodInfo.Invoke(ObjectInstance, parameters).ToString();
            }
        }

        public string OuterHtml
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                return methodInfo.Invoke(ObjectInstance, parameters).ToString();
            }
        }

        public string Style
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                return methodInfo.Invoke(ObjectInstance, parameters).ToString();
            }
            set
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {value};
                methodInfo.Invoke(ObjectInstance, parameters);
            }
        }

        public IWebElement[] Children
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                var collection = (IEnumerable) methodInfo.Invoke(ObjectInstance, parameters);
                List<IWebElement> list =
                    (from object item in collection select (IWebElement) new ManagedWebElement(item)).ToList();
                return list.ToArray();
            }
        }

        public Rectangle OffsetRectangle
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                return (Rectangle) methodInfo.Invoke(ObjectInstance, parameters);
            }
        }

        public IWebElement OffsetParent
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

        public string XPath
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof (MyLibrary).GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {this};
                return (string) methodInfo.Invoke(null, parameters);
            }
        }


        public bool Equals(IWebElement obj)
        {
            Debug.Assert(obj is IManagedObject);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (ManagedObject.ManagedObject)};
            MethodInfo methodInfo = GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {obj};
            return (bool) methodInfo.Invoke(this, objects);
        }

        public Rectangle Rectangle
        {
            get
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof (MyLibrary).GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {this};
                return (Rectangle) methodInfo.Invoke(null, parameters);
            }
        }

        #region

        public void ScrollIntoView(bool b)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {b};
            methodInfo.Invoke(ObjectInstance, parameters);
        }

        public void Focus()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {};
            methodInfo.Invoke(ObjectInstance, parameters);
        }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public void DoubleClick()
        {
            throw new NotImplementedException();
        }

        public void KeyDown(VirtualKeyCode code)
        {
            throw new NotImplementedException();
        }

        public void KeyPress(VirtualKeyCode code)
        {
            throw new NotImplementedException();
        }

        public void KeyUp(VirtualKeyCode code)
        {
            throw new NotImplementedException();
        }

        public void TextEntry(string text)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}