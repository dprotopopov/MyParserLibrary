using System.Collections.Generic;
using HtmlAgilityPack;

namespace MyParser
{
    public interface IParser
    {
        /// <summary>
        ///     ����� � ������������ �������� ������������ ����� ������������ � ����� ����������
        /// </summary>
        ReturnFields BuildReturnFields(IEnumerable<HtmlDocument> parentDocuments, Values parentValues,
            ReturnFieldInfos returnFieldInfos);

        /// <summary>
        ///     ������������ ��� ������������� ��������� - �������� ���������
        ///     ��� ������ � ������-�������
        /// </summary>
        /// <param name="template"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        Values BuildValues(string template, HtmlNode node);
    }
}