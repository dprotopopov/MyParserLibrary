using System;
using MyLibrary.LastError;
using MyParser.WebSessions;
using MyParser.WebTasks;

namespace MyParser.Forms
{
    public interface IChildForm<TTaskViewItem, TSessionViewItem, TResultViewItem> : ILastError, IManagerProvider
    {
        DataGridViewSessionManager<TSessionViewItem> DataGridViewSessionManager { get; set; }
        DataGridViewTaskManager<TTaskViewItem> DataGridViewTaskManager { get; set; }
        DataGridViewResultManager<TResultViewItem> DataGridViewResultManager { get; set; }
        int MaxLevel { get; set; }
        int MaxThreads { get; set; }
        int MaxSessions { get; set; }
        bool UseRandomProxy { get; set; }
        TimeSpan Timeout { get; set; }
        bool IsRunning { get; set; }
        void IdleUpdate(params object[] parameters);
        void IdleLauncher(params object[] parameters);

        /// <summary>
        ///     Создание новых задач при инициализации формы
        /// </summary>
        void GenerateTasks();

        void StartWorker(params object[] parameters);
        void StopWorker(params object[] parameters);
        void AbortWorker(params object[] parameters);
        void SaveAs(string fileName);
        void LoadFrom(string fileName);
        void OnWebSessionDefault(IWebSession webSession);
        void OnWebTaskDefault(IWebTask webTask);
        void OnWebTaskCompliteOrError(IWebTask webTask);
    }
}