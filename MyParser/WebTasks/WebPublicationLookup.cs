using System;
using System.Windows.Forms;

namespace MyParser.WebTasks
{
    public class WebPublicationLookup : WebTask, IWebPublicationLookup
    {
        public WebPublicationLookup()
        {
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Url);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Email);
            ReturnFieldInfos.Add(Defaults.ReturnFieldInfos.Phone);
            PublicationId = string.Empty;
        }

        public string PublicationId { get; set; }

        public override object ToViewItem(Type type)
        {
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (ListViewItem)))
            {
                var listViewItem = base.ToViewItem(type) as ListViewItem;
                listViewItem.SubItems.Add(PublicationId);
                return listViewItem;
            }
            return base.ToViewItem(type);
        }
    }
}