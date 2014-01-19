using System;
using System.Collections.Generic;
using System.Linq;

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
            int toStart = MaxThreads - TotalRunning;
            foreach (
                var task in
                    Tasks.Where(task => task.Status == WebTask.WebTaskStatus.Ready).Where(task => toStart-- > 0))
                task.Start();
        }

        public void ResumeAllTasks()
        {
            int toStart = MaxThreads - TotalRunning;
            foreach (
                var task in
                    Tasks.Where(task => task.Status == WebTask.WebTaskStatus.Paused).Where(task => toStart-- > 0))
                task.Resume();
        }

        public void AbortAllTasks()
        {
            foreach (
                var task in
                    Tasks.Where(
                        task =>
                            task.Status == WebTask.WebTaskStatus.Running || task.Status == WebTask.WebTaskStatus.Paused)
                )
                task.Abort();
        }

        #endregion

        private readonly Object _thisLock = new Object();
        /// <summary>
        /// Добавление новой задачи в очередь
        /// 
        /// Пример использования:
        ///try
        ///{
        ///    WebTask newTask = WebTaskManager.AddTask(uri, "GET", task.Level + 1);
        ///    newTask.OnStartCallback = OnWebTaskDefault;
        ///    newTask.OnAbortCallback = OnWebTaskDefault;
        ///    newTask.OnResumeCallback = OnWebTaskDefault;
        ///    newTask.OnErrorCallback = OnWebTaskDefault;
        ///    newTask.OnCompliteCallback = OnWebTaskComplite;
        ///    newTask.ReturnFieldInfos = ReturnFieldInfos;
        ///    ListViewItem viewItem = new ListViewItem(newTask.ToString())
        ///    {
        ///        ImageIndex = 0,
        ///        Name = newTask.ToString().ToLower()
        ///    };
        ///    viewItem.SubItems.Add(Convert.ToString(newTask.Level));
        ///    listView2.Items.Add(viewItem);
        ///}
        ///catch (Exception)
        ///{
        ///}
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public WebTask AddTask(Uri uri, string method, int level)
        {
            lock (_thisLock)
            {
                var newTask = new WebTask(Tasks.Count, level)
                {
                    Url = uri.AbsoluteUri,
                    Method = method,
                };
                if (newTask.Level < MaxLevel && !_webTasksIndex.ContainsKey(newTask.ToString().ToLower()))
                {
                    Tasks.Add(newTask);
                    _webTasksIndex.Add(newTask.ToString().ToLower(), newTask.Id);
                    return newTask;
                }
                throw new Exception();
            }
        }
    }
}
