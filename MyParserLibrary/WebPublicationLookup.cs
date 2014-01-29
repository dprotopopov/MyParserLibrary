using System.Diagnostics;
using System.Windows.Forms;

namespace MyParserLibrary
{
    public class WebPublicationLookup : WebTask, IWebPublicationLookup
    {
        public WebPublicationLookup()
        {
            ReturnFieldInfos.Add(Defaults.DefaultUrlInfo);
            ReturnFieldInfos.Add(Defaults.DefaultEmailInfo);
            ReturnFieldInfos.Add(Defaults.DefaultPhoneInfo);
            ReturnFieldInfos.Add(Defaults.DefaultPublicationIdInfo);
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
