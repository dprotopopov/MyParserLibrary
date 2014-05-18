using System.Collections.Generic;
using MyLibrary.Collections;

namespace MyParser
{
    public interface IDatabase : MyDatabase.IDatabase
    {
        string SiteTable { get; }
        string MappingTable { get; }
        string HierarchicalTable { get; }
        string ReturnFieldTable { get; }
        string BuilderTable { get; }
        string TableBuilderTable { get; }
        string ProxyTable { get; }
        string ModuleNamespaceColumn { get; }
        string ModuleClassnameColumn { get; }
        string AddressColumn { get; }
        string PortColumn { get; }
        string SchemaColumn { get; }
        string SiteIdColumn { get; }
        string ModuleClassname { get; set; }


        /// <summary>
        ///     �������� �� ���� ������ �������� ���������� �����
        /// </summary>
        SiteProperties GetSiteProperties(object siteId);

        Mappings GetMappings(object siteId);

        /// <summary>
        ///     ���������� �� ���� ������ �������� �����
        ///     �� ������������
        /// </summary>
        void SetSiteProperties(SiteProperties properties);

        /// <summary>
        ///     ���������� �� ���� ������ ����������� ��� ���������� ����� siteId
        ///     �� ������������
        /// </summary>
        void SetMapping(Mapping mapping, string tableName, string keyColumnName, string valueColumnName,
            object siteId);

        /// <summary>
        ///     �������� �� ���� ������ ������ ������������ ����� ��� ���������� �����
        ///     ������������ ������ ��� �������� ������� �����
        /// </summary>
        ReturnFieldInfos GetReturnFieldInfos(object siteId);

        BuilderInfos GetBuilderInfos(object siteId);
        Proxy GetNextProxy();

        /// <summary>
        ///     �������� �� ���� ������ ���� �������� �� ��������� ������� ��������� �������
        /// </summary>
        new IEnumerable<object> GetList(params object[] parameters);

        /// <summary>
        ///     �������� ���� ��� ����-�������� �� ������� ���� ������ ��� ���������� siteId
        ///     ���� � ���� keyColumnName
        ///     �������� � ���� valueColumnName
        /// </summary>
        Mapping GetMapping(params object[] parameters);

        MyLibrary.Collections.Properties GetUserFields(object id, object mappedId, string mappedTableName,
            object siteId);

        /// <summary>
        ///     ������� ���������� �������� �� ������� ������� �� ����� == id
        ///     ���� � ���� �������� �������+"Id"
        ///     �������� � ���� columnName
        /// </summary>
        new object GetScalar(params object[] parameters);

        TableBuilderInfos GetTableBuilderInfos(object siteId);
    }
}