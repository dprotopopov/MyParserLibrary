using System;

namespace MyParserLibrary
{
    public partial class WebBrowser : System.Windows.Forms.WebBrowser, IWebBrowser
    {
        public WebBrowser()
        {
            InitializeComponent();
        }

        public new WebDocument Document { get { return new WebDocument(base.Document);} }
    }
}
