using System;
using HtmlAgilityPack;
using MyLibrary.Collections;
using MyParser.WebSessions;

namespace MyParser
{
    public interface ICrawler
    {
        int Edition { get; set; }
        string Method { get; set; }
        string Encoding { get; set; }
        string Compression { get; set; }
        bool UseRandomProxy { get; set; }
        int NumberOfTriesBeforeError { get; set; }
        ISessionManager SessionManager { get; set; }

        /// <summary>
        ///     Возвращает или задает промежуток времени в миллисекундах до истечения срока действия вопроса.
        ///     Продолжительность задержки запроса в миллисекундах или же значение System.Threading.Timeout.Infinite, показывающее,
        ///     что для данного запроса задержка не используется.Значение по умолчанию определяется вложенным классом.
        /// </summary>
        TimeSpan Timeout { set; get; }

        IDatabase Database { get; set; }

        /// <summary>
        ///     Запрос к сайту с использованием RT.Crawler
        /// </summary>
        StackListQueue<HtmlDocument> WebRequestHtmlDocument(Uri uri, IWebSession webSession);
    }
}