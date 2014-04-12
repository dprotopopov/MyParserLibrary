using System;
using MyWebSimulator;

namespace MyAdvertisement.AdSense
{
    public class AdSenseSimulator : WebSimulator, IAdvertisement
    {
        private int _navigatingCountdown = 3;

        public new void DocumentLoadCompleted(params object[] parameters)
        {
            base.DocumentLoadCompleted(parameters);
            _navigatingCountdown--;
        }

        #region

        public void NavigateAdvertisement(object sender, EventArgs e)
        {
            if (_navigatingCountdown == 0)
            {
                //e.Cancel = true;
                //String url = e.Url.ToString();
                //Process.Start(url);
            }
        }

        public void ShowAdvertisement(object sender, EventArgs e)
        {
            Click(@"//*");
        }

        public void AdvertisementRefresh(object sender, EventArgs e)
        {
            _navigatingCountdown = 3;
        }

        #endregion
    }
}