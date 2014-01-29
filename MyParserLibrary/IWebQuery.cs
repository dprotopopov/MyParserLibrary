
namespace MyParserLibrary
{
    public interface IWebQuery : IWebTask
    {
        WebQueryCallback OnQueryCompliteCallback { get; set; }
    }
}