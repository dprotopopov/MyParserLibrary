using System;
using System.Diagnostics;
using System.Threading;
using HtmlAgilityPack;

namespace MyParserLibrary
{
    public delegate void WebTaskCallback(WebTask task);
    /// <summary>
    /// Задача загрузки страницы и разбора её на поля
    /// </summary>
    public class WebTask
    {
        public enum WebTaskStatus
        {
            Ready = 0,
            Running = 1,
            Finished = 2,
            Paused = 3,
            Canceled = 4,
            Error = 5
        }

        public override string ToString()
        {
            return Url;
        }

        #region Аттрибуты

        public int Id { get; set; }
        public int Level { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public ReturnFieldInfos ReturnFieldInfos { get; set; }
        public ReturnFields ReturnFields { get; set; }
        protected Thread Thread { get; set; }
        public WebTaskStatus Status { get; set; }

        #endregion


        #region Callback функции

        public WebTaskCallback OnStartCallback { get; set; }
        public WebTaskCallback OnAbortCallback { get; set; }
        public WebTaskCallback OnResumeCallback { get; set; }
        public WebTaskCallback OnCompliteCallback { get; set; }
        public WebTaskCallback OnErrorCallback { get; set; }

        #endregion


        public static void ThreadProc(object obj)
        {
            WebTask This = obj as WebTask;
            Debug.Assert(This != null, "This != null");
            HtmlDocument doc = MyParserLibrary.WebRequestHtmlDocument(This.Url, This.Method);
            try
            {
                Arguments arguments = MyParserLibrary.BuildArguments(This.Url, This.Method);
                This.ReturnFields = MyParserLibrary.BuildReturnFields(doc.DocumentNode, arguments, This.ReturnFieldInfos);
                This.Status = WebTaskStatus.Finished;
                This.Thread = null;
                This.OnCompliteCallback(This);
            }
            catch (Exception)
            {
                This.Status = WebTaskStatus.Error;
                This.Thread = null;
                This.OnErrorCallback(This);
            }
        }

        public WebTask(int id, int level)
        {
            Id = id;
            Level = level;
            Status = WebTaskStatus.Ready;
        }

        #region Методы

        public void Start()
        {
            Thread = new Thread(ThreadProc);
            if (Thread != null)
            {
                Status = WebTaskStatus.Running;
                OnStartCallback(this);
                Thread.Start(this);
            }
        }

        public void Abort()
        {
            if (Thread != null)
            {
                Thread.Abort();
                Status = WebTaskStatus.Canceled;
                Thread = null;
                OnAbortCallback(this);
            }
        }

        public void Resume()
        {
            if (Thread != null)
            {
                Status = WebTaskStatus.Running;
                OnResumeCallback(this);
                Thread.Resume();
            }
        }

        public void Join()
        {
            if (Thread != null) Thread.Join();
        }

        #endregion

    }
}
