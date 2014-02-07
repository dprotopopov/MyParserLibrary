using System;
using DevExpress.XtraEditors;
using MyParser.Library;

namespace MyParser.Library.Forms
{
    public partial class ReturnFieldInfosDialog : XtraForm
    {
        public ReturnFieldInfosDialog()
        {
            InitializeComponent();
        }

        public ReturnFieldInfos ReturnFieldInfos { get; set; }

        private void ReturnFieldInfosDialog_Load(object sender, EventArgs e)
        {
            foreach (string key in ReturnFieldInfos.Keys)
            {
                listBoxControlKey.Items.Add(key);
            }
        }

        private void listBoxKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            var key = (string) listBoxControlKey.SelectedItem;
            propertyGridControlReturnFieldInfo.SelectedObject = ReturnFieldInfos[key];
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}