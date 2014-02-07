using System.Threading;
using System.Windows.Forms;

namespace MyParser.Library
{
    public interface IWebPublicationLookup : IWebTask
    {
        string PublicationId { get; set; }
    }
}