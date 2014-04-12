using MyParser.WebSessions;
using MyParser.WebTasks;

namespace MyParser.Forms
{
    public interface IManagerProvider
    {
        IWebSession AddSession(IWebSession obj);
        void RemoveSession(IWebSession obj);
        IWebTask AddTask(IWebTask obj);
        void RemoveTask(IWebTask obj);
        void ClearTaskManager();
        void ClearSessionManager();
    }
}