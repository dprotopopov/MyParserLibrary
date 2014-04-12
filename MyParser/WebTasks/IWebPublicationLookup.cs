namespace MyParser.WebTasks
{
    public interface IWebPublicationLookup : IWebTask
    {
        string PublicationId { get; set; }
    }
}