using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyParser.ProxyTasks;
using MyParser.WebTasks;

namespace MyParserLibrary.UnitTest
{
    [TestClass]
    public class ProxyListTaskTest
    {
        void WebTaskCallback(IWebTask webTask)
        {
            Console.WriteLine(webTask.ToValues());
            if(webTask.ReturnFields!=null) Console.WriteLine(webTask.ReturnFields.ToValues());
        }
        [TestMethod]
        public void TestHidemyAssProxyListTask()
        {
            var task = new HidemyAssProxyListTask()
            {
                OnAbortCallback = WebTaskCallback,
                OnCompliteCallback =  WebTaskCallback,
                OnErrorCallback =  WebTaskCallback,
                OnResumeCallback =  WebTaskCallback,
                OnStartCallback = WebTaskCallback,
                OnSuspendCallback = WebTaskCallback,
            };
            task.Start();
            task.Thread.Join();
        }}
}
