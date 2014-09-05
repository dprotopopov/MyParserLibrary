using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;

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
            gridControl1.DataSource = new BindingList<ReturnFieldInfo>(ReturnFieldInfos.ToList());
        }

        private void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            propertyGridControlReturnFieldInfo.SelectedObject =
                gridView1.GetFocusedRow();
        }
    }
}