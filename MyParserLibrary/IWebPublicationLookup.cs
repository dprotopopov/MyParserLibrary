using System.Threading;
using System.Windows.Forms;

namespace MyParserLibrary
{
    public interface IWebPublicationLookup : IWebTask
    {
        string PublicationId { get; set; }
    }
}