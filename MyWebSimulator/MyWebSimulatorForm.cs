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
            RefreshControls();
        }

        public MyWebSimulatorFormWorkspace Workspace { get; set; }

        private WebSimulator WebSimulator { get; set; }

        private void RefreshControls()
        {
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
            propertyGridControlWindow.Refresh();
        }

        private void ClearControls(int level)
        {
            if (level <= 0)
            {
                listBoxWindows.Items.Clear();
                listBoxWindows.SelectedItem = null;
            }
            if (level <= 1)
            {
                propertyGridControlWindow.SelectedObject = null;
            }
            if (level <= 2)
            {
                listBoxNodes.Items.Clear();
                listBoxNodes.SelectedItem = null;
                listBoxElements.Items.Clear();
                listBoxElements.SelectedItem = null;
            }
            if (level <= 3)
            {
                propertyGridControlHtmlNode.SelectedObject = null;
            }
            if (level <= 4)
            {
                propertyGridControlHtmlElement.SelectedObject = null;
                WebSimulator.HighlightElement(WebSimulator.HighlightedElement, false, false);
            }
        }

        private void FillControls(int level)
        {
            if (level == 0)
            {
                foreach (var item in WebSimulator.Windows(WebSimulator.TopmostWindow))
                {
                    listBoxWindows.Items.Add(item);
                }
            }
            if (level <= 1)
            {
                HtmlDocument document = WebSimulator.HtmlDocument;
                foreach (var index in from HtmlNode node in document.DocumentNode.SelectNodes(@"//*")
                    select new KeyValuePair<string, HtmlNode>(node.OuterHtml, node))
                {
                    listBoxNodes.Items.Add(index);
                }
                foreach (var index in from WebElement element in WebSimulator.WebDocument.All
                    select new KeyValuePair<string, WebElement>(element.OuterHtml, element))
                {
                    listBoxElements.Items.Add(index);
                }
            }
            if (level == 2)
            {
                HtmlDocument document = WebSimulator.HtmlDocument;
                List<HtmlNode> nodeList = document.DocumentNode.SelectNodes(Workspace.Xpath).ToList();
                List<WebElement> elementList = WebSimulator.GetElementByNode(nodeList);
                foreach (var itemNode in nodeList.Select(node => new KeyValuePair<string, HtmlNode>(node.OuterHtml,
                    node)))
                {
                    listBoxNodes.Items.Add(itemNode);
                }
                foreach (
                    var itemElement in
                        elementList.Select(element => new KeyValuePair<string, WebElement>(element.OuterHtml, element))
                    )
                {
                    listBoxElements.Items.Add(itemElement);
                }
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                Workspace.Url = WebSimulator.ToString();

                ClearControls(0);
                WebSimulator.Window = WebSimulator.TopmostWindow;
                propertyGridControlWindow.SelectedObject = WebSimulator.TopmostWindow.ManagedObject;
                FillControls(0);

                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
        }

        private void listBoxNodes_SelectedChanged(object sender, EventArgs e)
        {
            try
            {
                ClearControls(3);
                FillControls(3);
                if (listBoxNodes.SelectedItem != null)
                {
                    var itemNode = (KeyValuePair<string, HtmlNode>) listBoxNodes.SelectedItem;
                    propertyGridControlHtmlNode.SelectedObject = itemNode.Value;
                    Workspace.Xpath = WebSimulator.XPathSanitize(itemNode.Value.XPath);
                    WebElement element =
                        WebSimulator.GetElementByNode(new List<HtmlNode>
                        {
                            itemNode.Value
                        })
                            .FirstOrDefault();
                    KeyValuePair<string, WebElement> itemElement = listBoxElements.Items
                        .Cast<KeyValuePair<string, WebElement>>()
                        .FirstOrDefault(index => index.Value.Equals(element));
                    listBoxElements.SelectedItem = itemElement;
                    propertyGridControlHtmlElement.SelectedObject = itemElement.Value.ManagedObject;
                }

                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
        }

        private void listBoxElements_SelectedChanged(object sender, EventArgs e)
        {
            try
            {
                ClearControls(4);
                FillControls(4);
                if (listBoxElements.SelectedItem != null)
                {
                    var itemElement = ((KeyValuePair<string, WebElement>) listBoxElements.SelectedItem);
                    propertyGridControlHtmlElement.SelectedObject = itemElement.Value.ManagedObject;
                    WebSimulator.HighlightElement(WebSimulator.HighlightedElement = itemElement.Value, true, true);
                    Workspace.Xpath = itemElement.Value.XPath;
                }
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                repositoryItemComboBoxUrl.Items.Add(Workspace.Url);
                WebSimulator.WebBrowser.Navigate(Workspace.Url);
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ClearControls(2);
                FillControls(2);
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
        }

        private void SimulateEvent_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                EventInfo eventInfo = Workspace.ElementEventInfo;
                WebElement htmlElement = ((KeyValuePair<string, WebElement>) listBoxElements.SelectedItem).Value;
                List<object> parameters;
                if (eventInfo == typeof (WebElement).GetEvent("KeyDown") ||
                    eventInfo == typeof (WebElement).GetEvent("KeyPress") ||
                    eventInfo == typeof (WebElement).GetEvent("KeyUp"))
                    parameters = new List<object> {Workspace.VirtualKeyCode};
                else parameters = new List<object>();
                WebSimulator.SimulateEvent(eventInfo, htmlElement, parameters);
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
        }

        private void SimulateTextEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                WebElement htmlElement = ((KeyValuePair<string, WebElement>) listBoxElements.SelectedItem).Value;
                var parameters = new List<object> {Workspace.TextEntry};
                WebSimulator.SimulateTextEntry(htmlElement, parameters);
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
        }

        private void listBoxWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearControls(1);

                if (listBoxWindows.SelectedItem != null)
                {
                    var itemWindow = (KeyValuePair<WebWindow, string>) listBoxWindows.SelectedItem;
                    WebSimulator.Window = itemWindow.Key;
                    propertyGridControlWindow.SelectedObject = itemWindow.Key.ManagedObject;
                    FillControls(1);
                }
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
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
            public WebWindow Window { get; set; }
        }
    }
}