namespace MyParser.Library
{
    public interface IWebTaskManager: ILastError
    {
        int TotalRunning { get; }
        void StartAllTasks();
        void ResumeAllTasks();
        void AbortAllTasks();

        ///  <summary>
        ///  Добавление новой задачи в очередь
        ///  </summary>
        /// <returns></returns>
        IWebTask AddTask(IWebTask newTask);
        IWebTask[] Tasks { get; }
        int MaxLevel { get; set; }
        int MaxThreads { get; set; }
        void OnWebTaskDefault(IWebTask task);
    }
}