using System;
using System.Windows.Forms;
using MyWebSimulator;

namespace MyParser.Library.Forms
{
    public interface IChildForm : ILastError
    {
        IWebTaskManager WebTaskManager { get; set; }
        IWebSimulator WebSimulator { get; set; }
        int MaxLevel { get; set; }
        int MaxThreads { get; set; }
        bool IsRunning { get; set; }
        void GenerateTasks();
        void StartWorker();
        void StopWorker();
        void AbortWorker();
        void ShowAdvert();
        void SaveAs(string fileName);
        void OnWebTaskDefault(IWebTask webTask);
        void OnWebTaskComplite(IWebTask webTask);
        void NavigateToAdvert(object sender, WebBrowserNavigatingEventArgs e);
        void AdvertRefresh(object sender, EventArgs e);
    }
}