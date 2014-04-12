using System;
using DevExpress.XtraBars;
using MyLibrary.LastError;

namespace MyParser.Forms
{
    public interface IMainForm : ILastError
    {
        int MaxLevel { get; set; }
        int MaxThreads { get; set; }
        int MaxSessions { get; set; }
        bool UseRandomProxy { get; set; }
        TimeSpan Timeout { get; set; }
        void IdleUpdate(params object[] parameters);
        void SaveAs(object sender, ItemClickEventArgs e);
        void ShowAboutBox(object sender, ItemClickEventArgs e);
        void ShowAdvertisement(object sender, ItemClickEventArgs e);
        void ShowFieldInfosDialog(object sender, ItemClickEventArgs e);
        void StartWorker(object sender, ItemClickEventArgs e);
        void StopWorker(object sender, ItemClickEventArgs e);
        void AbortWorker(object sender, ItemClickEventArgs e);
    }
}