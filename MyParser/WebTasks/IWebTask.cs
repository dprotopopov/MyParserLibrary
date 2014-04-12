using System;
using System.Threading;
using MyLibrary;
using MyLibrary.LastError;
using MyParser.WebSessions;

namespace MyParser.WebTasks
{
    public interface IWebTask : ILastError, IValueable
    {
        int Level { get; set; }
        string Url { get; set; }
        string Method { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Cookie { set; get; }
        string Encoding { get; set; }
        string Compression { get; set; }
        ReturnFieldInfos ReturnFieldInfos { get; set; }
        ReturnFields ReturnFields { get; set; }
        WebTaskStatus Status { get; set; }
        WebTaskCallback OnStartCallback { get; set; }
        WebTaskCallback OnAbortCallback { get; set; }
        WebTaskCallback OnResumeCallback { get; set; }
        WebTaskCallback OnCompliteCallback { get; set; }
        WebTaskCallback OnErrorCallback { get; set; }
        WebTaskCallback OnSuspendCallback { get; set; }
        IParser Parser { get; set; }
        ICrawler Crawler { get; set; }
        ITransformation Transformation { get; set; }
        ISessionManager SessionManager { get; set; }
        ITaskManager TaskManager { get; set; }
        IDatabase Database { get; set; }
        Thread Thread { get; set; }
        IWebSession WebSession { get; set; }
        void Start();
        void Abort();
        void Resume();
        void Join();
        void Suspend();
        string ToString();
        void ThreadProc(object obj);
        object ToViewItem(Type type);
        string ToSql();
        void Reset();
    }
}