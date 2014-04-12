namespace MyWebSimulator
{
    public interface IWebDocument
    {
        IWebElement RootElement { get; }
        IWebElement Body { get; }
        IWebElement[] All { get; }
        IWebWindow Window { get; }
        bool Equals(IWebDocument obj);
        string ToString();
        bool IsNullOrEmpty();
    }
}