using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyParserLibrary
{
    public class WebSearchLookup : WebTask, IWebSearchLookup
    {
        public WebSearchLookup()
        {
            ReturnFieldInfos.Add(Defaults.DefaultOtherPageUrlInfo);
            PageId = 0;
        }

        public int PageId { get; set; }

        public override ListViewItem ToListViewItem()
        {
            ListViewItem viewItem = base.ToListViewItem();
            Debug.Assert(viewItem != null, "viewItem != null");
            viewItem.SubItems.Add(Convert.ToString(PageId));
            return viewItem;
        }
    }
}