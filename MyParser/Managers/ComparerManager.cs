using System;
using System.Diagnostics;
using MyParser.Comparer;

namespace MyParser.Managers
{
    public class ComparerManager
    {
        /// <summary>
        ///     Создание экземпляра указанного класса, реализующего интерфейс IPublicationComparer
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public IPublicationComparer CreatePublicationComparer(string className)
        {
            string fullClassName = string.Format("MyParser.Comparer.{0}", className);
            Debug.WriteLine(fullClassName);
            Type type = Type.GetType(fullClassName);
            if (type != null)
                return
                    Activator.CreateInstance(type) as
                        IPublicationComparer;
            throw new NotImplementedException();
        }
    }
}