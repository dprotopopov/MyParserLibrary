namespace MyParser.Forms
{
    partial class ReturnFieldInfosDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.formAssistant1 = new DevExpress.XtraBars.FormAssistant();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listBoxControlKey = new DevExpress.XtraEditors.ListBoxControl();
            this.propertyGridControlReturnFieldInfo = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.simpleButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlKey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlReturnFieldInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.simpleButtonCancel);
            this.splitContainer1.Size = new System.Drawing.Size(652, 382);
            this.splitContainer1.SplitterDistance = 329;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBoxControlKey);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGridControlReturnFieldInfo);
            this.splitContainer2.Size = new System.Drawing.Size(652, 329);
            this.splitContainer2.SplitterDistance = 267;
            this.splitContainer2.TabIndex = 0;
            // 
            // listBoxControlKey
            // 
            this.listBoxControlKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxControlKey.Location = new System.Drawing.Point(0, 0);
            this.listBoxControlKey.Name = "listBoxControlKey";
            this.listBoxControlKey.Size = new System.Drawing.Size(267, 329);
            this.listBoxControlKey.TabIndex = 0;
            this.listBoxControlKey.SelectedIndexChanged += new System.EventHandler(this.listBoxKey_SelectedIndexChanged);
            // 
            // propertyGridControlReturnFieldInfo
            // 
            this.propertyGridControlReturnFieldInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControlReturnFieldInfo.Location = new System.Drawing.Point(0, 0);
            this.propertyGridControlReturnFieldInfo.Name = "propertyGridControlReturnFieldInfo";
            this.propertyGridControlReturnFieldInfo.Size = new System.Drawing.Size(381, 329);
            this.propertyGridControlReturnFieldInfo.TabIndex = 0;
            // 
            // simpleButtonCancel
            // 
            this.simpleButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.simpleButtonCancel.Location = new System.Drawing.Point(554, 11);
            this.simpleButtonCancel.Name = "simpleButtonCancel";
            this.simpleButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.simpleButtonCancel.TabIndex = 1;
            this.simpleButtonCancel.Text = "Cancel";
            // 
            // ReturnFieldInfosDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 382);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ReturnFieldInfosDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ReturnFieldInfosDialog";
            this.Load += new System.EventHandler(this.ReturnFieldInfosDialog_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControlKey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlReturnFieldInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.FormAssistant formAssistant1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private DevExpress.XtraVerticalGrid.PropertyGridControl propertyGridControlReturnFieldInfo;
        private DevExpress.XtraEditors.SimpleButton simpleButtonCancel;
        private DevExpress.XtraEditors.ListBoxControl listBoxControlKey;
    }
}