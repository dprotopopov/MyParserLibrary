using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace MyParserLibrary
{
    /// <summary>
    /// Параметры для процедуры возвращающей вычисленные поля при разборе страницы
    /// </summary>
    public class ReturnFieldInfo : Dictionary<string, string>
    {
        /// <summary>
        /// Стандартный конвертер содержания в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this)
            {
                sb.AppendFormat("%s\t: %s\n", item.Key, item.Value);
            }
            return sb.ToString();
        }

        public ListViewItem ToListViewItem()
        {
            ListViewItem viewItem = new ListViewItem(ReturnFieldId);
            viewItem.SubItems.Add(ReturnFieldXpath);
            viewItem.SubItems.Add(ReturnFieldResult);
            viewItem.SubItems.Add(ReturnFieldRegexPattern);
            viewItem.SubItems.Add(ReturnFieldRegexReplacement);
            viewItem.SubItems.Add(ReturnFieldRegexSelect);
            return viewItem;
        }
        /// <summary>
        /// Идентификатор возвращаемого поля
        /// </summary>
        public string ReturnFieldId
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Xpath для нахождения поля на загруженной странице
        /// </summary>
        public string ReturnFieldXpath
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон возвращаемого найденого текста 
        /// </summary>
        public string ReturnFieldResult
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон регулярного выражения, 
        /// используемого при замене найденого текста
        /// </summary>
        public string ReturnFieldRegexPattern
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон для замены у регулярного выражения, 
        /// используемого при замене найденого текста 
        /// </summary>
        public string ReturnFieldRegexReplacement
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
        /// <summary>
        /// Шаблон регулярного выражения для выборки из найденого текста 
        /// </summary>
        public string ReturnFieldRegexSelect
        {
            get
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (!ContainsKey(propertyName)) Add(propertyName, "");
                return this[propertyName];
            }
            set
            {
                string propertyName = MethodBase.GetCurrentMethod().Name.Substring(4);
                if (ContainsKey(propertyName))
                    this[propertyName] = value;
                else
                    Add(propertyName, value);
            }
        }
    }
}
