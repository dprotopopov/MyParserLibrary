using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MyParser.Forms
{
    public partial class ReturnFieldInfosDialog : XtraForm
    {
        public ReturnFieldInfosDialog()
        {
            InitializeComponent();
            Application.Idle += (sender, e) => Thread.Yield();
        }

        public IEnumerable<ReturnFieldInfo> ReturnFieldInfos { private get; set; }

        private void ReturnFieldInfosDialog_Load(object sender, EventArgs e)
        {
            listBoxControlKey.Items.Clear();
            listBoxControlKey.Items.AddRange(
                ReturnFieldInfos.ToList().Select(value => value).Cast<object>().ToArray());
        }

        private void listBoxKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGridControlReturnFieldInfo.SelectedObject =
                listBoxControlKey.SelectedItem;
        }
    }
}