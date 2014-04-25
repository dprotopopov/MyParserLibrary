using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using MyLibrary.LastError;
using MyParser.ItemView;

namespace MyParser.WebSessions
{
    /// <summary>
    /// </summary>
    public sealed class WebSession : IWebSession, ILastError, IValueable
    {
        public WebSession()
        {
            Status = WebSessionStatus.Ready;
            State = new object();
        }

        /// <summary>
        /// </summary>
        public object LastError { get; set; }

        public Values ToValues()
        {
            return new Values(this);
        }

        public WebSessionStatus Status { get; set; }
        public WebSessionCallback OnStartCallback { get; set; }
        public WebSessionCallback OnCompliteCallback { get; set; }
        public WebSessionCallback OnErrorCallback { get; set; }
        public string Method { get; set; }
        public string Request { get; set; }
        public WebRequest WebRequest { get; set; }
        public WebResponse WebResponse { get; set; }
        public IAsyncResult AsyncResult { get; set; }
        public object State { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public void GetResponse()
        {
            try
            {
                Status = WebSessionStatus.Running;
                if (OnStartCallback != null) OnStartCallback(this);
                Thread.Yield();
                AsyncResult = WebRequest.BeginGetResponse(AsyncCallback, State);
                while (!AsyncResult.AsyncWaitHandle.WaitOne(0)) Thread.Sleep(1000);
                Status = WebSessionStatus.Complite;
                if (OnCompliteCallback != null) OnCompliteCallback(this);
                Thread.Yield();
            }
            catch (WebException exception)
            {
                LastError = exception;
                Status = WebSessionStatus.Error;
                if (OnErrorCallback != null) OnErrorCallback(this);
                Thread.Yield();
                throw;
            }
            catch (Exception exception)
            {
                LastError = exception;
                Status = WebSessionStatus.Error;
                if (OnErrorCallback != null) OnErrorCallback(this);
                Thread.Yield();
                throw;
            }
        }

        public object ToViewItem(Type type)
        {
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (WebSessionView)))
            {
                var webSessionView = Activator.CreateInstance(type) as WebSessionView;
                webSessionView.Icon = Defaults.WebSessionIcons[(int) Status];
                webSessionView.Status = Status.ToString();
                webSessionView.Text = ToString();
                return webSessionView;
            }
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (ListViewItem)))
            {
                var listViewItem = Activator.CreateInstance(type) as ListViewItem;
                listViewItem.Name = ToString().ToLower();
                listViewItem.ImageIndex = (int) Status;
                return listViewItem;
            }
            if (MyLibrary.Types.Type.IsKindOf(type, typeof (String)))
                return ToString();
            throw new NotImplementedException();
        }

        public void Reset()
        {
            Status = WebSessionStatus.Ready;
        }

        private void AsyncCallback(IAsyncResult ar)
        {
            WebResponse = WebRequest.EndGetResponse(ar);
        }
    }
}