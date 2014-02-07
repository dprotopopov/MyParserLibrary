namespace ManagedObject
{
    public class ManagedObject : IManagedObject
    {
        public ManagedObject(object objectInstance)
        {
            ObjectInstance = objectInstance;
        }

        public object ObjectInstance { get; set; }

        public bool Equals(IManagedObject obj)
        {
            return ObjectInstance.Equals(obj.ObjectInstance);
        }

        public override string ToString()
        {
            return ObjectInstance.ToString();
        }

        public bool IsNullOrEmpty()
        {
            return ObjectInstance == null;
        }
    }
}