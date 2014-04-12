namespace MyParser.WebTasks
{
    public interface IWebSearchLookup : IWebTask
    {
        int PageId { get; set; }
    }
}