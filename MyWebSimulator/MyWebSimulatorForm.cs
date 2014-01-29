using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WindowsInput.Native;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using HtmlAgilityPack;
using MyParserLibrary;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MyWebSimulator
{
    public partial class MyWebSimulatorForm : RibbonForm
    {
        public MyWebSimulatorForm()
        {
            InitializeComponent();
            WebSimulator = new WebSimulator {WebBrowser = webBrowser};
            foreach (EventInfo item in WebSimulator.ElementEvents.Keys)
            {
                repositoryItemComboBoxElementEventInfo.Items.Add(item);
            }
            foreach (MethodInfo item in WebSimulator.SimulatorMethodInfos.Keys)
            {
                repositoryItemComboBoxSimulatorMethodInfo.Items.Add(item);
            }
            foreach (MethodInfo item in WebSimulator.MouseMethods.Keys)
            {
                repositoryItemComboBoxMouseMethodInfo.Items.Add(item);
            }
            foreach (MethodInfo item in WebSimulator.KeyboardMethods.Keys)
            {
                repositoryItemComboBoxKeyboardMethodInfo.Items.Add(item);
            }
            Workspace = new MyWebSimulatorFormWorkspace();
            propertyGridControlWorkspace.SelectedObject = Workspace;
            propertyGridControlWorkspace.Refresh();
        }

        public MyWebSimulatorFormWorkspace Workspace { get; set; }

        private WebSimulator WebSimulator { get; set; }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                Workspace.Url = WebSimulator.Uri.ToString();

                listBoxElements.Items.Clear();
                listBoxElements.SelectedItem = null;

                listBoxNodes.Items.Clear();
                listBoxNodes.SelectedItem = null;

                WebSimulator.HighlightElement(WebSimulator.HighlightedElement, false, false);
                propertyGridControlHtmlElement.SelectedObject = null;

                propertyGridControlHtmlNode.SelectedObject = null;

                HtmlDocument document = WebSimulator.HtmlDocument;
                foreach (var item in from HtmlNode node in document.DocumentNode.SelectNodes(@"//*")
                    select new KeyValuePair<string, HtmlNode>(node.OuterHtml, node))
                {
                    listBoxNodes.Items.Add(item);
                }
                foreach (var item in from HtmlElement element in WebSimulator.Document.All
                    select new KeyValuePair<string, HtmlElement>(element.OuterHtml, element))
                {
                    listBoxElements.Items.Add(item);
                }
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
        }

        private void listBoxNodes_SelectedChanged(object sender, EventArgs e)
        {
            try
            {
                propertyGridControlHtmlNode.SelectedObject = null;
                listBoxElements.SelectedItem = null;
                WebSimulator.HighlightElement(WebSimulator.HighlightedElement, false, false);
                propertyGridControlHtmlElement.SelectedObject = null;
                propertyGridControlHtmlNode.SelectedObject =
                    ((KeyValuePair<string, HtmlNode>) listBoxNodes.SelectedItem).Value;
                Workspace.Xpath =
                    WebSimulator.XPathSanitize(((HtmlNode) propertyGridControlHtmlNode.SelectedObject).XPath);
                HtmlElement element =
                    WebSimulator.GetElementByNode(new List<HtmlNode>
                    {
                        ((KeyValuePair<string, HtmlNode>) listBoxNodes.SelectedItem).Value
                    })
                        .FirstOrDefault();
                listBoxElements.SelectedItem =
                    listBoxElements.Items.Cast<KeyValuePair<string, HtmlElement>>()
                        .FirstOrDefault(item => item.Value == element);
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
        }

        private void listBoxElements_SelectedChanged(object sender, EventArgs e)
        {
            try
            {
                WebSimulator.HighlightElement(WebSimulator.HighlightedElement, false, false);
                propertyGridControlHtmlElement.SelectedObject = null;
                if (listBoxElements.SelectedItem != null)
                {
                    var item = ((KeyValuePair<string, HtmlElement>) listBoxElements.SelectedItem);
                    propertyGridControlHtmlElement.SelectedObject = item.Value;
                }
                WebSimulator.HighlightElement(
                    WebSimulator.HighlightedElement = (HtmlElement) propertyGridControlHtmlElement.SelectedObject, true,
                    true);
                Workspace.Xpath = WebSimulator.GetXPath((HtmlElement) propertyGridControlHtmlElement.SelectedObject);
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                repositoryItemComboBoxUrl.Items.Add(Workspace.Url);
                WebSimulator.WebBrowser.Navigate(Workspace.Url);
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }

            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                repositoryItemComboBoxXpath.Items.Add(Workspace.Xpath);

                WebSimulator.HighlightElement(WebSimulator.HighlightedElement, false, false);

                listBoxElements.Items.Clear();
                listBoxElements.SelectedItem = null;

                listBoxNodes.Items.Clear();
                listBoxNodes.SelectedItem = null;

                propertyGridControlHtmlNode.SelectedObject = null;
                propertyGridControlHtmlElement.SelectedObject = null;

                HtmlDocument document = WebSimulator.HtmlDocument;
                List<HtmlNode> nodeList = document.DocumentNode.SelectNodes(Workspace.Xpath).ToList();
                List<HtmlElement> elementList = WebSimulator.GetElementByNode(nodeList);
                foreach (var itemNode in nodeList.Select(node => new KeyValuePair<string, HtmlNode>(node.OuterHtml,
                    node)))
                {
                    listBoxNodes.Items.Add(itemNode);
                }
                foreach (
                    var itemElement in
                        elementList.Select(element => new KeyValuePair<string, HtmlElement>(element.OuterHtml, element))
                    )
                {
                    listBoxElements.Items.Add(itemElement);
                }
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
        }

        private void SimulateEvent_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                EventInfo eventInfo = Workspace.ElementEventInfo;
                HtmlElement htmlElement = ((KeyValuePair<string, HtmlElement>) listBoxElements.SelectedItem).Value;
                List<object> parameters;
                if (eventInfo == typeof (HtmlElement).GetEvent("KeyDown") ||
                    eventInfo == typeof (HtmlElement).GetEvent("KeyPress") ||
                    eventInfo == typeof (HtmlElement).GetEvent("KeyUp"))
                    parameters = new List<object> {Workspace.VirtualKeyCode};
                else parameters = new List<object>();
                WebSimulator.SimulateEvent(eventInfo, htmlElement, parameters);
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
        }

        public class MyWebSimulatorFormWorkspace
        {
            public string Url { get; set; }
            public string Xpath { get; set; }
            public EventInfo ElementEventInfo { get; set; }
            public MethodInfo SimulatorMethodInfo { get; set; }
            public MethodInfo MouseMethodInfo { get; set; }
            public MethodInfo KeyboardMethodInfo { get; set; }
            public object LastError { get; set; }
            public VirtualKeyCode VirtualKeyCode { get; set; }
            public string TextEntry { get; set; }
        }

        private void SimulateTextEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                HtmlElement htmlElement = ((KeyValuePair<string, HtmlElement>)listBoxElements.SelectedItem).Value;
                List<object> parameters= new List<object> { Workspace.TextEntry };
                WebSimulator.SimulateTextEntry(htmlElement, parameters);
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
        }
    }
}