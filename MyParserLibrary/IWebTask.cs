using System.Threading;
using System.Windows.Forms;

namespace MyParserLibrary
{
    public interface IWebTask
    {
        int Id { get; set; }
        int Level { get; set; }
        string Url { get; set; }
        string Method { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        ReturnFieldInfos ReturnFieldInfos { get; set; }
        ReturnFields ReturnFields { get; set; }
        Thread Thread { get; set; }
        WebTask.WebTaskStatus Status { get; set; }
        MyParserLibrary ParserLibrary { get; set; }
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