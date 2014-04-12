namespace MyAdvertisement.AdSense
{
    public class MenuHandler : CefSharp.IMenuHandler
    {
        public bool OnBeforeContextMenu(CefSharp.IWebBrowser browser)
        {
            // Return false if you want to disable the context menu.
            return true;
        }
    }
}