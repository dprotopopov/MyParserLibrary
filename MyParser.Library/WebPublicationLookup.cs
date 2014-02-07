using System.Diagnostics;
using System.Windows.Forms;

namespace MyParser.Library
{
    public class WebPublicationLookup : WebTask, IWebPublicationLookup
    {
        public WebPublicationLookup()
        {
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Url);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Email);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Phone);
            PublicationId = @"";
        }

        public string PublicationId { get; set; }

        public override ListViewItem ToListViewItem()
        {
            ListViewItem viewItem = base.ToListViewItem();
            Debug.Assert(viewItem != null, "viewItem != null");
            viewItem.SubItems.Add(PublicationId);
            return viewItem;
        }
    }
}