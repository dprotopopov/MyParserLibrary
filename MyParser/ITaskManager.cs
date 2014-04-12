using System.Collections.Generic;
using MyLibrary.LastError;
using MyParser.WebTasks;

namespace MyParser
{
    public interface ITaskManager : ILastError
    {
        int TotalRunning { get; }
        IEnumerable<IWebTask> Tasks { get; }
        int MaxLevel { get; set; }
        int MaxThreads { get; set; }
        bool Contains(IWebTask webTask);
        void StartAllTasks();
        void ResumeAllTasks();
        void SuspendAllTasks();
        void AbortAllTasks();
        void JoinAllThreads();

        /// <summary>
        ///     Добавление новой задачи в очередь
        ///     InvokeRequired required compares the thread ID of the
        ///     calling thread to the thread ID of the creating thread.
        ///     If these threads are different, it returns true.
        /// </summary>
        /// <returns></returns>
        IWebTask AddTask(IWebTask webTask);

        void RemoveTask(IWebTask webTask);

        void Clear();
    }
}