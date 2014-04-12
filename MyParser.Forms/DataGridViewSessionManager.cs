using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using MyLibrary.Lock;
using MyParser.WebSessions;

namespace MyParser.Forms
{
    public class DataGridViewSessionManager<T> : SessionManager, ISingleLock<BindingList<T>>, ISingleLock<GridControl>
    {
        private readonly Dictionary<IWebSession, int> _webCrawlerIndex = new Dictionary<IWebSession, int>();

        public DataGridViewSessionManager()
        {
            MutexItems = new Mutex();
            MutexDataGridView = new Mutex();
            Items = new BindingList<T>();
        }

        public BindingList<T> Items { get; set; }
        public IManagerProvider ChildForm { get; set; }

        private Mutex MutexItems { get; set; }
        public GridControl DataGridView { get; set; }
        private Mutex MutexDataGridView { get; set; }

        public bool Wait(BindingList<T> semaphore)
        {
            while (!MutexItems.WaitOne(0))
            {
                Application.DoEvents();
                Thread.Yield();
            }
            return true;
        }

        public bool Wait(BindingList<T> semaphore, TimeSpan timeout)
        {
            return MutexItems.WaitOne(timeout);
        }

        public void Release(BindingList<T> semaphore)
        {
            MutexItems.ReleaseMutex();
        }

        public bool Wait(GridControl semaphore)
        {
            while (!MutexDataGridView.WaitOne(0))
            {
                Application.DoEvents();
                Thread.Yield();
            }
            return true;
        }

        public bool Wait(GridControl semaphore, TimeSpan timeout)
        {
            return MutexDataGridView.WaitOne(timeout);
        }

        public void Release(GridControl semaphore)
        {
            MutexDataGridView.ReleaseMutex();
        }

        public void OnWebSessionDefault(IWebSession webSession)
        {
            if (Contains(webSession))
                Items[_webCrawlerIndex[webSession]] = (T) webSession.ToViewItem(typeof (T));
        }

        public override IWebSession AddSession(IWebSession webSession)
        {
            if (DataGridView.InvokeRequired) return ChildForm.AddSession(webSession);
            base.AddSession(webSession);
            int index = Items.Count;
            _webCrawlerIndex.Add(webSession, index);
            Items.Add((T) webSession.ToViewItem(typeof (T)));
            return webSession;
        }

        public override void RemoveSession(IWebSession webSession)
        {
            if (DataGridView.InvokeRequired)
            {
                ChildForm.RemoveSession(webSession);
                return;
            }
            int index = _webCrawlerIndex[webSession];
            _webCrawlerIndex.Remove(webSession);
            foreach (var pair in _webCrawlerIndex.Where(pair => pair.Value > index))
                _webCrawlerIndex[pair.Key]--;
            Items.RemoveAt(index);
            base.RemoveSession(webSession);
        }

        public override void Clear()
        {
            base.Clear();
            _webCrawlerIndex.Clear();
            Items.Clear();
        }

        public void AbortAllSessions()
        {
        }
    }
}