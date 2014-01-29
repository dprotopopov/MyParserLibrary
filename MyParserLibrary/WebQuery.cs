using System.Diagnostics;

namespace MyParserLibrary
{
    public class WebQuery : WebTask, IWebQuery
    {
        public WebQuery()
        {
            OnCompliteCallback = OnQueryComplite;
        }

        public WebQueryCallback OnQueryCompliteCallback { get; set; }

        public void Query()
        {
            Start();
        }

        public virtual void OnQueryComplite(WebTask task)
        {
            Debug.Assert(task == this);
            if (OnQueryCompliteCallback != null) OnQueryCompliteCallback(this);
        }
    }
}