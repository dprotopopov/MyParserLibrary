using System;
using System.Drawing;
using MyLibrary;
using MyLibrary.Comparer;
using MyParser.Managers;
using MyParser.WebSessions;

namespace MyParser
{
    public static class Defaults
    {
        public static readonly Bitmap[] EmailIcons =
        {
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Email.png"))
        };

        public static readonly Bitmap[] WebSessionIcons =
        {
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Ready.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Running.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Finished.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Error.png"))
        };

        public static readonly Bitmap[] WebTaskIcons =
        {
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Ready.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Running.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Finished.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Paused.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Canceled.png")),
            new Bitmap(typeof (Defaults).Assembly.GetManifestResourceStream("MyParser.Error.png"))
        };

        public static bool UseRandomProxy
        {
            get { return false; }
        }

        public static int NumberOfTriesBeforeError
        {
            get { return 1; }
        }

        public static TimeSpan Timeout
        {
            get { return new TimeSpan(0, 0, 0, 10); }
        }

        public static int Edition
        {
            get { return (int) DocumentEdition.Original + (int) DocumentEdition.Tided; }
        }

        public static string Method
        {
            get { return "GET"; }
        }

        public static string Cookie
        {
            get { return ""; }
        }

        public static string Encoding
        {
            get { return "utf-8"; }
        }

        public static string Compression
        {
            get { return "NoCompression"; }
        }

        public static string ModuleClassname
        {
            get { return typeof (Defaults).Namespace; }
        }

        public static string ModuleNamespace
        {
            get { return typeof (Defaults).Namespace; }
        }

        public static ObjectComparer ObjectComparer
        {
            get { return new ObjectComparer(); }
        }

        public static Database Database
        {
            get { return new Database(); }
        }

        public static Transformation Transformation
        {
            get { return new Transformation(); }
        }

        public static WebSession WebSession
        {
            get { return new WebSession(); }
        }

        public static Converter Converter
        {
            get { return new Converter(); }
        }

        public static SessionManager SessionManager
        {
            get { return new SessionManager(); }
        }

        public static TaskManager TaskManager
        {
            get { return new TaskManager(); }
        }

        public static CompressionManager CompressionManager
        {
            get
            {
                return new CompressionManager
                {
                    ModuleNamespace = ModuleNamespace,
                };
            }
        }

        public static ComparerManager ComparerManager
        {
            get
            {
                return new ComparerManager
                {
                    ModuleNamespace = ModuleNamespace,
                };
            }
        }

        public static Parser Parser
        {
            get
            {
                return new Parser
                {
                    Transformation = Transformation
                };
            }
        }

        public static Crawler Crawler
        {
            get
            {
                return new Crawler
                {
                    Compression = Compression,
                    Encoding = Encoding,
                    CompressionManager = CompressionManager,
                    Database = Database,
                    Timeout = Timeout,
                    Edition = Edition,
                    SessionManager = SessionManager,
                };
            }
        }

        public static ReturnFieldInfos ReturnFieldInfos
        {
            get
            {
                return new ReturnFieldInfos
                {
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "Url",
                        ReturnFieldXpathTemplate = @"//a[@href]",
                        ReturnFieldResultTemplate = @"{{href}}",
                        ReturnFieldRegexPattern = @".+",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "Email",
                        ReturnFieldXpathTemplate = @"/",
                        ReturnFieldResultTemplate = @"{{InnerHtml}}",
                        ReturnFieldRegexPattern =
                            @"\b[a-zA-Z0-9]+([\.-][a-zA-Z0-9]+)*@([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}\b",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "Phone",
                        ReturnFieldXpathTemplate = @"/",
                        ReturnFieldResultTemplate = @"{{InnerHtml}}",
                        ReturnFieldRegexPattern = @"\b\(?\d{3}\)?-? *\d{3}-? *-?\d{4}\b",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "PublicationId",
                        ReturnFieldXpathTemplate = @"/",
                        ReturnFieldResultTemplate = @"{{InnerHtml}}",
                        ReturnFieldRegexPattern = @"\d+",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "PublicationLink",
                        ReturnFieldXpathTemplate = @"//a[@href]",
                        ReturnFieldResultTemplate = @"{{href}}",
                        ReturnFieldRegexPattern = @".+",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "PublicationDatetime",
                        ReturnFieldXpathTemplate = @"/",
                        ReturnFieldResultTemplate = @"{{InnerHtml}}",
                        ReturnFieldRegexPattern =
                            @"\d{1,2}(?<dot>[\.\/-])\d{1,2}\k{dot}\d{2}(\d{2})?(\s+\d{1,2}:\d{1,2}(:\d{1,2})?)?",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "OtherPageUrl",
                        ReturnFieldXpathTemplate = @"//a[contains(@href,'page')]",
                        ReturnFieldResultTemplate = @"{{href}}",
                        ReturnFieldRegexPattern = @".+",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "Option",
                        ReturnFieldXpathTemplate = @"//select/option",
                        ReturnFieldResultTemplate = @"{{value}}",
                        ReturnFieldRegexPattern = @".+",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                    new ReturnFieldInfo
                    {
                        ReturnFieldId = "Value",
                        ReturnFieldXpathTemplate = @"//select/option",
                        ReturnFieldResultTemplate = @"{{InnerText}}",
                        ReturnFieldRegexPattern = @".+",
                        ReturnFieldRegexReplacement = @"$&",
                    },
                };
            }
        }

        public static ReturnFields ReturnFields
        {
            get { return new ReturnFields(); }
        }
    }
}