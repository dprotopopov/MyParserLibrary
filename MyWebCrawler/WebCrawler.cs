using System;
using System.Net;
using System.Threading.Tasks;

namespace MyWebCrawler
{
    /// <summary>
    ///     Crawler (получения контента с веба)
    /// </summary>
    public sealed class WebCrawler : ICrawler
    {
        /// <summary>
        ///     Получить ответ
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Ответ</returns>
        public async Task<WebResponse> GetResponse(WebRequest request)
        {
            OnUriLogger(new UriEventArgs(request.RequestUri)); //залогируем запрос 

            return await Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                null);
        }

        #region Helper Methods

        /// <summary>
        ///     Событие запроса Uri
        /// </summary>
        public static event EventHandler<UriEventArgs> UriLogger;

        /// <summary>
        ///     Логирование запроса ссылки
        /// </summary>
        /// <param name="e"></param>
        private void OnUriLogger(UriEventArgs e)
        {
            if (UriLogger != null)
                UriLogger(this, e);
        }

        #endregion
    }
}