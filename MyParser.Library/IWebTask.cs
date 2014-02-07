using System.Threading;
using System.Windows.Forms;

namespace MyParser.Library
{
    public interface IWebTask : ILastError
    {
        int Id { get; set; }
        int Level { get; set; }
        string Url { get; set; }
        string Method { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Cookie { set; get; }
        string Encoding { get; set; }
        ReturnFieldInfos ReturnFieldInfos { get; set; }
        ReturnFields ReturnFields { get; set; }
        Thread Thread { get; set; }
        WebTaskStatus Status { get; set; }
        MyLibrary Library { get; set; }
        WebTaskCallback OnStartCallback { get; set; }
        WebTaskCallback OnAbortCallback { get; set; }
        WebTaskCallback OnResumeCallback { get; set; }
        WebTaskCallback OnCompliteCallback { get; set; }
        WebTaskCallback OnErrorCallback { get; set; }
        void Start();
        void Abort();
        void Resume();
        void Join();
        string ToString();
        void ThreadProc(object obj);
        ListViewItem ToListViewItem();
    }
}