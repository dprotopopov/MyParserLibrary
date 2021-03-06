﻿using System;
using System.Net;

namespace MyParser.WebSessions
{
    /// <summary>
    ///     Интерфейс Crawler (получения контента с веба)
    /// </summary>
    public interface IWebSession
    {
        WebSessionStatus Status { get; set; }
        WebSessionCallback OnStartCallback { get; set; }
        WebSessionCallback OnCompliteCallback { get; set; }
        WebSessionCallback OnErrorCallback { get; set; }

        string Method { get; set; }
        string Request { get; set; }
        WebRequest WebRequest { get; set; }
        WebResponse WebResponse { get; set; }
        IAsyncResult AsyncResult { get; set; }
        object State { get; set; }

        /// <summary>
        ///     Получить ответ
        /// </summary>
        /// <returns>Ответ</returns>
        void GetResponse();

        object ToViewItem(Type type);
        void Reset();
    }
}