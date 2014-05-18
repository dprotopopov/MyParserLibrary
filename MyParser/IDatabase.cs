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
        Proxy GetNextProxy();

        /// <summary>
        ///     Загрузка из базы данных всех значений из указанной колонки указанной таблицы
        /// </summary>
        new IEnumerable<object> GetList(params object[] parameters);

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
        new object GetScalar(params object[] parameters);

        TableBuilderInfos GetTableBuilderInfos(object siteId);
    }
}