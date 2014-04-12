using System;
using System.Windows.Forms;
using CefSharp.Example;

namespace WebView.Evaluate
{
    internal static class Program
    {
        /// <summary>
        ///     Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ExamplePresenter.Init();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WebViewEvaluate());
        }
    }
}