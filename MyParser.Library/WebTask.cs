using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MyParser.Library
{
    /// <summary>
    ///     Задача загрузки страницы и разбора её на поля
    /// </summary>
    public class WebTask : IWebTask
    {
        public WebTask()
        {
            Status = WebTaskStatus.Ready;
            ReturnFieldInfos = new ReturnFieldInfos();
            Library = new MyLibrary();
            Method = "GET";
            Cookie = "";
            Encoding = "";
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
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
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
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
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
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
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
                Debug.WriteLine(MethodBase.GetCurrentMethod().Name + ":" + LastError);
            }
        }

        #endregion

        public override string ToString()
        {
            ParametersValues parametersValues = MyLibrary.BuildParametersValues(Url, this);
            string url = MyLibrary.ParseRowTemplate(Url, parametersValues);
            return url;
        }

        public virtual async void ThreadProc(object obj)
        {
            var This = obj as IWebTask;
            Debug.Assert(This != null, "This != null");
            try
            {
                ParametersValues parametersValues = MyLibrary.BuildParametersValues(This.Url, This);
                var returnFields = new ReturnFields();
                string url = MyLibrary.ParseRowTemplate(Url, parametersValues);
                var uri = new Uri(url);
                HtmlDocument[] docs =
                    await MyLibrary.WebRequestHtmlDocument(uri, This.Method, This.Encoding);
                HtmlNode[] nodes = docs.Select(doc => doc.DocumentNode).ToArray();

                returnFields.InsertOrAppend(MyLibrary.BuildReturnFields(nodes, parametersValues,
                    This.ReturnFieldInfos));
                This.ReturnFields = returnFields;
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

        public string Encoding { get; set; }

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
        public MyLibrary Library { get; set; }

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