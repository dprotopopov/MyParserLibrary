using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using MyLibrary.Collections;
using MyLibrary.LastError;
using MyParser.WebSessions;

namespace MyParser
{
    public class SessionManager : ISessionManager, ILastError
    {
        private readonly List<IWebSession> _webCrawlers = new StackListQueue<IWebSession>();

        private int _maxSessions;

        public SessionManager()
        {
            Mutex = new Mutex();
            _maxSessions = 256;
            Semaphore = new Semaphore(_maxSessions, _maxSessions);
        }

        public Semaphore Semaphore { get; set; }
        private Mutex Mutex { get; set; }
        public object LastError { get; set; }

        public int MaxSessions
        {
            set
            {
                Semaphore.WaitOne(_maxSessions);
                _maxSessions = value;
                Semaphore = new Semaphore(_maxSessions, _maxSessions);
            }
            get { return _maxSessions; }
        }

        public WebResponse GetResponse(IWebSession webSession, WebRequest webRequest)
        {
            try
            {
                webSession.WebRequest = webRequest;
                webSession.GetResponse();
                return webSession.WebResponse;
            }
            catch (WebException exception)
            {
                LastError = exception;
                throw;
            }
            catch (Exception exception)
            {
                LastError = exception;
                throw;
            }
        }

        public virtual IWebSession AddSession(IWebSession webSession)
        {
            _webCrawlers.Add(webSession);
            return webSession;
        }

        public virtual void RemoveSession(IWebSession webSession)
        {
            _webCrawlers.Remove(webSession);
        }

        public bool Wait(IWebSession webSession)
        {
            while (!Semaphore.WaitOne(0)) Thread.Sleep(1000);
            return true;
        }

        public bool Wait(IWebSession webSession, TimeSpan timeout)
        {
            return Semaphore.WaitOne(timeout);
        }

        public void Release(IWebSession webSession)
        {
            Semaphore.Release();
        }

        public virtual void Clear()
        {
            _webCrawlers.Clear();
            Semaphore = new Semaphore(_maxSessions, _maxSessions);
        }

        public bool Contains(IWebSession webSession)
        {
            return _webCrawlers.Contains(webSession);
        }


        public bool Wait(Semaphore semaphore)
        {
            while (!Mutex.WaitOne(0)) Thread.Sleep(1000);
            return true;
        }

        public bool Wait(Semaphore semaphore, TimeSpan timeout)
        {
            return Mutex.WaitOne(timeout);
        }

        public void Release(Semaphore semaphore)
        {
            Mutex.ReleaseMutex();
        }
    }
}