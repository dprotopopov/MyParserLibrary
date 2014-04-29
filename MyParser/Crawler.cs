using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using HtmlAgilityPack;
using MyLibrary.Attribute;
using MyLibrary.Collections;
using MyLibrary.Lock;
using MyLibrary.Trace;
using MyParser.Compression;
using MyParser.Managers;
using MyParser.WebSessions;
using TidyManaged;

namespace MyParser
{
    public class Crawler : ITrace, ICrawler, IValueable, ISingleLock<bool>, ISingleLock<TimeSpan>
    {
        public Crawler()
        {
            Method = Defaults.Method;
            Encoding = Defaults.Encoding;
            Compression = Defaults.Compression;
            CompressionManager = Defaults.CompressionManager;
            Database = Defaults.Database;
            SessionManager = Defaults.SessionManager;
            UseRandomProxy = Defaults.UseRandomProxy;
            NumberOfTriesBeforeError = Defaults.NumberOfTriesBeforeError;
            Timeout = Defaults.Timeout;
            Edition = Defaults.Edition;
            SessionManager = Defaults.SessionManager;
            MutexBool = new Mutex();
            MutexTimeSpan = new Mutex();
        }

        public CompressionManager CompressionManager { get; set; }
        private Mutex MutexBool { get; set; }
        private Mutex MutexTimeSpan { get; set; }
        public IDatabase Database { get; set; }
        public int Edition { get; set; }
        public string Method { get; set; }
        public string Request { get; set; }
        public string Encoding { get; set; }
        public string Compression { get; set; }
        public bool UseRandomProxy { get; set; }
        public int NumberOfTriesBeforeError { get; set; }
        public ISessionManager SessionManager { get; set; }
        public TimeSpan Timeout { get; set; }

        /// <summary>
        ///     Запрос к сайту с использованием RT.Crawler
        /// </summary>
        public StackListQueue<HtmlDocument> WebRequestHtmlDocument(Uri uri, IWebSession webSession)
        {
            long current = 0;
            long total = 1;
            var collection = new StackListQueue<HtmlDocument>();
            ICompression compression = CompressionManager.CreateCompression(Compression);
            Encoding encoder = System.Text.Encoding.GetEncoding(Encoding);
            var memoryStreams = new StackListQueue<MemoryStream>();

            Debug.Assert(NumberOfTriesBeforeError > 0);
            for (int i = 0; i < NumberOfTriesBeforeError && !memoryStreams.Any(); i++)
                try
                {
                    var httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
                    httpWebRequest.CookieContainer = new CookieContainer();
                    httpWebRequest.AutomaticDecompression = DecompressionMethods.None;
                    httpWebRequest.ContentType = string.Format(@"text/html; charset={0}", Encoding);
                    httpWebRequest.Referer = @"http://yandex.ru";
                    httpWebRequest.Accept = @"text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    httpWebRequest.UserAgent =
                        @"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";
                    httpWebRequest.KeepAlive = true;

                    if (string.Compare(Method, "JSON", StringComparison.Ordinal) == 0)
                    {
                        httpWebRequest.ContentType = "application/json; charset=utf-8";
                        httpWebRequest.Accept = "application/json, text/javascript, */*";
                    }

                    if (string.Compare(Method, "JSON", StringComparison.Ordinal) == 0 ||
                        string.Compare(Method, "POST", StringComparison.Ordinal) == 0)
                    {
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            Debug.WriteLine("Request: {0}", Request);
                            streamWriter.Write(Request);
                            streamWriter.Flush();
                        }
                    }
                    else
                        httpWebRequest.Method = Method;

                    httpWebRequest.Timeout = (int) Timeout.TotalMilliseconds;
                    if (UseRandomProxy)
                        try
                        {
                            Database.Wait(Database.Connection);
                            Proxy proxy = Database.GetNextProxy();
                            httpWebRequest.Proxy = proxy.ToWebProxy();
                        }
                        catch (Exception exception)
                        {
                            if (AppendLineCallback != null) AppendLineCallback(exception.ToString());
                        }
                        finally
                        {
                            // Whether or not the exception was thrown, the current 
                            // thread owns the mutex, and must release it. 
                            Database.Release(Database.Connection);
                        }
                    try
                    {
                        SessionManager.Wait(webSession);
                        SessionManager.AddSession(webSession);
                        WebResponse responce = SessionManager.GetResponse(webSession, httpWebRequest);
                        Stream responceStream = responce.GetResponseStream();
                        if (responceStream == null) throw new Exception();
                        memoryStreams.Add(new MemoryStream());
                        compression.Decompress(responceStream, memoryStreams.Last());
                        memoryStreams.Last().Seek(0, SeekOrigin.Begin);
                        var decodedReader = new StreamReader(memoryStreams.Last(), encoder);
                        memoryStreams.Add(
                            new MemoryStream(System.Text.Encoding.Default.GetBytes(decodedReader.ReadToEnd())));
                        memoryStreams.Dequeue();
                    }
                    finally
                    {
                        SessionManager.RemoveSession(webSession);
                        SessionManager.Release(webSession);
                    }
                }
                catch (AbandonedMutexException exception)
                {
                    if (AppendLineCallback != null) AppendLineCallback(exception.ToString());
                }
                catch (WebException exception)
                {
                    if (AppendLineCallback != null) AppendLineCallback(exception.ToString());
                }
                catch (Exception exception)
                {
                    if (AppendLineCallback != null) AppendLineCallback(exception.ToString());
                }

            if (!memoryStreams.Any()) throw new FileNotFoundException();
            if (ProgressCallback != null) ProgressCallback(++current, ++total);

            memoryStreams.First().Seek(0, SeekOrigin.Begin);
            if ((Edition & (int) DocumentEdition.Tided) != 0)
            {
                Document tidy = Document.FromStream(memoryStreams.First());

                tidy.ForceOutput = true;
                tidy.PreserveEntities = true;
                tidy.InputCharacterEncoding = EncodingType.Raw;
                tidy.OutputCharacterEncoding = EncodingType.Raw;
                tidy.CharacterEncoding = EncodingType.Raw;
                tidy.ShowWarnings = false;
                tidy.Quiet = true;
                tidy.OutputXhtml = true;
                tidy.CleanAndRepair();

                memoryStreams.Add(new MemoryStream());
                tidy.Save(memoryStreams.Last());
                if (ProgressCallback != null) ProgressCallback(++current, ++total);
            }
            if ((Edition & (int) DocumentEdition.Original) == 0)
                memoryStreams.Dequeue();

            foreach (MemoryStream stream in memoryStreams)
            {
                stream.Seek(0, SeekOrigin.Begin);
                var edition = new HtmlDocument();
                edition.Load(stream, System.Text.Encoding.Default);
                collection.Add(edition);
            }
            if (ProgressCallback != null) ProgressCallback(++current, total);

            if (CompliteCallback != null) CompliteCallback();
            return collection;
        }

        public bool Wait(bool semaphore)
        {
            while (!MutexBool.WaitOne(0)) Thread.Sleep(1000);
            return true;
        }

        public bool Wait(bool semaphore, TimeSpan timeout)
        {
            return MutexBool.WaitOne(timeout);
        }

        public void Release(bool semaphore)
        {
            MutexBool.ReleaseMutex();
        }

        public bool Wait(TimeSpan semaphore)
        {
            while (!MutexTimeSpan.WaitOne(0)) Thread.Sleep(1000);
            return true;
        }

        public bool Wait(TimeSpan semaphore, TimeSpan timeout)
        {
            return MutexTimeSpan.WaitOne(timeout);
        }

        public void Release(TimeSpan semaphore)
        {
            MutexTimeSpan.ReleaseMutex();
        }

        public ProgressCallback ProgressCallback { get; set; }
        public AppendLineCallback AppendLineCallback { get; set; }
        public CompliteCallback CompliteCallback { get; set; }

        public Values ToValues()
        {
            return new Values(this);
        }
    }
}