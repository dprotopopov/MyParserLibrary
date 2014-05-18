using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;

namespace MyParser
{
    public interface IParser
    {
        /// <summary>
        ///     ����� � ������������ �������� ������������ ����� ������������ � ����� ����������
        /// </summary>
        ReturnFields BuildReturnFields(IEnumerable<MemoryStream> streams, Values parents,
            IEnumerable<ReturnFieldInfo> returnFieldInfos);

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