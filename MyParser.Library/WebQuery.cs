using System.Diagnostics;

namespace MyParser.Library
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

        public virtual void OnQueryComplite(IWebTask webTask)
        {
            Debug.Assert(webTask == this);
            if (OnQueryCompliteCallback != null) OnQueryCompliteCallback(this);
        }
    }
}