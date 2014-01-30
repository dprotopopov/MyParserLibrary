namespace MyParserLibrary
{
    public class ObjectManager : IObjectManager
    {
        public ObjectManager(object managedObject)
        {
            ManagedObject = managedObject;
        }

        public object ManagedObject { get; set; }

        public bool Equals(IObjectManager obj)
        {
            return ManagedObject.Equals(obj.ManagedObject);
        }

        public override string ToString()
        {
            return ManagedObject.ToString();
        }

        public bool IsNullOrEmpty()
        {
            return ManagedObject == null;
        }
    }
}