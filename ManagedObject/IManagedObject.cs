namespace ManagedObject
{
    public interface IManagedObject
    {
        object ObjectInstance { get; set; }
        bool Equals(IManagedObject obj);
        string ToString();
        bool IsNullOrEmpty();
    }
}