using WebBrowser = MyParserLibrary.WebBrowser;
namespace MyWebSimulator
{
    partial class MyWebSimulatorForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.propertyGridControlWorkspace = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.repositoryItemComboBoxUrl = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBoxElementEventInfo = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBoxXpath = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBoxSimulatorMethodInfo = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBoxMouseMethodInfo = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBoxKeyboardMethodInfo = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.Url = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.Xpath = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ElementEventInfo = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.LastError = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.SimulatorMethodInfo = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.MouseMethodInfo = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.KeyboardMethodInfo = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.listBoxWindows = new System.Windows.Forms.ListBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.listBoxNodes = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.webBrowser = new MyParserLibrary.WebBrowser();
            this.listBoxElements = new System.Windows.Forms.ListBox();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.propertyGridControlHtmlNode = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.propertyGridControlWindow = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.propertyGridControlHtmlElement = new DevExpress.XtraVerticalGrid.PropertyGridControl();
            this.formAssistant1 = new DevExpress.XtraBars.FormAssistant();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.TextEntry = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlWorkspace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxUrl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxElementEventInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxXpath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSimulatorMethodInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxMouseMethodInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxKeyboardMethodInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlHtmlNode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlWindow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlHtmlElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 158);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer7);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer1.Size = new System.Drawing.Size(1156, 495);
            this.splitContainer1.SplitterDistance = 227;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.propertyGridControlWorkspace);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.listBoxWindows);
            this.splitContainer7.Size = new System.Drawing.Size(1156, 227);
            this.splitContainer7.SplitterDistance = 169;
            this.splitContainer7.TabIndex = 0;
            // 
            // propertyGridControlWorkspace
            // 
            this.propertyGridControlWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControlWorkspace.Location = new System.Drawing.Point(0, 0);
            this.propertyGridControlWorkspace.Name = "propertyGridControlWorkspace";
            this.propertyGridControlWorkspace.RecordWidth = 166;
            this.propertyGridControlWorkspace.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxUrl,
            this.repositoryItemComboBoxElementEventInfo,
            this.repositoryItemComboBoxXpath,
            this.repositoryItemComboBoxSimulatorMethodInfo,
            this.repositoryItemComboBoxMouseMethodInfo,
            this.repositoryItemComboBoxKeyboardMethodInfo});
            this.propertyGridControlWorkspace.RowHeaderWidth = 34;
            this.propertyGridControlWorkspace.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.Url,
            this.Xpath,
            this.ElementEventInfo,
            this.LastError,
            this.SimulatorMethodInfo,
            this.MouseMethodInfo,
            this.KeyboardMethodInfo,
            this.TextEntry});
            this.propertyGridControlWorkspace.Size = new System.Drawing.Size(1156, 169);
            this.propertyGridControlWorkspace.TabIndex = 1;
            // 
            // repositoryItemComboBoxUrl
            // 
            this.repositoryItemComboBoxUrl.AutoHeight = false;
            this.repositoryItemComboBoxUrl.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxUrl.Items.AddRange(new object[] {
            "http://rbc.ru",
            "http://mail.ru",
            "http://protopopov.ru",
            "http://protopopov.ru/myemailextractor/advert.php"});
            this.repositoryItemComboBoxUrl.Name = "repositoryItemComboBoxUrl";
            this.repositoryItemComboBoxUrl.Tag = "<Null>";
            // 
            // repositoryItemComboBoxElementEventInfo
            // 
            this.repositoryItemComboBoxElementEventInfo.AutoHeight = false;
            this.repositoryItemComboBoxElementEventInfo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxElementEventInfo.Name = "repositoryItemComboBoxElementEventInfo";
            this.repositoryItemComboBoxElementEventInfo.Sorted = true;
            this.repositoryItemComboBoxElementEventInfo.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemComboBoxXpath
            // 
            this.repositoryItemComboBoxXpath.AutoHeight = false;
            this.repositoryItemComboBoxXpath.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxXpath.Name = "repositoryItemComboBoxXpath";
            // 
            // repositoryItemComboBoxSimulatorMethodInfo
            // 
            this.repositoryItemComboBoxSimulatorMethodInfo.AutoHeight = false;
            this.repositoryItemComboBoxSimulatorMethodInfo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxSimulatorMethodInfo.Name = "repositoryItemComboBoxSimulatorMethodInfo";
            // 
            // repositoryItemComboBoxMouseMethodInfo
            // 
            this.repositoryItemComboBoxMouseMethodInfo.AutoHeight = false;
            this.repositoryItemComboBoxMouseMethodInfo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxMouseMethodInfo.Name = "repositoryItemComboBoxMouseMethodInfo";
            // 
            // repositoryItemComboBoxKeyboardMethodInfo
            // 
            this.repositoryItemComboBoxKeyboardMethodInfo.AutoHeight = false;
            this.repositoryItemComboBoxKeyboardMethodInfo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxKeyboardMethodInfo.Name = "repositoryItemComboBoxKeyboardMethodInfo";
            // 
            // Url
            // 
            this.Url.Name = "Url";
            this.Url.Properties.Caption = "Url";
            this.Url.Properties.FieldName = "Url";
            this.Url.Properties.RowEdit = this.repositoryItemComboBoxUrl;
            // 
            // Xpath
            // 
            this.Xpath.Name = "Xpath";
            this.Xpath.Properties.Caption = "Xpath";
            this.Xpath.Properties.FieldName = "Xpath";
            this.Xpath.Properties.RowEdit = this.repositoryItemComboBoxXpath;
            // 
            // ElementEventInfo
            // 
            this.ElementEventInfo.Name = "ElementEventInfo";
            this.ElementEventInfo.Properties.Caption = "ElementEventInfo";
            this.ElementEventInfo.Properties.FieldName = "ElementEventInfo";
            this.ElementEventInfo.Properties.RowEdit = this.repositoryItemComboBoxElementEventInfo;
            // 
            // LastError
            // 
            this.LastError.Name = "LastError";
            this.LastError.Properties.Caption = "LastError";
            this.LastError.Properties.FieldName = "LastError";
            this.LastError.Properties.ReadOnly = true;
            // 
            // SimulatorMethodInfo
            // 
            this.SimulatorMethodInfo.Name = "SimulatorMethodInfo";
            this.SimulatorMethodInfo.Properties.Caption = "SimulatorMethodInfo";
            this.SimulatorMethodInfo.Properties.FieldName = "SimulatorMethodInfo";
            this.SimulatorMethodInfo.Properties.RowEdit = this.repositoryItemComboBoxSimulatorMethodInfo;
            // 
            // MouseMethodInfo
            // 
            this.MouseMethodInfo.Name = "MouseMethodInfo";
            this.MouseMethodInfo.Properties.Caption = "MouseMethodInfo";
            this.MouseMethodInfo.Properties.FieldName = "MouseMethodInfo";
            this.MouseMethodInfo.Properties.RowEdit = this.repositoryItemComboBoxMouseMethodInfo;
            // 
            // KeyboardMethodInfo
            // 
            this.KeyboardMethodInfo.Name = "KeyboardMethodInfo";
            this.KeyboardMethodInfo.Properties.Caption = "KeyboardMethodInfo";
            this.KeyboardMethodInfo.Properties.FieldName = "KeyboardMethodInfo";
            this.KeyboardMethodInfo.Properties.RowEdit = this.repositoryItemComboBoxKeyboardMethodInfo;
            // 
            // listBoxWindows
            // 
            this.listBoxWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxWindows.FormattingEnabled = true;
            this.listBoxWindows.ItemHeight = 16;
            this.listBoxWindows.Location = new System.Drawing.Point(0, 0);
            this.listBoxWindows.Name = "listBoxWindows";
            this.listBoxWindows.Size = new System.Drawing.Size(1156, 54);
            this.listBoxWindows.TabIndex = 3;
            this.listBoxWindows.SelectedIndexChanged += new System.EventHandler(this.listBoxWindows_SelectedIndexChanged);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer4.Size = new System.Drawing.Size(1156, 264);
            this.splitContainer4.SplitterDistance = 181;
            this.splitContainer4.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.listBoxNodes);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer6.Size = new System.Drawing.Size(1156, 181);
            this.splitContainer6.SplitterDistance = 292;
            this.splitContainer6.TabIndex = 0;
            // 
            // listBoxNodes
            // 
            this.listBoxNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxNodes.FormattingEnabled = true;
            this.listBoxNodes.ItemHeight = 16;
            this.listBoxNodes.Location = new System.Drawing.Point(0, 0);
            this.listBoxNodes.Name = "listBoxNodes";
            this.listBoxNodes.Size = new System.Drawing.Size(292, 181);
            this.listBoxNodes.TabIndex = 2;
            this.listBoxNodes.SelectedIndexChanged += new System.EventHandler(this.listBoxNodes_SelectedChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.webBrowser);
            this.splitContainer2.Panel1MinSize = 480;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.listBoxElements);
            this.splitContainer2.Size = new System.Drawing.Size(860, 181);
            this.splitContainer2.SplitterDistance = 505;
            this.splitContainer2.TabIndex = 0;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(18, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(505, 181);
            this.webBrowser.TabIndex = 4;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // listBoxElements
            // 
            this.listBoxElements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxElements.FormattingEnabled = true;
            this.listBoxElements.ItemHeight = 16;
            this.listBoxElements.Location = new System.Drawing.Point(0, 0);
            this.listBoxElements.Name = "listBoxElements";
            this.listBoxElements.Size = new System.Drawing.Size(351, 181);
            this.listBoxElements.TabIndex = 1;
            this.listBoxElements.SelectedIndexChanged += new System.EventHandler(this.listBoxElements_SelectedChanged);
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.propertyGridControlHtmlNode);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer5.Size = new System.Drawing.Size(1156, 79);
            this.splitContainer5.SplitterDistance = 364;
            this.splitContainer5.TabIndex = 0;
            // 
            // propertyGridControlHtmlNode
            // 
            this.propertyGridControlHtmlNode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControlHtmlNode.Location = new System.Drawing.Point(0, 0);
            this.propertyGridControlHtmlNode.Name = "propertyGridControlHtmlNode";
            this.propertyGridControlHtmlNode.Size = new System.Drawing.Size(364, 79);
            this.propertyGridControlHtmlNode.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.propertyGridControlWindow);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGridControlHtmlElement);
            this.splitContainer3.Size = new System.Drawing.Size(788, 79);
            this.splitContainer3.SplitterDistance = 404;
            this.splitContainer3.TabIndex = 0;
            // 
            // propertyGridControlWindow
            // 
            this.propertyGridControlWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControlWindow.Location = new System.Drawing.Point(0, 0);
            this.propertyGridControlWindow.Name = "propertyGridControlWindow";
            this.propertyGridControlWindow.Size = new System.Drawing.Size(404, 79);
            this.propertyGridControlWindow.TabIndex = 0;
            // 
            // propertyGridControlHtmlElement
            // 
            this.propertyGridControlHtmlElement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridControlHtmlElement.Location = new System.Drawing.Point(0, 0);
            this.propertyGridControlHtmlElement.Name = "propertyGridControlHtmlElement";
            this.propertyGridControlHtmlElement.Size = new System.Drawing.Size(380, 79);
            this.propertyGridControlHtmlElement.TabIndex = 1;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem3,
            this.barButtonItem4});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 5;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1156, 158);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Загрузить страницу";
            this.barButtonItem1.Glyph = global::MyWebSimulator.Properties.Resources.download_16x16;
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.LargeGlyph = global::MyWebSimulator.Properties.Resources.download_32x32;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "Выбрать элементы";
            this.barButtonItem2.Glyph = global::MyWebSimulator.Properties.Resources.filter_16x16;
            this.barButtonItem2.Id = 2;
            this.barButtonItem2.LargeGlyph = global::MyWebSimulator.Properties.Resources.filter_32x32;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "Симулировать событие элемента";
            this.barButtonItem3.Id = 3;
            this.barButtonItem3.LargeGlyph = global::MyWebSimulator.Properties.Resources.operatingsystem_32x32;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SimulateEvent_ItemClick);
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "Симулировать ввод текста";
            this.barButtonItem4.Glyph = global::MyWebSimulator.Properties.Resources.operatingsystem_32x32;
            this.barButtonItem4.Id = 4;
            this.barButtonItem4.LargeGlyph = global::MyWebSimulator.Properties.Resources.operatingsystem_32x32;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SimulateTextEntry_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Главная";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem2);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem3);
            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem4);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "Действия";
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 653);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1156, 28);
            // 
            // TextEntry
            // 
            this.TextEntry.Name = "TextEntry";
            this.TextEntry.Properties.Caption = "TextEntry";
            this.TextEntry.Properties.FieldName = "TextEntry";
            // 
            // MyWebSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 681);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.Name = "MyWebSimulatorForm";
            this.Ribbon = this.ribbonControl1;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "My Web Simulator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlWorkspace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxUrl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxElementEventInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxXpath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxSimulatorMethodInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxMouseMethodInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxKeyboardMethodInfo)).EndInit();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlHtmlNode)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlWindow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridControlHtmlElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraBars.FormAssistant formAssistant1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private DevExpress.XtraVerticalGrid.PropertyGridControl propertyGridControlHtmlNode;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private System.Windows.Forms.ListBox listBoxNodes;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private MyParserLibrary.WebBrowser webBrowser;
        private System.Windows.Forms.ListBox listBoxElements;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private DevExpress.XtraVerticalGrid.PropertyGridControl propertyGridControlWindow;
        private DevExpress.XtraVerticalGrid.PropertyGridControl propertyGridControlHtmlElement;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private DevExpress.XtraVerticalGrid.PropertyGridControl propertyGridControlWorkspace;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxUrl;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxElementEventInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxXpath;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxSimulatorMethodInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxMouseMethodInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxKeyboardMethodInfo;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow Url;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow Xpath;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ElementEventInfo;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow LastError;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow SimulatorMethodInfo;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow MouseMethodInfo;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow KeyboardMethodInfo;
        private System.Windows.Forms.ListBox listBoxWindows;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow TextEntry;
    }
}

