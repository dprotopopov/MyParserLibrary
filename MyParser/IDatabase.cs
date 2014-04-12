using System.Collections.Generic;
using System.Data.SQLite;
using MyLibrary.Collections;

namespace MyParser
{
    public interface IDatabase
    {
        string SiteTable { get; }
        string MappingTable { get; }
        string HierarchicalTable { get; }
        string ReturnFieldTable { get; }
        string BuilderTable { get; }
        string ProxyTable { get; }
        string IdColumn { get; }
        string TitleColumn { get; }
        string TableNameColumn { get; }
        string LevelColumn { get; }
        string ParentIdColumn { get; }
        string HasChildColumn { get; }
        string ModuleNamespaceColumn { get; }
        string ModuleClassnameColumn { get; }
        string AddressColumn { get; }
        string PortColumn { get; }
        string SchemaColumn { get; }
        string SiteIdColumn { get; }
        string ModuleClassname { get; set; }

        /// <summary>
        ///     ��������� � ���� ������
        /// </summary>
        SQLiteConnection Connection { get; set; }

        bool Wait(SQLiteConnection connection);

        void Release(SQLiteConnection connection);
        void Connect();

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
        IEnumerable<Record> Load(Record maskRecord);
        void Delete(Record maskRecord);
        Record GetNext(Record maskRecord);
        bool Exists(Record maskRecord);
        int InsertOrReplace(IEnumerable<Record> records);
        int InsertOrReplace(Record record);
        int ExecuteNonQuery(string commandText);
        Proxy GetNextProxy();
        int InsertIfNoExists(Record record);

        /// <summary>
        ///     �������� �� ���� ������ ���� �������� �� ��������� ������� ��������� �������
        /// </summary>
        IEnumerable<object> GetList(params object[] parameters);

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
        object GetScalar(params object[] parameters);
    }
}