using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MyParser.Forms
{
    public partial class AboutBox : XtraForm
    {
        public AboutBox()
        {
            InitializeComponent();
            Application.Idle += (sender, e) => Thread.Yield();
        }

        #region Методы доступа к атрибутам сборки

        private string AssemblyTitle
        {
            get
            {
                object[] attributes =
                    Assembly.GetCustomAttributes(typeof (AssemblyTitleAttribute), false);
                if (attributes.Length <= 0)
                    return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                var titleAttribute = (AssemblyTitleAttribute) attributes[0];
                return titleAttribute.Title != string.Empty
                    ? titleAttribute.Title
                    : Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        private string AssemblyVersion
        {
            get { return Assembly.GetName().Version.ToString(); }
        }

        private string AssemblyDescription
        {
            get
            {
                object[] attributes =
                    Assembly.GetCustomAttributes(typeof (AssemblyDescriptionAttribute), false);
                return attributes.Length == 0
                    ? string.Empty
                    : ((AssemblyDescriptionAttribute) attributes[0]).Description;
            }
        }

        private string AssemblyProduct
        {
            get
            {
                object[] attributes =
                    Assembly.GetCustomAttributes(typeof (AssemblyProductAttribute), false);
                return attributes.Length == 0 ? string.Empty : ((AssemblyProductAttribute) attributes[0]).Product;
            }
        }

        private string AssemblyCopyright
        {
            get
            {
                object[] attributes =
                    Assembly.GetCustomAttributes(typeof (AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute) attributes[0]).Copyright;
            }
        }

        private string AssemblyCompany
        {
            get
            {
                object[] attributes =
                    Assembly.GetCustomAttributes(typeof (AssemblyCompanyAttribute), false);
                return attributes.Length == 0 ? string.Empty : ((AssemblyCompanyAttribute) attributes[0]).Company;
            }
        }

        #endregion

        public Assembly Assembly { private get; set; }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            Text = String.Format("О программе {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Версия {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;
            textBoxDescription.Text = AssemblyDescription;
        }
    }
}