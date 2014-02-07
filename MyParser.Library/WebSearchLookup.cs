using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyParser.Library
{
    public class WebSearchLookup : WebTask, IWebSearchLookup
    {
        public WebSearchLookup()
        {
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.PublicationId);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.PublicationDate);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.PublicationLink);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.OtherPageUrl);
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