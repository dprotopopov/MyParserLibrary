using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using WindowsInput.Native;
using MyLibrary.ManagedObject;
using MyParser;
using String = MyLibrary.Types.String;

namespace MyWebSimulator.Managed
{
    public sealed class ManagedWebElement : ManagedObject, IWebElement
    {
        public ManagedWebElement(object objectInstance)
            : base(objectInstance)
        {
        }

        public IWebElement Parent
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

        public string TagName
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                string value = methodInfo.Invoke(ObjectInstance, parameters).ToString();
                Debug.WriteLine("value = " + String.IntroText(value));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public string OuterHtml
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                string value = methodInfo.Invoke(ObjectInstance, parameters).ToString();
                Debug.WriteLine("value = " + String.IntroText(value));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
        }

        public string Style
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                string value = methodInfo.Invoke(ObjectInstance, parameters).ToString();
                Debug.WriteLine("value = " + String.IntroText(value));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value;
            }
            set
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                Debug.WriteLine("value = " + String.IntroText(value));
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {value};
                methodInfo.Invoke(ObjectInstance, parameters);
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
        }

        public IWebElement[] Children
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                var collection = (IEnumerable) methodInfo.Invoke(ObjectInstance, parameters);
                List<IWebElement> value =
                    (from object item in collection select (IWebElement) new ManagedWebElement(item)).ToList();
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return value.ToArray();
            }
        }

        public Rectangle OffsetRectangle
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {};
                object value = methodInfo.Invoke(ObjectInstance, parameters);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return (Rectangle) value;
            }
        }

        public IWebElement OffsetParent
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

        public string XPath
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof (WebSimulator).GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {this};
                object value = methodInfo.Invoke(null, parameters);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return (string) value;
            }
        }


        public bool Equals(IWebElement obj)
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

        public Rectangle Rectangle
        {
            get
            {
                Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                string methodName = MethodBase.GetCurrentMethod().Name;
                MethodInfo methodInfo = typeof(WebSimulator).GetMethod(methodName);
                Debug.Assert(methodInfo != null);
                var parameters = new object[] {this};
                object value = methodInfo.Invoke(null, parameters);
                Debug.WriteLine("value = " + String.IntroText(value.ToString()));
                Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                return (Rectangle) value;
            }
        }

        #region

        public void ScrollIntoView(bool b)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {b};
            methodInfo.Invoke(ObjectInstance, parameters);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void Focus()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            string methodName = MethodBase.GetCurrentMethod().Name;
            MethodInfo methodInfo = ObjectInstance.GetType().GetMethod(methodName);
            Debug.Assert(methodInfo != null);
            var parameters = new object[] {};
            methodInfo.Invoke(ObjectInstance, parameters);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
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