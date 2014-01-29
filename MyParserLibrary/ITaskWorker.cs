
namespace MyParserLibrary
{
    public interface ITaskWorker
    {
        void GenerateTasks();
        void StartWorker();
        void StopWorker();
        void AbortWorker();
        void ShowAdvert();
    }
}
