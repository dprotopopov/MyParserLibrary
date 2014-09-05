namespace MyParser.WebTasks
{
    /// <summary>
    ///     Статус задачи загрузки данных
    /// </summary>
    public enum WebTaskStatus
    {
        Ready = 0,
        Running = 1,
        Finished = 2,
        Paused = 3,
        Canceled = 4,
        Error = 5
    }
}