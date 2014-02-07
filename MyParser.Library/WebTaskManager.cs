using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MyParser.Library
{
    /// <summary>
    ///     Менеджер очереди задач
    /// </summary>
    public class WebTaskManager : IWebTaskManager
    {
        private readonly Object _thisLock = new Object();
        private readonly List<IWebTask> _webTasks = new List<IWebTask>();
        private readonly Dictionary<string, int> _webTasksIndex = new Dictionary<string, int>();

        public ListView ListView { get; set; }

        public IWebTask[] Tasks
        {
            get { return _webTasks.ToArray(); }
        }

        public int TotalRunning
        {
            get { return Tasks.Count(task => task.Status == WebTaskStatus.Running); }
        }

        /// <summary>
        ///     Добавление новой задачи в очередь
        /// </summary>
        /// <returns></returns>
        public IWebTask AddTask(IWebTask newTask)
        {
            lock (_thisLock)
            {
                newTask.Id = _webTasks.Count;
                if (newTask.Level < MaxLevel && !_webTasksIndex.ContainsKey(newTask.ToString().ToLower()))
                {
                    _webTasks.Add(newTask);
                    _webTasksIndex.Add(newTask.ToString().ToLower(), newTask.Id);
                    ListViewItem viewItem = newTask.ToListViewItem();
                    if (ListView != null) ListView.Items.Add(viewItem);
                    return newTask;
                }
                throw new Exception();
            }
        }

        public object LastError { get; set; }

        #region Массовые действия

        public void StartAllTasks()
        {
            int count = MaxThreads - TotalRunning;
            foreach (IWebTask task in Tasks)
            {
                if (task.Status == WebTaskStatus.Ready)
                {
                    if (count-- > 0) task.Start();
                }
            }
        }

        public void ResumeAllTasks()
        {
            int count = MaxThreads - TotalRunning;
            foreach (IWebTask task in Tasks)
            {
                if (task.Status == WebTaskStatus.Paused)
                {
                    if (count-- > 0) task.Resume();
                }
            }
        }

        public void AbortAllTasks()
        {
            foreach (IWebTask task in Tasks)
            {
                if (task.Status == WebTaskStatus.Running || task.Status == WebTaskStatus.Paused)
                    task.Abort();
            }
        }

        #endregion

        #region

        /// <summary>
        ///     Максимальное число одновременно работающих потоков
        /// </summary>
        public int MaxThreads { get; set; }

        /// <summary>
        ///     Максимальный уровень задачи, который можно добавить в очередь
        /// </summary>
        public int MaxLevel { get; set; }

        #endregion

        public void OnWebTaskDefault(IWebTask task)
        {
            Debug.Assert(ListView != null);
            try
            {
                ListView.ListViewItemCollection items = ListView.Items;
                ListViewItem lvi = items[task.Id];
                if (lvi != null) lvi.ImageIndex = (int) task.Status;
            }
            catch (Exception exception)
            {
                LastError = exception;
            }
        }
    }
}