using System;
using System.Windows.Forms;

namespace MyParserLibrary
{
    public interface IWebBrowser
    {
        WebDocument Document { get; }
        Uri Url { get; set; }
        IntPtr Handle { get; }
        void Navigate(string url);
        Form FindForm();
        bool Focus();
    }
}