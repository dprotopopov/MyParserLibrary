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
        ///     Коннектор к базе данных
        /// </summary>
        SQLiteConnection Connection { get; set; }

        bool Wait(SQLiteConnection connection);

        void Release(SQLiteConnection connection);
        void Connect();

        /// <summary>
        ///     Загрузка из базы данных настроек указанного сайта
        /// </summary>
        SiteProperties GetSiteProperties(object siteId);

        Mappings GetMappings(object siteId);

        /// <summary>
        ///     Сохранение из базу данных настроек сайта
        ///     Не используется
        /// </summary>
        void SetSiteProperties(SiteProperties properties);

        /// <summary>
        ///     Сохранение из базу данных справочника для указанного сайта siteId
        ///     Не используется
        /// </summary>
        void SetMapping(Mapping mapping, string tableName, string keyColumnName, string valueColumnName,
            object siteId);

        /// <summary>
        ///     Загрузка из базы данных описай возвращаемых полей для указанного сайта
        ///     Используется только при загрузке свойств сайта
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
        ///     Загрузка из базы данных всех значений из указанной колонки указанной таблицы
        /// </summary>
        IEnumerable<object> GetList(params object[] parameters);

        /// <summary>
        ///     Загрузка всех пар Ключ-Значение из таблицы базы данных для указанного siteId
        ///     Ключ в поле keyColumnName
        ///     Значение в поле valueColumnName
        /// </summary>
        Mapping GetMapping(params object[] parameters);

        MyLibrary.Collections.Properties GetUserFields(object id, object mappedId, string mappedTableName,
            object siteId);

        /// <summary>
        ///     Выборка скалярного значения из колонки таблицы по ключу == id
        ///     Ключ в поле Название таблицы+"Id"
        ///     Значение в поле columnName
        /// </summary>
        object GetScalar(params object[] parameters);
    }
}