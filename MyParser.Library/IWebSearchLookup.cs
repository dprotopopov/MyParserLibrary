using System.Threading;
using System.Windows.Forms;

namespace MyParser.Library
{
    public interface IWebSearchLookup : IWebTask
    {
        int PageId { get; set; }
    }
}