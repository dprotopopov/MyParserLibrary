using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MyLibrary.Collections;
using MyParser.WebTasks;

namespace MyParser
{
    public class TaskManager : ITaskManager
    {
        private readonly List<IWebTask> _webTasks = new StackListQueue<IWebTask>();

        public IEnumerable<IWebTask> Tasks
        {
            get { return _webTasks.ToList(); }
        }

        public int TotalRunning
        {
            get { return Tasks.Count(task => task.Status == WebTaskStatus.Running); }
        }

        public object LastError { get; set; }

        #region

        public void RemoveTask(IWebTask webTask)
        {
            _webTasks.Remove(webTask);
        }

        public bool Contains(IWebTask webTask)
        {
            return _webTasks.Contains(webTask);
        }

        public void Clear()
        {
            _webTasks.Clear();
        }

        /// <summary>
        ///     Максимальное число одновременно работающих потоков
        /// </summary>
        public int MaxThreads { get; set; }

        /// <summary>
        ///     Максимальный уровень задачи, который можно добавить в очередь
        /// </summary>
        public int MaxLevel { get; set; }

        #endregion

        #region Массовые действия

        public void JoinAllThreads()
        {
            IEnumerable<Thread> threads = Tasks.Select(task => task.Thread);
            foreach (Thread thread in threads.Where(thread => thread != null))
                try
                {
                    thread.Join();
                }
                catch
                {
                }
        }

        public IWebTask AddTask(IWebTask webTask)
        {
            if (webTask.Level >= MaxLevel || _webTasks.Contains(webTask))
                throw new ArgumentException();
            _webTasks.Add(webTask);
            return webTask;
        }

        public void StartAllTasks()
        {
            int count = MaxThreads - TotalRunning;
            foreach (IWebTask task in Tasks.Where(task => task.Status == WebTaskStatus.Ready).Where(task => count-- > 0)
                )
                task.Start();
        }

        public void ResumeAllTasks()
        {
            int count = MaxThreads - TotalRunning;
            foreach (
                IWebTask task in Tasks.Where(task => task.Status == WebTaskStatus.Paused).Where(task => count-- > 0))
                task.Resume();
        }

        public void SuspendAllTasks()
        {
            foreach (IWebTask task in Tasks.Where(task => task.Status == WebTaskStatus.Running))
                task.Suspend();
        }

        public void AbortAllTasks()
        {
            foreach (
                IWebTask task in
                    Tasks.Where(task => task.Status == WebTaskStatus.Running || task.Status == WebTaskStatus.Paused))
                task.Abort();
        }

        #endregion
    }
}