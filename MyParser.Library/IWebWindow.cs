namespace MyParser.Library
{
    public interface IWebWindow
    {
        IWebElement WindowFrameElement { get; }
        IWebWindow[] Frames { get; }
        IWebDocument Document { get; }
        bool Equals(IWebWindow obj);
        string ToString();
        bool IsNullOrEmpty();
    }
}