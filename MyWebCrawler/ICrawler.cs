using System.Net;
using System.Threading.Tasks;

namespace MyWebCrawler
{
    /// <summary>
    ///     Интерфейс Crawler (получения контента с веба)
    /// </summary>
    public interface ICrawler
    {
        /// <summary>
        ///     Получить ответ
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Ответ</returns>
        Task<WebResponse> GetResponse(WebRequest request);
    }
}