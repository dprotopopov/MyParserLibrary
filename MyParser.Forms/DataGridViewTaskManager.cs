using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using MyLibrary.Lock;
using MyParser.WebTasks;

namespace MyParser.Forms
{
    /// <summary>
    ///     Менеджер очереди задач
    /// </summary>
    public class DataGridViewTaskManager<T> : TaskManager, ISingleLock<GridControl>, ISingleLock<BindingList<T>>
    {
        private readonly Dictionary<IWebTask, int> _webTaskIndex = new Dictionary<IWebTask, int>();

        public DataGridViewTaskManager()
        {
            MutexItems = new Mutex();
            MutexDataGridView = new Mutex();
            Items = new BindingList<T>();
        }

        public IManagerProvider ChildForm { get; set; }

        public BindingList<T> Items { get; set; }
        public GridControl DataGridView { get; set; }
        private Mutex MutexItems { get; set; }
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

        /// <summary>
        ///     Добавление новой задачи в очередь
        /// </summary>
        /// <returns></returns>
        public new IWebTask AddTask(IWebTask webTask)
        {
            base.AddTask(webTask);
            _webTaskIndex.Add(webTask, _webTaskIndex.Count);
            Items.Add((T) webTask.ToViewItem(typeof (T)));
            return webTask;
        }

        public new void RemoveTask(IWebTask webTask)
        {
            int index = _webTaskIndex[webTask];
            _webTaskIndex.Remove(webTask);
            foreach (var pair in _webTaskIndex.Where(pair => pair.Value > index))
                _webTaskIndex[pair.Key]--;
            Items.RemoveAt(index);
            base.RemoveTask(webTask);
        }

        public new void Clear()
        {
            base.Clear();
            _webTaskIndex.Clear();
            Items.Clear();
        }

        public void OnWebTaskDefault(IWebTask webTask)
        {
            Items[_webTaskIndex[webTask]] = (T) webTask.ToViewItem(typeof (T));
        }
    }
}