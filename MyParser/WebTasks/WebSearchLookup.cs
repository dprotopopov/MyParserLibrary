using System;
using System.Windows.Forms;

namespace MyParser.WebTasks
{
    public class WebSearchLookup : WebTask, IWebSearchLookup
    {
        public WebSearchLookup()
        {
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.PublicationId);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.PublicationDatetime);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.PublicationLink);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.OtherPageUrl);
            PageId = 0;
        }

        public int PageId { get; set; }

        public override object ToViewItem(Type type)
        {
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (ListViewItem)))
            {
                var viewItem = base.ToViewItem(type) as ListViewItem;
                viewItem.SubItems.Add(Convert.ToString(PageId));
                return viewItem;
            }
            return base.ToViewItem(type);
        }
    }
}