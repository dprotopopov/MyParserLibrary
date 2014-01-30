namespace MyParserLibrary
{
    public interface IObjectManager
    {
        object ManagedObject { get; set; }
        bool Equals(IObjectManager obj);
        string ToString();
        bool IsNullOrEmpty();
    }
}