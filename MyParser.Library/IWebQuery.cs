
namespace MyParser.Library
{
    public interface IWebQuery : IWebTask
    {
        WebQueryCallback OnQueryCompliteCallback { get; set; }
    }
}