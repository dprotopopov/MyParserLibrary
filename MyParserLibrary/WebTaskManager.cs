using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace MyParserLibrary
{
    /// <summary>
    /// Менеджер очереди задач
    /// </summary>
    public class WebTaskManager
    {
        readonly List<WebTask> _webTasks = new List<WebTask>();
        readonly Dictionary<string, int> _webTasksIndex = new Dictionary<string, int>();

        public List<WebTask> Tasks { get { return _webTasks; } }

        #region

        /// <summary>
        /// Максимальное число одновременно работающих потоков
        /// </summary>
        public int MaxThreads { get; set; }

        /// <summary>
        /// Максимальный уровень задачи, который можно добавить в очередь
        /// </summary>
        public int MaxLevel { get; set; }

        #endregion

        public ListView ListView { get; set; }

        public void OnWebTaskDefault(WebTask task)
        {
            Debug.Assert(ListView != null);
            try
            {
                ListView.ListViewItemCollection items = ListView.Items;
                var lvi = items[task.Id];
                if (lvi != null) lvi.ImageIndex = (int)task.Status;
            }
            catch (Exception)
            {
            }
        }
        private int TotalRunning
        {
            get
            {
                return Tasks.Count(task => task.Status == WebTask.WebTaskStatus.Running);
            }
        }

        #region Массовые действия

        public void StartAllTasks()
        {
            int count = MaxThreads - TotalRunning;
            foreach (var task in Tasks)
            {
                if (task.Status == WebTask.WebTaskStatus.Ready)
                {
                    if (count-- > 0) task.Start();
                }
            }
        }

        public void ResumeAllTasks()
        {
            int count = MaxThreads - TotalRunning;
            foreach (var task in Tasks)
            {
                if (task.Status == WebTask.WebTaskStatus.Paused)
                {
                    if (count-- > 0) task.Resume();
                }
            }
        }

        public void AbortAllTasks()
        {
            foreach (var task in Tasks)
            {
                if (task.Status == WebTask.WebTaskStatus.Running || task.Status == WebTask.WebTaskStatus.Paused) task.Abort();
            }
        }

        #endregion

        private readonly Object _thisLock = new Object();

        ///  <summary>
        ///  Добавление новой задачи в очередь
        ///  </summary>
        /// <returns></returns>
        public WebTask AddTask(WebTask newTask)
        {
            lock (_thisLock)
            {
                newTask.Id = Tasks.Count;
                if (newTask.Level < MaxLevel && !_webTasksIndex.ContainsKey(newTask.ToString().ToLower()))
                {
                    Tasks.Add(newTask);
                    _webTasksIndex.Add(newTask.ToString().ToLower(), newTask.Id);
                    ListViewItem viewItem = newTask.ToListViewItem();
                    if (ListView != null) ListView.Items.Add(viewItem);
                    return newTask;
                }
                throw new Exception();
            }
        }
    }
}
