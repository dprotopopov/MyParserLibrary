using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MyLibrary;

namespace MyWebSimulator.WebView
{
    public class WebViewImpl
    {
        private static readonly Transformation Transformation =
            new Transformation {FieldPattern = JavaScript.FieldPattern};

        public CefSharp.WinForms.WebView WebView { get; set; }
        public string XPath { get; set; }

        public bool IsNullOrEmpty()
        {
            return WebView == null && XPath == null;
        }

        public bool Equals(WebViewImpl obj)
        {
            return Equals(WebView, obj.WebView) &&
                   String.Compare(XPath, obj.XPath, StringComparison.OrdinalIgnoreCase) == 0;
        }

        protected object EvaluateInvokeScript(Values parentDictionary)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var values = new Values
            {
                {JavaScript.MethodArgument, parentDictionary[JavaScript.MethodArgument]},
            };
            foreach (string template in parentDictionary[JavaScript.ParametersArgument])
            {
                values.Add(JavaScript.ParametersArgument, ParseRowTemplate(template,
                    parentDictionary));
            }
            object value = EvaluateScript(JavaScript.ElementInvokeMethod, values);
            Debug.WriteLine("value = " + MyLibrary.Types.String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        protected object EvaluateScript(string command, Values parentDictionary)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine("XPath = " + XPath);
            var values = new Values();
            var sb = new StringBuilder();
            sb.Append(JavaScript.ArrayPrototypeLast);
            sb.Append(JavaScript.ArrayPrototypeForEach);
            sb.Append(JavaScript.FunctionGetElementByXPath);
            sb.Append(JavaScript.FunctionGetXPath);
            sb.Append(JavaScript.DeclareVariables);
            int last = 0;
            sb.Append(JavaScript.PushDocument);
            foreach (Match match in Regex.Matches(XPath, MyLibrary.XPath.FramePattern))
            {
                string xpath = XPath.Substring(last, match.Index + match.Length - last);
                if (!String.IsNullOrEmpty(xpath))
                {
                    values.Add(JavaScript.XpathArgument, xpath);
                    sb.Append(JavaScript.GetFrameByXPath);
                }
                last = match.Index + match.Length;
            }
            {
                string xpath = XPath.Substring(last);
                if (!String.IsNullOrEmpty(xpath))
                {
                    values.Add(JavaScript.XpathArgument, xpath);
                    sb.Append(JavaScript.GetElementByXPath);
                }
            }
            sb.Append(command);
            sb.Append(JavaScript.EvaluateQueueJoin);
            values.Add(parentDictionary);
            string script = JavaScript.Compress(ParseRowTemplate(sb.ToString(), values));
            Debug.WriteLine("script = " + MyLibrary.Types.String.IntroText(script));
            object value = WebView.EvaluateScript(script);
            Debug.WriteLine("value = " + MyLibrary.Types.String.IntroText(value.ToString()));
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return value;
        }

        private string ParseRowTemplate(string template, Values dictionary)
        {
            Debug.Assert(
                Transformation != null && String.Compare(Transformation.FieldPattern,
                    JavaScript.FieldPattern,
                    StringComparison.Ordinal) ==
                0);
            Debug.Assert(!String.IsNullOrEmpty(template));
            Debug.Assert(dictionary != null);
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            string methodName = MethodBase.GetCurrentMethod().Name;
            Type[] types = {typeof (string), typeof (Values)};
            MethodInfo methodInfo = Transformation.GetType().GetMethod(methodName, types);
            Debug.Assert(methodInfo != null);
            object[] objects = {template, dictionary};
            object value = methodInfo.Invoke(Transformation, objects);
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return (string) value;
        }
    }
}