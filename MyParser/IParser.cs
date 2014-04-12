using System.Collections.Generic;
using HtmlAgilityPack;

namespace MyParser
{
    public interface IParser
    {
        /// <summary>
        ///     Поиск и формирование значений возвращаемых полей загруженного с сайта объявления
        /// </summary>
        ReturnFields BuildReturnFields(IEnumerable<HtmlDocument> parentDocuments, Values parentValues,
            ReturnFieldInfos returnFieldInfos);

        /// <summary>
        ///     Формирование пар идентификатор параметра - значение параметра
        ///     для замены в строке-шаблоне
        /// </summary>
        /// <param name="template"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Values BuildValues(string template, HtmlNode node);
    }
}