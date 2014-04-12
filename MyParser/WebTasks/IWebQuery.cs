namespace MyParser.WebTasks
{
    public interface IWebQuery : IWebTask
    {
        WebQueryCallback OnQueryCompliteCallback { get; set; }
    }
}