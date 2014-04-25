using System;
using System.Diagnostics;
using MyParser.Compression;

namespace MyParser.Managers
{
    public class CompressionManager
    {
        /// <summary>
        ///     Создание экземпляра указанного класса, реализующего интерфейс ICompression
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public ICompression CreateCompression(string className)
        {
            string fullClassName = string.Format("MyParser.Compression.{0}", className);
            Debug.WriteLine(fullClassName);
            Type type = Type.GetType(fullClassName);
            if (type != null)
                return
                    Activator.CreateInstance(type) as
                        ICompression;
            throw new NotImplementedException();
        }
    }
}