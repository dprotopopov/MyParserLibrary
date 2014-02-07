using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using MyParser.Library;

namespace MyParser.WebView
{
    public class WebViewImpl
    {
        protected const string ElementPattern = @"\/(?<tag>\w+)\[(?<index>\d+)\]";
        protected const string FramePattern = @"\/(frame|iframe)\[\d+\]";

        #region

        protected const string FocusMethod = @"focus";
        protected const string GetAttributeMethod = @"getAttribute";
        protected const string SetAttributeMethod = @"setAttribute";
        protected const string FireEventMethod = @"fireEvent";

        protected const string FocusMethodParameters = @"";
        protected const string GetAttributeMethodParameters = @"'{{attribute}}'";
        protected const string SetAttributeMethodParameters = @"'{{attribute}}','{{value}}'";
        protected const string FireEventMethodParameters = @"'{{event}}'";

        #endregion

        #region

        protected const string OuterHtmlProperty = @"outerHTML";
        protected const string OffsetLeftProperty = @"offsetLeft";
        protected const string OffsetTopProperty = @"offsetTop";
        protected const string OffsetWidthProperty = @"offsetWidth";
        protected const string OffsetHeightProperty = @"offsetHeight";
        protected const string OffsetParentProperty = @"offsetParent";

        #endregion

        #region

        protected const string StyleAttribute = @"style";

        #endregion

        #region

        protected const string OnclickEvent = @"onclick";
        protected const string OndblclickEvent = @"ondblclick";
        protected const string OnmousedownEvent = @"onmousedown";
        protected const string OnmousemoveEvent = @"onmousemove";
        protected const string OnmouseoutEvent = @"onmouseout";
        protected const string OnmouseoverEvent = @"onmouseover";
        protected const string OnmouseupEvent = @"onmouseup";
        protected const string OnmousewheeltEvent = @"onmousewheel";
        protected const string OnscrollEvent = @"onscroll";
        protected const string OnkeydownEvent = @"onkeydown";
        protected const string OnkeypressEvent = @"onkeypress";
        protected const string OnkeyupEvent = @"onkeydown";
        protected const string OnchangeEvent = @"onchange";
        protected const string OnfocusEvent = @"onfocus";
        protected const string OnselectEvent = @"onselect";
        protected const string OnsubmitEvent = @"onsubmit";
        protected const string OnblurEvent = @"onblur";

        #endregion

        #region

        protected const string ArrayPrototypeLast = @"
            if(!Array.prototype.last) {
                Array.prototype.last = function() {
                    return this[this.length - 1];
                }
            }
        ";
        protected const string FunctionGetElementByXPath = @"
            function getElementByXPath(
                a, // the XPath
                b  // document-placeholder
              ) {
                return b.evaluate(
                  a,    // xpathExpression
                  b,    // contextNode
                  null, // namespaceResolver
                  9,    // resultType
                  null  // result
                ).singleNodeValue; // the first node
            }
        ";
        protected const string FunctionGetXPath = @"
            function getXPath( element )
            {
                var xpath = '';
                for ( ; element && element.nodeType == 1; element = element.parentNode )
                {
                    var id = $(element.parentNode).children(element.tagName).index(element) + 1;
                    xpath = '/' + element.tagName.toLowerCase() + '[' + id + ']' + xpath;
                }
                return xpath;
            }
        ";
        protected const string ElementFireKeyboardEvent = @"{
            var keyboardEvent = document.createEvent('KeyboardEvent');
            var initMethod = typeof keyboardEvent.initKeyboardEvent !== 'undefined' ? 'initKeyboardEvent' : 'initKeyEvent';

            keyboardEvent[initMethod](
                               '{{eventtype}}', // event type : keydown, keyup, keypress
                                true, // bubbles
                                true, // cancelable
                                window, // viewArg: should be window
                                false, // ctrlKeyArg
                                false, // altKeyArg
                                false, // shiftKeyArg
                                false, // metaKeyArg
                                {{code}}, // keyCodeArg : unsigned long the virtual key code, else 0
                                0 // charCodeArgs : unsigned long the Unicode character associated with the depressed key, else 0
            );

            queue.shift().dispatchEvent(keyboardEvent);
        }";
        protected const string ElementFireMouseEvent = @"{
            var mouseEvent = document.createEvent('MouseEvents');
            var initMethod = 'initMouseEvent';

            mouseEvent[initMethod](
                        '{{eventtype}}',
                                    // the string to set the event's type to. 
                                    // Possible types for mouse events include: 
                                    // click, mousedown, mouseup, mouseover, mousemove, mouseout.
                        true,
                                    // whether or not the event can bubble. Sets the value of event.bubbles.
                        true,
                                    // whether or not the event's default action can be prevented. Sets the value of event.cancelable.
                        window,
                                    // the Event's AbstractView. You should pass the window object here. Sets the value of event.view.
                        0,
                                    // the Event's mouse click count. Sets the value of event.detail.
                        0,
                                    // the Event's screen x coordinate. Sets the value of event.screenX.
                        0,
                                    // the Event's screen y coordinate. Sets the value of event.screenY.
                        0,
                                    // the Event's client x coordinate. Sets the value of event.clientX.
                        0,
                                    // the Event's client y coordinate. Sets the value of event.clientY.
                        false,
                                    // whether or not control key was depressed during the Event. Sets the value of event.ctrlKey.
                        false,
                                    // whether or not alt key was depressed during the Event. Sets the value of event.altKey.
                        false,
                                    // whether or not shift key was depressed during the Event. Sets the value of event.shiftKey.
                        false,
                                    // whether or not meta key was depressed during the Event. Sets the value of event.metaKey.
                        {{button}},
                                    // the Event's mouse event.button.
                        null
                                    // the Event's related EventTarget. Only used with some event types (e.g. mouseover and mouseout). In other cases, pass null.
            );

            queue.shift().dispatchEvent(mouseEvent);
        }";

        protected const string DeclareVariables = @"
            var queue = [ ];
        ";
        protected const string PushDocument = @"
             queue.push ( document ) ;
        ";
        protected const string GetFrameByXPath = @"
            queue.push ( getElementByXPath('{{xpath}}', queue.shift() ).contentWindow.document ) ;
        ";
        protected const string GetElementByXPath = @"
            queue.push ( getElementByXPath('{{xpath}}', queue.shift() ) ) ;
        ";
        protected const string ElementInvokeMethod = @"
            queue.push( queue.shift().{{method}} ({{parameterTemplates}}) ) ;
        ";
        protected const string GetElementProperty = @"
            queue.push( queue.shift().{{property}} ) ;
        ";
        protected const string SetElementProperty = @"
            queue.shift().{{property}} = '{{value}}' ;
        ";
        protected const string SetElementAttribute = @"
            queue.shift().setAttribute('{{attribute}}','{{value}}') ;
        ";
        protected const string GetElementAttribute = @"
            queue.push( queue.shift().getAttribute('{{attribute}}') ) ;
        ";
        protected const string GetElementsByTagName = @"
            queue.shift().getElementsByTagName('{{tag}}').forEach(function(e) {
               queue.push( e );
            }); ;
        ";
        protected const string GetElementChildNodes = @"
            queue.shift().childNodes.forEach(function(e) {
               queue.push( e );
            }); ;
        ";
        protected const string ConvertToXPaths = @"
            for (var i = 0 ; i < queue.length ; i++ ) {
               queue[i] = getXPath( queue[i] ) ;
            };
        ";
        protected const string CloneQueueLast = @"
            queue.push ( queue.last() ) ;
        ";
        protected const string ReturnQueueJoin = @"
            return queue.join(',') ;
        ";

        #endregion

        #region

        protected const string KeydownEventtype = @"keydown";
        protected const string KeyupEventtype = @"keyup";
        protected const string KeypressEventtype = @"keypress";

        protected const string ClickEventtype = @"click";
        protected const string MousedownEventtype = @"mousedown";
        protected const string MouseupEventtype = @"mouseup";
        protected const string MouseoverEventtype = @"mouseover";
        protected const string MousemoveEventtype = @"mousemove";
        protected const string MouseoutEventtype = @"mouseout";

        #endregion

        #region

        protected const string XpathArgument = @"\{\{xpath\}\}";
        protected const string EventArgument = @"\{\{event\}\}";
        protected const string TagArgument = @"\{\{tag\}\}";
        protected const string PropertyArgument = @"\{\{property\}\}";
        protected const string ValueArgument = @"\{\{value\}\}";
        protected const string AttributeArgument = @"\{\{attribute\}\}";
        protected const string EventtypeArgument = @"\{\{eventtype\}\}";
        protected const string CodeArgument = @"\{\{code\}\}";
        protected const string MethodArgument = @"\{\{method\}\}";
        protected const string ParametersArgument = @"\{\{parameterTemplates\}\}";
        protected const string ButtonArgument = @"\{\{button\}\}";

        #endregion

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

        protected object EvaluateInvokeScript(string command, List<string> parameterTemplates,
            ParametersValues parentParametersValues)
        {
            var parametersValues = new ParametersValues();
            foreach (string template in parameterTemplates)
            {
                parametersValues[ParametersArgument].Add(MyLibrary.ParseRowTemplate(template,
                    parentParametersValues));
            }
            return EvaluateScript(command, parametersValues);
        }

        protected object EvaluateScript(string command, ParametersValues parentParametersValues)
        {
            var parametersValues = new ParametersValues(parentParametersValues);
            var sb = new StringBuilder();
            sb.Append(ArrayPrototypeLast);
            sb.Append(FunctionGetElementByXPath);
            sb.Append(FunctionGetXPath);
            sb.Append(DeclareVariables);
            int last = 0;
            sb.Append(PushDocument);
            foreach (Match match in Regex.Matches(XPath, FramePattern))
            {
                string xpath = XPath.Substring(last, match.Index + match.Length - last);
                parametersValues.Add(XpathArgument, xpath);
                sb.Append(GetFrameByXPath);
                last = match.Index + match.Length;
            }
            {
                string xpath = XPath.Substring(last);
                parametersValues.Add(XpathArgument, xpath);
                sb.Append(GetElementByXPath);
            }
            sb.Append(command);
            string script = MyLibrary.ParseRowTemplate(sb.ToString(), parametersValues);
            Debug.WriteLine(script);
            return WebView.EvaluateScript(script);
        }
    }
}