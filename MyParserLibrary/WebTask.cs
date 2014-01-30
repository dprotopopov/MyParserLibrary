using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MyParserLibrary
{
    /// <summary>
    ///     Задача загрузки страницы и разбора её на поля
    /// </summary>
    public class WebTask : IWebTask
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

        public WebTask()
        {
            Status = WebTaskStatus.Ready;
            ReturnFieldInfos = new ReturnFieldInfos();
            ParserLibrary = new MyParserLibrary();
            Method = "GET";
            Cookie = "";
        }

        #region Методы

        public object LastError { get; set; }

        public void Start()
        {
            Thread = new Thread(ThreadProc);
            if (Thread != null)
            {
                Status = WebTaskStatus.Running;
                if (OnStartCallback != null) OnStartCallback(this);
                Thread.Start(this);
            }
            else
            {
                LastError = "Thread is null";
            }
        }

        public void Abort()
        {
            if (Thread != null)
            {
                Thread.Abort();
                Status = WebTaskStatus.Canceled;
                Thread = null;
                if (OnAbortCallback != null) OnAbortCallback(this);
            }
            else
            {
                LastError = "Thread is null";
            }
        }

        public void Resume()
        {
            if (Thread != null)
            {
                Status = WebTaskStatus.Running;
                if (OnResumeCallback != null) OnResumeCallback(this);
                Thread.Resume();
            }
            else
            {
                LastError = "Thread is null";
            }
        }

        public void Join()
        {
            if (Thread != null)
            {
                Thread.Join();
            }
            else
            {
                LastError = "Thread is null";
            }
        }

        #endregion

        public override string ToString()
        {
            Arguments arguments = ParserLibrary.BuildArguments(this);
            return ParserLibrary.ParseTemplate(Url, arguments);
        }

        public virtual void ThreadProc(object obj)
        {
            var This = obj as WebTask;
            Debug.Assert(This != null, "This != null");
            Arguments arguments = This.ParserLibrary.BuildArguments(This);
            string url = This.ParserLibrary.ParseTemplate(This.Url, arguments);
            HtmlDocument doc = This.ParserLibrary.WebRequestHtmlDocument(url, This.Method);
            try
            {
                This.ReturnFields = This.ParserLibrary.BuildReturnFields(doc.DocumentNode, arguments,
                    This.ReturnFieldInfos);
                This.Status = WebTaskStatus.Finished;
                This.Thread = null;
                if (This.OnCompliteCallback != null) This.OnCompliteCallback(This);
            }
            catch (Exception exception)
            {
                This.LastError = exception;
                This.Status = WebTaskStatus.Error;
                This.Thread = null;
                if (This.OnErrorCallback != null) This.OnErrorCallback(This);
            }
        }

        public virtual ListViewItem ToListViewItem()
        {
            var viewItem = new ListViewItem(ToString().ToLower()) {ImageIndex = (int) Status};
            viewItem.SubItems.Add(Convert.ToString(Level));
            return viewItem;
        }

        #region Аттрибуты

        public int Id { get; set; }
        public int Level { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ReturnFieldInfos ReturnFieldInfos { get; set; }
        public ReturnFields ReturnFields { get; set; }
        public Thread Thread { get; set; }
        public WebTaskStatus Status { get; set; }
        public string Cookie { set; get; }
        public MyParserLibrary ParserLibrary { get; set; }

        #endregion

        #region Callback функции

        public WebTaskCallback OnStartCallback { get; set; }
        public WebTaskCallback OnAbortCallback { get; set; }
        public WebTaskCallback OnResumeCallback { get; set; }
        public WebTaskCallback OnCompliteCallback { get; set; }
        public WebTaskCallback OnErrorCallback { get; set; }

        #endregion
    }
}