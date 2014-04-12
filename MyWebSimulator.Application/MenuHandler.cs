using CefSharp;

namespace MyWebSimulator.Application
{
    internal class MenuHandler : IMenuHandler
    {
        public bool OnBeforeContextMenu(CefSharp.IWebBrowser browser)
        {
            // Return false if you want to disable the context menu.
            return true;
        }
    }
}