using System;
using System.Collections.Generic;
using System.IO;
using MyParser.WebSessions;

namespace MyParser
{
    public interface ICrawler
    {
        int Edition { get; set; }
        string Method { get; set; }

        /// <summary>
        ///     ������ ��� POST � JSON
        /// </summary>
        string Request { get; set; }

        string Encoding { get; set; }
        string Compression { get; set; }
        bool UseRandomProxy { get; set; }
        int NumberOfTriesBeforeError { get; set; }
        ISessionManager SessionManager { get; set; }

        /// <summary>
        ///     ���������� ��� ������ ���������� ������� � ������������� �� ��������� ����� �������� �������.
        ///     ����������������� �������� ������� � ������������� ��� �� �������� System.Threading.Timeout.Infinite, ������������,
        ///     ��� ��� ������� ������� �������� �� ������������.�������� �� ��������� ������������ ��������� �������.
        /// </summary>
        TimeSpan Timeout { set; get; }

        IDatabase Database { get; set; }

        /// <summary>
        ///     ������ � ����� � �������������� RT.Crawler
        /// </summary>
        IEnumerable<MemoryStream> WebRequest(Uri uri, IWebSession webSession);
    }
}