using System;
using System.Windows.Forms;
using CefSharp.Example;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;

namespace WebView.Evaluate
{
    public partial class WebViewEvaluate : RibbonForm
    {
        private readonly CefSharp.WinForms.WebView _webView;

        public WebViewEvaluate()
        {
            InitializeComponent();
            _webView = new CefSharp.WinForms.WebView(ExamplePresenter.DefaultUrl)
            {
                Dock = DockStyle.Fill,MenuHandler = new MenuHandler()
            };
            panel1.Controls.Add(_webView);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                textEditResult.Text = _webView.EvaluateScript(textEditScript.Text).ToString();
            }
            catch (Exception exception)
            {
                textEditResult.Text = exception.ToString();
            }
        }

        private void WebViewEvaluate_FormClosing(object sender, FormClosingEventArgs e)
        {
            panel1.Controls.Clear();
        }
    }
}