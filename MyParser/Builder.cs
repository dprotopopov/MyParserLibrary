using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyLibrary;
using MyLibrary.Attribute;
using MyLibrary.Collections;
using MyLibrary.Comparer;
using MyLibrary.LastError;
using MyLibrary.Trace;
using MyParser.Managers;
using MyParser.WebSessions;
using String = MyLibrary.Types.String;

namespace MyParser
{
    public sealed class Builder : ITrace, ILastError, IValueable
    {
        private const string ReturnTitle = @"Title";
        private const string ReturnValue = @"Value";

        private static readonly Dictionary<string, string> OptionPatches = new Dictionary<string, string>();

        private static readonly Dictionary<string, string> ValuePatches = new Dictionary<string, string>
        {
            {@"\A\/Санкт\-Петербург\Z", @"/Россия/Санкт-Петербург+город"},
            {@"\A\/Москва\Z", @"/Россия/Москва+город"},
            {@"\A\/(Москва|Санкт\-Петербург)\+город", @"/Россия/$1+город"},
        };

        #region

        private string ModuleNamespace { get; set; }
        public IDatabase Database { get; set; }
        private Transformation Transformation { get; set; }
        private Parser Parser { get; set; }
        private ObjectComparer ObjectComparer { get; set; }
        private CompressionManager CompressionManager { get; set; }
        private Converter Converter { get; set; }
        private Crawler Crawler { get; set; }

        #endregion

        public Builder()
        {
            GridItems = new StackListQueue<GridItem>();
            MinLevel = 0;
            MaxLevel = 1;
            SiteMinLevel = 0;
            SiteMaxLevel = 1;
            MaxDistance = Int32.MaxValue;
            ModuleClassname = GetType().Namespace;
            ModuleNamespace = GetType().Namespace;
            Database = new Database {ModuleClassname = ModuleClassname};
            CompressionManager = new CompressionManager();
            Converter = new Converter();
            Transformation = new Transformation();
            Parser = new Parser {Transformation = Transformation};
            Crawler = new Crawler {CompressionManager = CompressionManager};
            ObjectComparer = new ObjectComparer();
        }

        public string ModuleClassname { get; set; }

        public string CommandText { get; set; }
        public object TableName { get; set; }

        public object SiteId
        {
            get { return Site.Key; }
        }

        public KeyValuePair<object, object> Site { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public long MinLevel { get; set; }
        public long MaxLevel { get; set; }
        public long SiteMinLevel { get; set; }
        public long SiteMaxLevel { get; set; }
        public int MaxDistance { get; set; }
        public long Total { get; set; }
        public long Current { get; set; }

        public List<GridItem> GridItems { get; set; }
        public KeyValuePair<Dictionary<object, string>, Dictionary<object, string>> TitleData { get; set; }
        public KeyValuePair<Dictionary<object, string>, Dictionary<object, string>> IndexData { get; set; }
        private ReturnFieldInfos ReturnFieldInfos { get; set; }
        public object LastError { get; set; }
        public ProgressCallback ProgressCallback { get; set; }
        public AppendLineCallback AppendLineCallback { get; set; }
        public CompliteCallback CompliteCallback { get; set; }

        public Values ToValues()
        {
            return new Values(this);
        }

        #region

        private readonly MethodInfo _getListMethodInfo = typeof (Database).GetMethod("GetList");
        private readonly MethodInfo _getMappingMethodInfo = typeof (Database).GetMethod("GetMapping");
        private readonly MethodInfo _scalarMethodInfo = typeof (Database).GetMethod("GetScalar");

        #endregion

        public void BuildGridItems()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var stack = new Stack<object>();
            stack.Push(CompliteCallback);
            CompliteCallback = null;

            var
                titles = new KeyValuePair<
                    Dictionary<object, KeyValuePair<object, string>>,
                    Dictionary<object, KeyValuePair<object, string>>>(
                    TitleData.Key.ToDictionary(
                        item => item.Key, item => item, ObjectComparer),
                    TitleData.Value.ToDictionary(item => item.Key, item => item, ObjectComparer));

            Mapping mapping = Database.GetMapping(TableName.ToString(), MinLevel, MaxLevel, SiteMinLevel, SiteMaxLevel,
                SiteId);

            GridItems.Clear();

            Total += mapping.Count;
            foreach (GridItem item in mapping.Select(pair => new GridItem
            {
                Key = titles.Key[pair.Key],
                Value = titles.Value[pair.Value]
            }))
            {
                GridItems.Add(item);
                if (ProgressCallback != null) ProgressCallback(++Current, Total);
                Thread.Yield();
            }


            CompliteCallback = (CompliteCallback) stack.Pop();


            if (CompliteCallback != null) CompliteCallback();
            Thread.Yield();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void RefreshGridItems()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var stack = new Stack<object>();
            stack.Push(CompliteCallback);
            CompliteCallback = null;


            Total += GridItems.Count;

            AppendLineCallback(string.Format("CREATE TABLE IF NOT EXISTS {0}{1}{2}(", Database.SiteTable,
                TableName, Database.MappingTable));
            AppendLineCallback(string.Format("{0}{1} INTEGER,", Database.SiteTable, Database.IdColumn));
            AppendLineCallback(string.Format("{0}{1} INTEGER,", TableName, Database.IdColumn));
            AppendLineCallback(string.Format("{0}{1}{2} VARCHAR,", Database.SiteTable, TableName,
                Database.IdColumn));
            AppendLineCallback(string.Format("PRIMARY KEY({0}{2},{1}{2}));", Database.SiteTable,
                TableName,
                Database.IdColumn));

            string insertOrReplaceString =
                string.Format(
                    "INSERT OR REPLACE INTO {0}{1}{2}({0}{3},{1}{3},{0}{1}{3}) VALUES ({{{{{0}{3}}}}},{{{{{1}{3}}}}},'{{{{{0}{1}{3}}}}}');",
                    Database.SiteTable,
                    TableName, Database.MappingTable, Database.IdColumn);

            string siteIdPattern =
                string.Format("{0}{1}", Database.SiteTable, Database.IdColumn);
            string tableIdPattern =
                string.Format("{0}{1}", TableName, Database.IdColumn);
            string siteTableIdPattern =
                string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.IdColumn);

            var values = new Values
            {
                {siteIdPattern, Enumerable.Repeat(SiteId.ToString(), GridItems.Count).ToList()},
                {
                    tableIdPattern,
                    GridItems.Select(item => item.Key.Key.ToString()).ToList()
                },
                {
                    siteTableIdPattern,
                    GridItems.Select(item => item.Value.Key.ToString()).ToList()
                },
            };
            IEnumerable<string> commandTexts = Transformation.ParseTemplate(insertOrReplaceString, values);

            AppendLineCallback("BEGIN;");
            foreach (string commandText in commandTexts)
            {
                AppendLineCallback(commandText);
                if (ProgressCallback != null) ProgressCallback(++Current, Total);
            }
            AppendLineCallback("COMMIT;");


            CompliteCallback = (CompliteCallback) stack.Pop();
            if (CompliteCallback != null) CompliteCallback();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        private Dictionary<object, string> BuildFullTitle(ICollection<object> items, IList<object[]> objects)
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine("items.Length={0}", items.Count);
            var stack = new Stack<object>();
            stack.Push(CompliteCallback);
            CompliteCallback = null;

            Mapping titleMapping;
            Mapping levelMapping;
            Mapping parentMapping;
            IEnumerable<string> mapping;
            IEnumerable<string> hierarchical;
            try
            {
                Database.Wait(Database.Connection);
                titleMapping = (Mapping) _getMappingMethodInfo.Invoke(Database, new object[] {objects[0]});
                levelMapping = (Mapping) _getMappingMethodInfo.Invoke(Database, new object[] {objects[1]});
                parentMapping = (Mapping) _getMappingMethodInfo.Invoke(Database, new object[] {objects[2]});
                mapping = Database.GetList(Database.MappingTable, Database.TableNameColumn)
                    .Select(item => item.ToString());
                hierarchical = Database.GetList(Database.HierarchicalTable, Database.TableNameColumn)
                    .Select(item => item.ToString());
            }
            finally
            {
                // Whether or not the exception was thrown, the current 
                // thread owns the mutex, and must release it. 
                Database.Release(Database.Connection);
            }

            Debug.Assert(mapping.Contains(TableName.ToString()));
            var dictionary = new Dictionary<object, string>();

            Total += items.Count();
            string[] enumerable = hierarchical as string[] ?? hierarchical.ToArray();
            bool contains = enumerable.Contains(TableName);
            foreach (object key in items)
            {
                object i = key;
                var list = new StackListQueue<string>();
                do
                {
                    if (titleMapping.ContainsKey(i)) list.Add(titleMapping[i].ToString());
                } while
                    (
                    contains &&
                    levelMapping.ContainsKey(i) && MyDatabase.Database.ConvertTo<long>(levelMapping[i]) > 1 &&
                    parentMapping.ContainsKey(i) && (i = parentMapping[i]) != null &&
                    !string.IsNullOrWhiteSpace(i.ToString())
                    );
                string value = string.Join("::", list);
                dictionary.Add(key, value);
                if (ProgressCallback != null) ProgressCallback(++Current, Total);
            }

            CompliteCallback = (CompliteCallback) stack.Pop();


            if (CompliteCallback != null) CompliteCallback();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            return dictionary;
        }

        private KeyValuePair<StackListQueue<object>, StackListQueue<object>> GetAllChildren(
            KeyValuePair<object, object> pair,
            KeyValuePair<Dictionary<object, StackListQueue<object>>, Dictionary<object, StackListQueue<object>>>
                childrenPair)
        {
            var list = new StackListQueue<object> {pair.Key};
            for (int i = 0; i < list.Count(); i++)
            {
                if (childrenPair.Key.ContainsKey(list[i]))
                    list.AddRange(childrenPair.Key[list[i]]);
            }
            var list1 = new StackListQueue<object> {pair.Value};
            for (int i = 0; i < list1.Count(); i++)
            {
                if (childrenPair.Value.ContainsKey(list1[i]))
                    list1.AddRange(childrenPair.Value[list1[i]]);
            }
            return new KeyValuePair<StackListQueue<object>, StackListQueue<object>>(list, list1);
        }

        public void BuildMappingData()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var stack = new Stack<object>();
            stack.Push(CompliteCallback);
            CompliteCallback = null;


            object[][] objects1 =
            {
                new object[]
                {
                    string.Format("{0}", TableName),
                    MinLevel,
                    MaxLevel
                },
                new[]
                {
                    string.Format("{0}", TableName),
                    SiteMinLevel,
                    SiteMaxLevel,
                    SiteId
                }
            };
            Mapping[] items;
            try
            {
                Database.Wait(Database.Connection);
                items = objects1.Select(obj => (Mapping) _getMappingMethodInfo.Invoke(Database, new object[] {obj}))
                    .ToArray();
            }
            finally
            {
                // Whether or not the exception was thrown, the current 
                // thread owns the mutex, and must release it. 
                Database.Release(Database.Connection);
            }

            IndexData =
                new KeyValuePair<Dictionary<object, string>, Dictionary<object, string>>(
                    items[0].ToDictionary(pair => pair.Key,
                        pair => String.ToTitleCase(String.NormalizeAddress(pair.Value.ToString().ToLower()))),
                    items[1].ToDictionary(pair => pair.Key,
                        pair => String.ToTitleCase(String.NormalizeAddress(pair.Value.ToString().ToLower()))));

            object[][][] objects2 =
            {
                new[]
                {
                    new object[]
                    {
                        string.Format("{0}", TableName),
                        string.Format("{0}{1}", TableName, Database.IdColumn),
                        string.Format("{0}{1}", TableName, Database.TitleColumn)
                    },
                    new object[]
                    {
                        string.Format("{0}", TableName),
                        string.Format("{0}{1}", TableName, Database.IdColumn),
                        Database.LevelColumn
                    },
                    new object[]
                    {
                        string.Format("{0}", TableName),
                        string.Format("{0}{1}", TableName, Database.IdColumn),
                        Database.ParentIdColumn
                    }
                },
                new[]
                {
                    new[]
                    {
                        string.Format("{0}{1}", Database.SiteTable, TableName),
                        string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.IdColumn),
                        string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.TitleColumn),
                        SiteId
                    },
                    new[]
                    {
                        string.Format("{0}{1}", Database.SiteTable, TableName),
                        string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.IdColumn),
                        Database.LevelColumn,
                        SiteId
                    },
                    new[]
                    {
                        string.Format("{0}{1}", Database.SiteTable, TableName),
                        string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.IdColumn),
                        Database.ParentIdColumn,
                        SiteId
                    }
                }
            };

            Dictionary<object, string>[] titles =
                objects2.Select((x, i) => BuildFullTitle(items[i].Keys.ToArray(), x)).ToArray();

            TitleData =
                new KeyValuePair<Dictionary<object, string>, Dictionary<object, string>>(titles[0], titles[1]);

            CompliteCallback = (CompliteCallback) stack.Pop();
            if (CompliteCallback != null) CompliteCallback();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void BuildMapping()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var stack = new Stack<object>();
            stack.Push(CompliteCallback);
            CompliteCallback = null;


            Dictionary<object, string> key = IndexData.Key;
            Dictionary<object, string> value = IndexData.Value;

            var data =
                new KeyValuePair<StackListQueue<object>, StackListQueue<object>>(new StackListQueue<object> {key.Keys},
                    new StackListQueue<object> {value.Keys});
            data.Key.Sort(ObjectComparer);
            data.Value.Sort(ObjectComparer);

            var
                titles = new KeyValuePair<
                    Dictionary<object, KeyValuePair<object, string>>,
                    Dictionary<object, KeyValuePair<object, string>>>(
                    TitleData.Key.ToDictionary(
                        item => item.Key, item => item, ObjectComparer),
                    TitleData.Value.ToDictionary(item => item.Key, item => item, ObjectComparer));

            GridItems.Clear();

            var progress = new object();
            var stackListQueue = new StackListQueue<KeyValuePair<StackListQueue<object>, StackListQueue<object>>>();

            if (MinLevel > 0 && SiteMinLevel > 0)
            {
                Mapping mapping = Database.GetMapping(TableName.ToString(), MinLevel - 1, MinLevel - 1, SiteMinLevel - 1,
                    SiteMinLevel - 1,
                    SiteId);

                Debug.WriteLine("mapping.Count {0}", mapping.Count);
                Total += mapping.Count;


                object[][] objects =
                {
                    new object[]
                    {
                        string.Format("{0}", TableName),
                        string.Format("{0}{1}", TableName, Database.IdColumn),
                        Database.ParentIdColumn,
                        MinLevel,
                        MaxLevel
                    },
                    new[]
                    {
                        string.Format("{0}{1}", Database.SiteTable, TableName),
                        string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.IdColumn),
                        Database.ParentIdColumn,
                        SiteMinLevel,
                        SiteMaxLevel,
                        SiteId
                    }
                };

                Mapping[] parentMappings =
                    objects.Select(obj => (Mapping) _getMappingMethodInfo.Invoke(Database, new object[] {obj}))
                        .ToArray();

                lock (progress) Total += parentMappings[0].Count + parentMappings[1].Count;
                lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);

                Dictionary<object, StackListQueue<object>>[] children =
                    parentMappings.Select(
                        parentMapping =>
                            parentMapping.Values.Distinct()
                                .ToDictionary(item => item,
                                    item =>
                                        new StackListQueue<object>
                                        {
                                            parentMapping.Where(pair => ObjectComparer.Equals(pair.Value, item))
                                                .Select(pair => pair.Key)
                                        }, ObjectComparer)).ToArray();
                var childrenPair =
                    new KeyValuePair
                        <Dictionary<object, StackListQueue<object>>, Dictionary<object, StackListQueue<object>>>(
                        children[0],
                        children[1]);

                lock (progress) Current += parentMappings[0].Count + parentMappings[1].Count;
                lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);

                Parallel.ForEach(mapping, map =>
                {
                    KeyValuePair<StackListQueue<object>, StackListQueue<object>> pair = GetAllChildren(map, childrenPair);
                    if (pair.Key.Any() && pair.Value.Any())
                    {
                        lock (progress) Total += pair.Key.Count + pair.Value.Count;
                        lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);

                        pair.Key.Sort(ObjectComparer);
                        pair.Value.Sort(ObjectComparer);

                        var keyValuePair =
                            new KeyValuePair<StackListQueue<object>, StackListQueue<object>>(
                                (StackListQueue<object>)
                                    (new SortedStackListQueue<object>(pair.Key) {Comparer = ObjectComparer}).Intersect(
                                        data.Key),
                                (StackListQueue<object>)
                                    (new SortedStackListQueue<object>(pair.Value) {Comparer = ObjectComparer}).Intersect
                                        (data.Value));

                        lock (progress) Current += pair.Key.Count + pair.Value.Count;
                        lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);

                        lock (stackListQueue) stackListQueue.Enqueue(keyValuePair);

                        lock (progress) Total += keyValuePair.Key.Count + keyValuePair.Value.Count;
                        lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);
                    }

                    lock (progress) if (ProgressCallback != null) ProgressCallback(++Current, Total);
                });
            }
            else
            {
                stackListQueue.Enqueue(data);
                Total += data.Key.Count + data.Value.Count;
                if (ProgressCallback != null) ProgressCallback(Current, Total);
            }

            var gridItems = new object();

            Total += stackListQueue.Count;

            Parallel.ForEach(stackListQueue, keyValuePair =>
            {
                List<string> list = keyValuePair.Key.Select(item => key[item]).ToList();
                list.AddRange(keyValuePair.Value.Select(item => value[item]).ToList());
                Dictionary<string, KeyValuePair<StackListQueue<object>, StackListQueue<object>>> map =
                    list.Distinct().ToDictionary(name => name,
                        name =>
                            new KeyValuePair<StackListQueue<object>, StackListQueue<object>>(
                                new StackListQueue<object>
                                {
                                    keyValuePair.Key.Where(
                                        item =>
                                            string.Compare(key[item], name, StringComparison.OrdinalIgnoreCase) ==
                                            0)
                                },
                                new StackListQueue<object>
                                {
                                    keyValuePair.Value.Where(
                                        item =>
                                            string.Compare(value[item], name, StringComparison.OrdinalIgnoreCase) ==
                                            0)
                                }));

                lock (progress) Current += keyValuePair.Key.Count + keyValuePair.Value.Count;
                lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);

                IEnumerable<string> bad =
                    (from item in map where item.Value.Value.Count == 0 select item.Key);
                IEnumerable<string> good =
                    (from item in map where item.Value.Value.Count > 0 select item.Key);

                lock (progress) Total += bad.Count()*good.Count();
                lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);

                Dictionary<string, string> dictionary = bad.ToDictionary(s => s,
                    s => LevenshteinDistance.FindNeighbour(s, good, MaxDistance));
                foreach (
                    var pair in
                        dictionary.Where(pair => !string.IsNullOrWhiteSpace(pair.Key) && !string.IsNullOrWhiteSpace(pair.Value)))
                {
                    map[pair.Key] = new KeyValuePair<StackListQueue<object>, StackListQueue<object>>(map[pair.Key].Key,
                        map[pair.Value].Value);
                }

                lock (progress) Current += bad.Count()*good.Count();
                lock (progress) if (ProgressCallback != null) ProgressCallback(Current, Total);

                lock (gridItems)
                    GridItems.AddRange(from pair in map.Values
                        from keyIndex in pair.Key
                        from valueIndex in pair.Value
                        select new GridItem
                        {
                            Key = titles.Key[keyIndex],
                            Value = titles.Value[valueIndex]
                        });

                lock (progress) if (ProgressCallback != null) ProgressCallback(++Current, Total);
            });


            CompliteCallback = (CompliteCallback) stack.Pop();
            if (CompliteCallback != null) CompliteCallback();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void ExecuteNonQuery()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var stack = new Stack<object>();
            stack.Push(CompliteCallback);
            CompliteCallback = null;


            string[] commandStrings = CommandText.Split(';');

            Total += commandStrings.Length;
            Database.Connection.Open();
            foreach (string commandString in commandStrings)
                using (SQLiteCommand command = Database.Connection.CreateCommand())
                {
                    string commandText = commandString.Trim();
                    if (!string.IsNullOrWhiteSpace(commandText))
                    {
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }
                    if (ProgressCallback != null) ProgressCallback(++Current, Total);
                }
            Database.Connection.Close();

            CompliteCallback = (CompliteCallback) stack.Pop();
            if (CompliteCallback != null) CompliteCallback();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public void DownloadTable()
        {
            Debug.WriteLine("Begin {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
            var stack = new Stack<object>();
            stack.Push(CompliteCallback);
            CompliteCallback = null;

            AppendLineCallback(string.Format("CREATE TABLE IF NOT EXISTS {0}{1}(", Database.SiteTable,
                TableName));
            AppendLineCallback(string.Format("{0}{1} INTEGER,", Database.SiteTable, Database.IdColumn));
            AppendLineCallback(string.Format("{0}{1}{2} VARCHAR,", Database.SiteTable, TableName,
                Database.IdColumn));
            AppendLineCallback(string.Format("{0}{1}{2} VARCHAR,", Database.SiteTable, TableName,
                Database.TitleColumn));
            AppendLineCallback(string.Format("{0} VARCHAR,", Database.ParentIdColumn));
            AppendLineCallback(string.Format("{0} INTEGER,", Database.LevelColumn));
            AppendLineCallback(string.Format("PRIMARY KEY({0}{2},{0}{1}{2}));", Database.SiteTable,
                TableName,
                Database.IdColumn));
            AppendLineCallback("BEGIN;");

            string insertOrReplaceString =
                string.Format(
                    "INSERT OR REPLACE INTO {0}{1}({0}{2},{0}{1}{2},{0}{1}{3},{4},{5}) VALUES ({{{{{0}{2}}}}},'{{{{{0}{1}{2}}}}}','{{{{{0}{1}{3}}}}}','{{{{{4}}}}}',{{{{{5}}}}});",
                    Database.SiteTable, TableName, Database.IdColumn, Database.TitleColumn,
                    Database.ParentIdColumn, Database.LevelColumn);

            string siteIdPattern =
                string.Format("{0}{1}", Database.SiteTable, Database.IdColumn);
            string siteTableIdPattern =
                string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.IdColumn);
            string siteTableTitlePattern =
                string.Format("{0}{1}{2}", Database.SiteTable, TableName, Database.TitleColumn);
            string parentIdPattern = string.Format("{0}", Database.ParentIdColumn);
            string levelPattern = string.Format("{0}", Database.LevelColumn);

            SiteProperties siteProperties = Database.GetSiteProperties(SiteId);
            BuilderInfos builderInfos = Database.GetBuilderInfos(SiteId);
            BuilderInfo builderInfo = builderInfos[TableName.ToString()];
            Crawler.Method = builderInfo.Method.ToString();
            Crawler.Encoding = siteProperties.Encoding.ToString();
            Crawler.Compression = siteProperties.CompressionClassName.ToString();
            ReturnFieldInfos = Database.GetReturnFieldInfos(SiteId);

            var baseValues = new Values
            {
                Table = new StackListQueue<string> {TableName.ToString()},
            };

            string[] urlTemplates = builderInfo.UrlTemplate.ToString().Split(Parser.SplitChar);
            string urlTemplate = (urlTemplates != null && urlTemplates.Length > 1)
                ? urlTemplates[1]
                : builderInfo.UrlTemplate.ToString();


            baseValues.Url = Transformation.ParseTemplate(urlTemplate, baseValues);

            var baseBuilder = new UriBuilder(String.Parse(baseValues.Url))
            {
                UserName = siteProperties.UserName.ToString(),
                Password = siteProperties.Password.ToString(),
            };
            Debug.WriteLine("baseBuilder {0}", baseBuilder);

            MatchCollection matches = Regex.Matches(builderInfo.IdTemplate.ToString(),
                Transformation.FieldPattern);

            string[] flags = builderInfo.Flags.ToString().Split(Parser.SplitChar);

            var stackListQueue = new StackListQueue<KeyValuePair<int, Values>>
            {
                new KeyValuePair<int, Values>(1, baseValues),
            };
            Total++;

            while (stackListQueue.Any())
            {
                KeyValuePair<int, Values> dequeue = stackListQueue.Dequeue();
                Values parentValues = dequeue.Value;
                int currentLevel = dequeue.Key;
                Match option = matches[currentLevel - 1];
                string optionValue = option.Groups[Transformation.NameGroup].Value;

                int parentCount = parentValues.MaxCount;

                parentValues.Option = Enumerable.Repeat(optionValue, parentCount).ToList();
                Debug.WriteLine("parentValues {0}:{1}", currentLevel, parentValues);

                string[] keyXPathTemplates = builderInfo.KeyXPathTemplate.ToString().Split(Parser.SplitChar);
                string[] valueXPathTemplates = builderInfo.ValueXPathTemplate.ToString().Split(Parser.SplitChar);
                string[] keyResultTemplates = builderInfo.KeyResultTemplate.ToString().Split(Parser.SplitChar);
                string[] valueResultTemplates = builderInfo.ValueResultTemplate.ToString().Split(Parser.SplitChar);
                string[] keyRegexPatterns = Regex.Split(builderInfo.KeyRegexPattern.ToString(),
                    MyLibrary.Types.Regex.Escape(
                        MyLibrary.Types.Regex.Escape(Parser.SplitChar.ToString(CultureInfo.InvariantCulture))));
                string[] valueRegexPatterns = Regex.Split(builderInfo.ValueRegexPattern.ToString(),
                    MyLibrary.Types.Regex.Escape(
                        MyLibrary.Types.Regex.Escape(Parser.SplitChar.ToString(CultureInfo.InvariantCulture))));
                string[] keyRegexReplacements = Regex.Split(builderInfo.KeyRegexReplacement.ToString(),
                    MyLibrary.Types.Regex.Escape(
                        MyLibrary.Types.Regex.Escape(Parser.SplitChar.ToString(CultureInfo.InvariantCulture))));
                string[] valueRegexReplacements = Regex.Split(builderInfo.ValueRegexReplacement.ToString(),
                    MyLibrary.Types.Regex.Escape(
                        MyLibrary.Types.Regex.Escape(Parser.SplitChar.ToString(CultureInfo.InvariantCulture))));

                string keyXPathTemplate = keyXPathTemplates[Math.Min(currentLevel, keyXPathTemplates.Length - 1)];
                string valueXPathTemplate = valueXPathTemplates[Math.Min(currentLevel, valueXPathTemplates.Length - 1)];
                string keyResultTemplate = keyResultTemplates[Math.Min(currentLevel, keyResultTemplates.Length - 1)];
                string valueResultTemplate =
                    valueResultTemplates[Math.Min(currentLevel, valueResultTemplates.Length - 1)];
                string keyRegexPattern = keyRegexPatterns[Math.Min(currentLevel, keyRegexPatterns.Length - 1)];
                string valueRegexPattern = valueRegexPatterns[Math.Min(currentLevel, valueRegexPatterns.Length - 1)];
                string keyRegexReplacement =
                    keyRegexReplacements[Math.Min(currentLevel, keyRegexReplacements.Length - 1)];
                string valueRegexReplacement =
                    valueRegexReplacements[Math.Min(currentLevel, valueRegexReplacements.Length - 1)];

                var returnFieldInfos = new ReturnFieldInfos();

                foreach (
                    string key in
                        ReturnFieldInfos.GetType().GetProperties()
                            .Select(propertyInfo => propertyInfo.Name)
                            .Where(key => ReturnFieldInfos.ContainsKey(key)))
                    returnFieldInfos.Add(key, ReturnFieldInfos[key]);

                foreach (var pair in new Dictionary<string, string[]>
                {
                    {ReturnValue, new[] {keyXPathTemplate, keyResultTemplate, keyRegexPattern, keyRegexReplacement}},
                    {
                        ReturnTitle,
                        new[] {valueXPathTemplate, valueResultTemplate, valueRegexPattern, valueRegexReplacement}
                    }
                })
                    returnFieldInfos.Add(
                        new ReturnFieldInfo
                        {
                            SiteId = SiteId,
                            ReturnFieldId = pair.Key,
                            ReturnFieldXpathTemplate = pair.Value[0],
                            ReturnFieldResultTemplate = pair.Value[1],
                            ReturnFieldRegexPattern = pair.Value[2],
                            ReturnFieldRegexReplacement = pair.Value[3],
                        });

                Debug.WriteLine("returnFieldInfos {0}:{1}", currentLevel, returnFieldInfos);

                List<string> parentIds =
                    Transformation.ParseTemplate(builderInfo.IdTemplate.ToString(), new Values
                    {
                        parentValues.Where(pair => string.CompareOrdinal(pair.Key, optionValue) != 0)
                    })
                        .Select(s => s.ToLower())
                        .ToList();

                for (int i = 0; i < parentCount; i++)
                {
                    Values slice = parentValues.Slice(i);
                    Uri uri = MyLibrary.Types.Uri.Combine(baseBuilder.Uri.ToString(), slice.Url.First());
                    slice.Url = new StackListQueue<string> {uri.ToString()};

                    string[] requestTemplates = builderInfo.RequestTemplate.ToString().Split(Parser.SplitChar);
                    string requestTemplate = requestTemplates[Math.Min(currentLevel, requestTemplates.Length - 1)];
                    Crawler.Request = String.Parse(Transformation.ParseTemplate(requestTemplate, slice));

                    IEnumerable<HtmlDocument> documents =
                        Crawler.WebRequestHtmlDocument(uri, new WebSession());

                    ReturnFields returnFields = Parser.BuildReturnFields(documents,
                        slice, returnFieldInfos);

                    returnFields.Option =
                        returnFields.Option.Select(
                            seed =>
                                OptionPatches.Aggregate(seed,
                                    (current, patch) =>
                                        new Regex(patch.Key, RegexOptions.IgnoreCase).Replace(current, patch.Value)));
                    returnFields.Value =
                        returnFields.Value.Select(
                            seed =>
                                ValuePatches.Aggregate(seed,
                                    (current, patch) =>
                                        new Regex(patch.Key, RegexOptions.IgnoreCase).Replace(current, patch.Value)));
                    Debug.WriteLine("returnFields {0}:{1}:{2}", currentLevel, i, returnFields);

                    List<string> options = returnFields.Value.ToList();
                    List<string> titles = returnFields.Title.ToList();
                    List<string> optionRedirect = returnFields.OptionRedirect.ToList();
                    List<string> valueRedirect = returnFields.ValueRedirect.ToList();

                    int keyCount = Math.Min(options.Count, titles.Count);
                    if (keyCount > 0)
                    {
                        List<string> currentOptions = options.GetRange(0, keyCount);
                        List<string> currentTitles = titles.GetRange(0, keyCount);
                        var currentValues = new Values();
                        if (string.CompareOrdinal(flags[currentLevel], @"?") == 0)
                        {
                            currentOptions.Add(parentIds[i].Split(Parser.SplitChar)[currentLevel - 1]);
                            foreach (var pair in slice.Where(pair => pair.Value.Any()))
                                currentValues.Add(pair.Key, Enumerable.Repeat(pair.Value.First(), keyCount + 1).ToList());
                        }
                        else
                        {
                            foreach (var pair in slice.Where(pair => pair.Value.Any()))
                                currentValues.Add(pair.Key, Enumerable.Repeat(pair.Value.First(), keyCount).ToList());
                        }

                        if (currentValues.ContainsKey(optionValue)) currentValues[optionValue] = currentOptions;
                        else currentValues.Add(optionValue, currentOptions);

                        List<string> ids = Transformation.ParseTemplate(builderInfo.IdTemplate.ToString(),
                            currentValues).Select(s => s.ToLower()).ToList();
                        var items = new Values
                        {
                            {
                                siteIdPattern,
                                Enumerable.Repeat(SiteId.ToString(), keyCount).ToList()
                            },
                            {siteTableIdPattern, ids.GetRange(0, keyCount)},
                            {siteTableTitlePattern, currentTitles},
                            {parentIdPattern, Enumerable.Repeat(parentIds[i], keyCount).ToList()},
                            {
                                levelPattern,
                                Enumerable.Repeat(currentLevel.ToString(CultureInfo.InvariantCulture),
                                    keyCount).ToList()
                            },
                        };

                        IEnumerable<string> commandTexts = Transformation.ParseTemplate(insertOrReplaceString, items);
                        Debug.Assert(commandTexts.Count() == keyCount);
                        Total += commandTexts.Count();
                        foreach (string commandText in commandTexts)
                        {
                            AppendLineCallback(commandText);
                            if (ProgressCallback != null) ProgressCallback(++Current, Total);
                        }

                        currentValues.Remove(string.Format("{0}", ReturnTitle));
                        if (currentLevel < matches.Count)
                        {
                            currentValues.Value = currentOptions;
                            currentValues.Title = new StackListQueue<string>();
                            string[] currentUrlTemplates = builderInfo.UrlTemplate.ToString().Split(Parser.SplitChar);
                            string currentUrlTemplate =
                                currentUrlTemplates[Math.Min(currentLevel + 1, currentUrlTemplates.Length - 1)];
                            currentValues.Url = Transformation.ParseTemplate(currentUrlTemplate, currentValues).ToList();
                            stackListQueue.Enqueue(new KeyValuePair<int, Values>(currentLevel + 1, currentValues));
                            Total += currentValues.MaxCount;
                        }
                    }
                    foreach (var redirect in new[] {optionRedirect, valueRedirect})
                    {
                        int countRedirect = redirect.Count;
                        if (countRedirect == 0) continue;
                        int optionCount = Math.Min(options.Count() - keyCount, countRedirect);
                        int titleCount = Math.Min(titles.Count() - keyCount, countRedirect);
                        List<string> redirectOptions = (optionCount > 0)
                            ? options.GetRange(keyCount, optionCount)
                            : new StackListQueue<string>();
                        List<string> redirectTitles = (titleCount > 0)
                            ? titles.GetRange(keyCount, titleCount)
                            : new StackListQueue<string>();
                        var redirectValues = new Values();
                        foreach (var pair in slice.Where(pair => pair.Value.Any()))
                            redirectValues.Add(pair.Key,
                                Enumerable.Repeat(pair.Value.First(), countRedirect).ToList());
                        if (redirectValues.ContainsKey(optionValue)) redirectValues[optionValue] = redirectOptions;
                        else redirectValues.Add(optionValue, redirectOptions);
                        redirectValues.Url = optionRedirect;
                        redirectValues.Value = redirectOptions;
                        redirectValues.Title = redirectTitles;
                        stackListQueue.Enqueue(new KeyValuePair<int, Values>(currentLevel, redirectValues));
                        Total += countRedirect;
                    }
                    if (ProgressCallback != null) ProgressCallback(++Current, Total);
                }
            }

            AppendLineCallback("COMMIT;");
            CompliteCallback = (CompliteCallback) stack.Pop();
            if (CompliteCallback != null) CompliteCallback();
            Debug.WriteLine("End {0}::{1}", GetType().Name, MethodBase.GetCurrentMethod().Name);
        }

        public static void ThreadProc(object obj)
        {
            var This = obj as Builder;
            object[] objects = {};
            if (This != null) This.MethodInfo.Invoke(This, objects);
        }

        public class GridItem
        {
            public KeyValuePair<object, string> Key { get; set; }
            public KeyValuePair<object, string> Value { get; set; }
        }
    }
}