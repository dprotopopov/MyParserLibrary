using System;
using System.Windows.Forms;
using MyLibrary.ManagedObject;

namespace MyWebSimulator
{
    public interface IWebBrowser : IManagedObject
    {
        IWebDocument Document { get; }
        Uri Url { get; set; }
        IntPtr Handle { get; }
        void Navigate(string url);
        Form FindForm();
        bool Focus();
    }
}