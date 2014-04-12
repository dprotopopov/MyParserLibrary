using System;

namespace MyAdvertisement
{
    public interface IAdvertisement
    {
        void NavigateAdvertisement(object sender, EventArgs e);
        void AdvertisementRefresh(object sender, EventArgs e);
        void ShowAdvertisement(object sender, EventArgs e);
    }
}