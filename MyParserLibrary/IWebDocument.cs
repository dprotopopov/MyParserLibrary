namespace MyParserLibrary
{
    public interface IWebDocument
    {
        IWebElement Body { get; }
        IWebElement[] All { get; }
        IWebWindow Window { get; }
        bool Equals(IWebDocument obj);
        string ToString();
        bool IsNullOrEmpty();
    }
}