using System;

namespace MyWebCrawler
{
    /// <summary>
    ///     Аргумент события в виде Uri
    /// </summary>
    public class UriEventArgs : EventArgs
    {
        /// <summary>
        ///     Конструктор аргумента
        /// </summary>
        /// <param name="url"></param>
        public UriEventArgs(Uri url)
        {
            Url = url;
        }

        /// <summary>
        ///     Cсылка
        /// </summary>
        public Uri Url { get; private set; }
    }
}