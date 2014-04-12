using System;
using System.Diagnostics;
using MyParser.Comparer;

namespace MyParser.Managers
{
    public class ComparerManager
    {
        public ComparerManager()
        {
            ModuleNamespace = Defaults.ModuleNamespace;
        }

        public string ModuleNamespace { get; set; }

        /// <summary>
        ///     Создание экземпляра указанного класса, реализующего интерфейс IPublicationComparer
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public IPublicationComparer CreatePublicationComparer(string className)
        {
            string fullClassName = string.Format("{0}.Comparer.{1}", ModuleNamespace, className);
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