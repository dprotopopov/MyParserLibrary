using System;
using System.Collections.Generic;
using System.Linq;

namespace MyParserLibrary
{
    public class WebTaskManager
    {
        public readonly List<WebTask> WebTasks = new List<WebTask>();
        readonly Dictionary<string, int> _webTasksIndex = new Dictionary<string, int>();

        public int MaxThreads { get; set; }
        public int MaxLevel { get; set; }
        private int TotalRunning
        {
            get
            {
                return WebTasks.Count(task => task.Status == WebTask.WebTaskStatus.Running);
            }
        }

        public void StartAllTasks()
        {
            int toStart = MaxThreads - TotalRunning;
            foreach (var task in WebTasks.Where(task => task.Status == WebTask.WebTaskStatus.Ready).Where(task => toStart-- > 0))
                task.Start();
        }

        public void ResumeAllTasks()
        {
            int toStart = MaxThreads - TotalRunning;
            foreach (var task in WebTasks.Where(task => task.Status == WebTask.WebTaskStatus.Paused).Where(task => toStart-- > 0))
                task.Resume();
        }

        public void AbortAllTasks()
        {
            foreach (var task in WebTasks.Where(task => task.Status == WebTask.WebTaskStatus.Running || task.Status == WebTask.WebTaskStatus.Paused))
                task.Abort();
        }
        private readonly Object _thisLock = new Object();
        public WebTask AddTask(Uri uri, string method, int level)
        {
            lock (_thisLock)
            {
                var newTask = new WebTask(WebTasks.Count, level)
                {
                    Url = uri.AbsoluteUri,
                    Method = method,
                };
                if (newTask.Level < MaxLevel && !_webTasksIndex.ContainsKey(newTask.ToString().ToLower()))
                {
                    WebTasks.Add(newTask);
                    _webTasksIndex.Add(newTask.ToString().ToLower(), newTask.Id);
                    return newTask;
                }
                throw new Exception();
            }
        }
    }
}
