using System.Threading;
using System.Windows.Forms;

namespace MyParserLibrary
{
    public interface IWebSearchLookup : IWebTask
    {
        int PageId { get; set; }
    }
}