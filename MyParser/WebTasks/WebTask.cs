using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using MyLibrary;
using MyLibrary.Attribute;
using MyLibrary.Collections;
using MyParser.ItemView;
using MyParser.WebSessions;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MyParser.WebTasks
{
    /// <summary>
    ///     Задача загрузки страницы и разбора её на поля
    /// </summary>
    public class WebTask : IWebTask, IValueable
    {
        public WebTask()
        {
            Status = WebTaskStatus.Ready;
            ReturnFieldInfos = Defaults.ReturnFieldInfos;
            Method = Defaults.Method;
            Cookie = Defaults.Cookie;
            Encoding = Defaults.Encoding;
            Compression = Defaults.Compression;
            Parser = Defaults.Parser;
            Crawler = Defaults.Crawler;
            Transformation = Defaults.Transformation;
            Database = Defaults.Database;
            ReturnFields = Defaults.ReturnFields;
            SessionManager = Defaults.SessionManager;
            TaskManager = Defaults.TaskManager;
            WebSession = Defaults.WebSession;
        }

        public void Reset()
        {
            if (Status == WebTaskStatus.Paused) Thread.Resume();
            if (Thread != null) Thread.Abort();
            Status = WebTaskStatus.Ready;
            WebSession.Reset();
        }

        public IWebSession WebSession { get; set; }

        public virtual Values ToValues()
        {
            return new Values(this);
        }

        public object LastError { get; set; }

        public override string ToString()
        {
            return Transformation.ParseRowTemplate(Url, ToValues());
        }

        public virtual void ThreadProc(object obj)
        {
            var This = obj as WebTask;
            Debug.Assert(This != null, "This != null");
            try
            {
                var builder = new UriBuilder(This.Transformation.ParseRowTemplate(This.Url, This.ToValues()))
                {
                    UserName = This.UserName,
                    Password = This.Password,
                };
                var values = new Values
                {
                    Url = new StackListQueue<string> {builder.ToString()}
                };
                IEnumerable<HtmlDocument> docs = This.Crawler.WebRequestHtmlDocument(builder.Uri, This.WebSession);
                This.ReturnFields = This.Parser.BuildReturnFields(docs, values,
                    This.ReturnFieldInfos);
                This.Status = WebTaskStatus.Finished;
                This.Thread = null;
                if (This.OnCompliteCallback != null) This.OnCompliteCallback(This);
                Thread.Yield();
            }
            catch (Exception exception)
            {
                This.LastError = exception;
                This.Status = WebTaskStatus.Error;
                This.Thread = null;
                if (This.OnErrorCallback != null) This.OnErrorCallback(This);
                Thread.Yield();
            }
        }

        [Value]
        public string Encoding { get; set; }

        [Value]
        public string Compression { get; set; }

        /// <summary>
        ///     Создание элемента ListView для отображения задачи в списке задач
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual object ToViewItem(Type type)
        {
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (WebTaskView)))
            {
                var webTaskView = Activator.CreateInstance(type) as WebTaskView;
                webTaskView.Icon = Defaults.WebTaskIcons[(int) Status];
                webTaskView.Status = Status.ToString();
                webTaskView.Url = ToString();
                webTaskView.Level = Level;
                return webTaskView;
            }
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (ListViewItem)))
            {
                var listViewItem = Activator.CreateInstance(type) as ListViewItem;
                listViewItem.Name = ToString().ToLower();
                listViewItem.ImageIndex = (int) Status;
                listViewItem.SubItems.Add(Convert.ToString(Level));
                return listViewItem;
            }
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (String)))
                return ToString();
            throw new NotImplementedException();
        }

        public virtual string ToSql()
        {
            throw new NotImplementedException();
        }

        [Value]
        public IParser Parser { get; set; }

        [Value]
        public ICrawler Crawler { get; set; }

        [Value]
        public ITransformation Transformation { get; set; }

        [Value]
        public ISessionManager SessionManager { get; set; }

        [Value]
        public ITaskManager TaskManager { get; set; }

        [Value]
        public IDatabase Database { get; set; }

        #region Аттрибуты

        public Thread Thread { get; set; }

        [Value]
        public int Level { get; set; }

        [Value]
        public string Url { get; set; }

        [Value]
        public string Method { get; set; }

        [Value]
        public string UserName { get; set; }

        [Value]
        public string Password { get; set; }

        [Value]
        public ReturnFieldInfos ReturnFieldInfos { get; set; }

        [Value]
        public ReturnFields ReturnFields { get; set; }

        [Value]
        public WebTaskStatus Status { get; set; }

        [Value]
        public string Cookie { set; get; }

        #endregion

        #region Callback функции

        public WebTaskCallback OnStartCallback { get; set; }
        public WebTaskCallback OnAbortCallback { get; set; }
        public WebTaskCallback OnResumeCallback { get; set; }
        public WebTaskCallback OnCompliteCallback { get; set; }
        public WebTaskCallback OnErrorCallback { get; set; }
        public WebTaskCallback OnSuspendCallback { get; set; }

        #endregion

        #region Методы

        public void Start()
        {
            Thread = new Thread(ThreadProc);
            if (Thread != null)
            {
                Status = WebTaskStatus.Running;
                Thread.Priority = ThreadPriority.Lowest;
                Thread.IsBackground = true;
                if (OnStartCallback != null) OnStartCallback(this);
                Thread.Yield();
                Thread.Start(this);
            }
            else
            {
                LastError = "Thread is null";
                Debug.WriteLine("{0}::{1} LastError = {2}", GetType().Name, MethodBase.GetCurrentMethod().Name,
                    LastError);
            }
        }

        public void Abort()
        {
            if (Thread != null)
            {
                if (Status == WebTaskStatus.Paused) Resume();
                Thread.Abort();
                Status = WebTaskStatus.Canceled;
                if (OnAbortCallback != null) OnAbortCallback(this);
                Thread.Yield();
            }
            else
            {
                LastError = "Thread is null";
                Debug.WriteLine("{0}::{1} LastError = {2}", GetType().Name, MethodBase.GetCurrentMethod().Name,
                    LastError);
            }
        }

        public void Resume()
        {
            if (Thread != null)
            {
                Status = WebTaskStatus.Running;
                if (OnResumeCallback != null) OnResumeCallback(this);
                Thread.Yield();
                Thread.Resume();
            }
            else
            {
                LastError = "Thread is null";
                Debug.WriteLine("{0}::{1} LastError = {2}", GetType().Name, MethodBase.GetCurrentMethod().Name,
                    LastError);
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
                Debug.WriteLine("{0}::{1} LastError = {2}", GetType().Name, MethodBase.GetCurrentMethod().Name,
                    LastError);
            }
        }

        public void Suspend()
        {
            if (Thread != null)
            {
                Thread.Suspend();
                Status = WebTaskStatus.Paused;
                if (OnSuspendCallback != null) OnSuspendCallback(this);
                Thread.Yield();
            }
            else
            {
                LastError = "Thread is null";
                Debug.WriteLine("{0}::{1} LastError = {2}", GetType().Name, MethodBase.GetCurrentMethod().Name,
                    LastError);
            }
        }

        #endregion
    }
}