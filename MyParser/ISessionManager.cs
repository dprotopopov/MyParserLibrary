using System.Net;
using MyLibrary.Lock;
using MyParser.WebSessions;

namespace MyParser
{
    public interface ISessionManager : IManyLock<IWebSession>
    {
        int MaxSessions { set; }
        WebResponse GetResponse(IWebSession webSession, WebRequest webRequest);
        IWebSession AddSession(IWebSession webSession);
        void RemoveSession(IWebSession webSession);
        void Clear();
        bool Contains(IWebSession webSession);
    }
}