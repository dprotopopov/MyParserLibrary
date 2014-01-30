namespace MyParserLibrary
{
    public class ObjectManager
    {
        public ObjectManager(object managedObject)
        {
            ManagedObject = managedObject;
        }

        public object ManagedObject { get; set; }

        public bool Equals(ObjectManager obj)
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