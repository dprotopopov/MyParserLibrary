using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WindowsInput.Native;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using HtmlAgilityPack;
using MyLibrary;
using MyLibrary.LastError;
using MyLibrary.ManagedObject;
using MyWebSimulator.Managed;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MyWebSimulator.Application
{
    public partial class MyWebSimulatorForm : RibbonForm
    {
        private readonly CefSharp.WinForms.WebView _webView;

        public MyWebSimulatorForm()
        {
            InitializeComponent();
            _webView = new CefSharp.WinForms.WebView(@"http://protopopov.ru")
            {
                Dock = DockStyle.Fill,
                MenuHandler = new MenuHandler(),
            };
            panel1.Controls.Add(_webView);

            WebSimulator = new WebSimulator {WebBrowser = new ManagedWebBrowser(_webView)};
            _webView.LoadCompleted += (sender, url) => WebSimulator.DocumentLoadCompleted(sender, url);
            _webView.LoadCompleted += (sender, url) => DocumentLoadCompleted(sender, url);

            Workspace = new MyWebSimulatorFormWorkspace();

            repositoryItemComboBoxElementEventInfo.Items.AddRange(
                WebSimulator.ElementEvents.Keys.Cast<object>().ToArray());
            repositoryItemComboBoxSimulatorMethodInfo.Items.AddRange(
                WebSimulator.SimulatorMethodInfos.Keys.Cast<object>().ToArray());
            repositoryItemComboBoxMouseMethodInfo.Items.AddRange(
                WebSimulator.MouseMethods.Keys.Cast<object>().ToArray());
            repositoryItemComboBoxKeyboardMethodInfo.Items.AddRange(
                WebSimulator.KeyboardMethods.Keys.Cast<object>().ToArray());
            propertyGridControlWorkspace.SelectedObject = Workspace;
        }

        private MyWebSimulatorFormWorkspace Workspace { get; set; }

        private WebSimulator WebSimulator { get; set; }

        private void RefreshControls()
        {
            propertyGridControlHtmlNode.Refresh();
            propertyGridControlHtmlElement.Refresh();
            propertyGridControlWorkspace.Refresh();
            propertyGridControlWindow.Refresh();
            _webView.Refresh();
        }

        private void ClearControls(int level)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine("level = " + level);
            if (level <= 0)
            {
                listBoxWindows.Items.Clear();
                listBoxWindows.SelectedItem = null;
                WebSimulator.HighlightedElement = null;
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
            WebSimulator.HighlightedElement = null;
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void FillControls(int level)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine("level = " + level);
            if (level == 0)
            {
                listBoxWindows.Items.AddRange(WebSimulator.Windows(WebSimulator.TopmostWindow).Cast<object>().ToArray());
            }
            if (level <= 1)
            {
                HtmlDocument document = WebSimulator.HtmlDocument;
                try
                {
                    listBoxNodes.Items.AddRange(
                        document.DocumentNode.SelectNodes(@"//*")
                            .Select(node => new KeyValuePair<string, HtmlNode>(node.OuterHtml, node))
                            .Cast<object>()
                            .ToArray());
                }
                catch (Exception exception)
                {
                    Workspace.LastError = exception;
                }
                try
                {
                    listBoxElements.Items.AddRange(
                        WebSimulator.WebDocument.All.Select(
                            element => new KeyValuePair<string, IWebElement>(element.OuterHtml, element))
                            .Cast<object>()
                            .ToArray());
                }
                catch (Exception exception)
                {
                    Workspace.LastError = exception;
                }
            }
            if (level == 2)
            {
                HtmlDocument document = WebSimulator.HtmlDocument;
                HtmlNode[] htmlNodes = document.DocumentNode.SelectNodes(Workspace.Xpath).ToArray();
                IEnumerable<IWebElement> webElements = WebSimulator.GetElementByNode(htmlNodes);
                try
                {
                    listBoxNodes.Items.AddRange(
                        htmlNodes.Select(node => new KeyValuePair<string, HtmlNode>(node.OuterHtml, node))
                            .Cast<object>()
                            .ToArray());
                }
                catch (Exception exception)
                {
                    Workspace.LastError = exception;
                }
                try
                {
                    listBoxElements.Items.AddRange(
                        webElements.Select(element => new KeyValuePair<string, IWebElement>(element.OuterHtml, element))
                            .Cast<object>()
                            .ToArray());
                }
                catch (Exception exception)
                {
                    Workspace.LastError = exception;
                }
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void DocumentLoadCompleted(params object[] parameters)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            if (propertyGridControlWorkspace.InvokeRequired
                || propertyGridControlWindow.InvokeRequired
                || propertyGridControlHtmlNode.InvokeRequired
                || propertyGridControlHtmlElement.InvokeRequired
                || listBoxWindows.InvokeRequired
                || listBoxNodes.InvokeRequired
                || listBoxElements.InvokeRequired
                )
            {
                try
                {
                    DocumentLoadCompletedDelegate d = DocumentLoadCompleted;
                    object[] objects = {parameters};
                    Debug.WriteLine("Invoke {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
                    Invoke(d, objects);
                }
                catch (Exception exception)
                {
                    Workspace.LastError = exception;
                }
            }
            else
            {
                try
                {
                    Workspace.Url = WebSimulator.ToString();

                    ClearControls(0);
                    if (WebSimulator.TopmostWindow is IManagedObject)
                        propertyGridControlWindow.SelectedObject =
                            (WebSimulator.TopmostWindow as IManagedObject).ObjectInstance;
                    else
                        propertyGridControlWindow.SelectedObject = WebSimulator.TopmostWindow;
                    FillControls(0);

                    Workspace.LastError = WebSimulator.LastError;
                }
                catch (Exception exception)
                {
                    Workspace.LastError = exception;
                }
                RefreshControls();
            }
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void listBoxNodes_SelectedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                ClearControls(3);
                FillControls(3);
                if (listBoxNodes.SelectedItem != null)
                {
                    var itemNode = (KeyValuePair<string, HtmlNode>) listBoxNodes.SelectedItem;
                    propertyGridControlHtmlNode.SelectedObject = itemNode.Value;
                    Workspace.Xpath = XPath.Sanitize(itemNode.Value.XPath);
                    IWebElement element =
                        WebSimulator.GetElementByNode(new[]
                        {
                            itemNode.Value
                        })
                            .FirstOrDefault();
                    KeyValuePair<string, IWebElement> itemElement = listBoxElements.Items
                        .Cast<KeyValuePair<string, IWebElement>>()
                        .FirstOrDefault(index => index.Value.Equals(element));
                    listBoxElements.SelectedItem = itemElement;
                    if (itemElement.Value is IManagedObject)
                        propertyGridControlHtmlElement.SelectedObject =
                            (itemElement.Value as IManagedObject).ObjectInstance;
                    else
                        propertyGridControlHtmlElement.SelectedObject = itemElement.Value;
                }

                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void listBoxElements_SelectedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                ClearControls(4);
                FillControls(4);
                if (listBoxElements.SelectedItem != null)
                {
                    var itemElement = ((KeyValuePair<string, IWebElement>) listBoxElements.SelectedItem);
                    if (itemElement.Value is IManagedObject)
                        propertyGridControlHtmlElement.SelectedObject =
                            (itemElement.Value as IManagedObject).ObjectInstance;
                    else
                        propertyGridControlHtmlElement.SelectedObject = itemElement.Value;
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void SimulateEvent_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                EventInfo eventInfo = Workspace.ElementEventInfo;
                IWebElement htmlElement = ((KeyValuePair<string, IWebElement>) listBoxElements.SelectedItem).Value;
                List<object> parameters;
                if (eventInfo == typeof (HtmlElement).GetEvent("KeyDown") ||
                    eventInfo == typeof (HtmlElement).GetEvent("KeyPress") ||
                    eventInfo == typeof (HtmlElement).GetEvent("KeyUp"))
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
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void SimulateTextEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                IWebElement htmlElement = ((KeyValuePair<string, IWebElement>) listBoxElements.SelectedItem).Value;
                var parameters = new List<object> {Workspace.TextEntry};
                WebSimulator.SimulateTextEntry(htmlElement, parameters);
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void listBoxWindows_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            try
            {
                ClearControls(1);

                if (listBoxWindows.SelectedItem != null)
                {
                    var itemWindow = (KeyValuePair<IWebWindow, string>) listBoxWindows.SelectedItem;
                    WebSimulator.Window = itemWindow.Key;
                    if (itemWindow.Key is IManagedObject)
                        propertyGridControlWindow.SelectedObject = (itemWindow.Key as IManagedObject).ObjectInstance;
                    else
                        propertyGridControlWindow.SelectedObject = itemWindow.Key;
                    FillControls(1);
                }
                Workspace.LastError = WebSimulator.LastError;
            }
            catch (Exception exception)
            {
                Workspace.LastError = exception;
            }
            RefreshControls();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void MyWebSimulatorForm_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            RefreshControls();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private void MyWebSimulatorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            panel1.Controls.Clear();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private class MyWebSimulatorFormWorkspace : ILastError
        {
            public string Url { get; set; }
            public string Xpath { get; set; }
            public EventInfo ElementEventInfo { get; set; }
            public MethodInfo SimulatorMethodInfo { get; set; }
            public MethodInfo MouseMethodInfo { get; set; }
            public MethodInfo KeyboardMethodInfo { get; set; }
            public VirtualKeyCode VirtualKeyCode { get; set; }
            public string TextEntry { get; set; }
            public IWebWindow Window { get; set; }
            public object LastError { get; set; }
        }
    }
}