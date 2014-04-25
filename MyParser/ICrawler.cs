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

        /// <summary>
        ///     ƒанные дл€ POST и JSON
        /// </summary>
        string Request { get; set; }

        string Encoding { get; set; }
        string Compression { get; set; }
        bool UseRandomProxy { get; set; }
        int NumberOfTriesBeforeError { get; set; }
        ISessionManager SessionManager { get; set; }

        /// <summary>
        ///     ¬озвращает или задает промежуток времени в миллисекундах до истечени€ срока действи€ вопроса.
        ///     ѕродолжительность задержки запроса в миллисекундах или же значение System.Threading.Timeout.Infinite, показывающее,
        ///     что дл€ данного запроса задержка не используетс€.«начение по умолчанию определ€етс€ вложенным классом.
        /// </summary>
        TimeSpan Timeout { set; get; }

        IDatabase Database { get; set; }

        /// <summary>
        ///     «апрос к сайту с использованием RT.Crawler
        /// </summary>
        StackListQueue<HtmlDocument> WebRequestHtmlDocument(Uri uri, IWebSession webSession);
    }
}