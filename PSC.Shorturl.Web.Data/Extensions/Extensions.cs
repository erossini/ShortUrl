using PSC.Shorturl.Web.Data;
using PSC.Shorturl.Web.Entities;
using PSC.Shorturl.Web.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Data.Extensions
{
    public static class MyExtensions
    {
        public static Stat Populate(this StatDetail st)
        {
            Stat rtn = new Stat()
            {
                ClickDate = DateTime.Now,
                Ip = st.IP,
                Referer = !string.IsNullOrEmpty(st.Referrer) ? st.Referrer.ToString() : "",
                BrowserLanguage = string.IsNullOrEmpty(st.BrowserLanguage) ? "" : st.BrowserLanguage,
                BrowserName = string.IsNullOrEmpty(st.BrowserName) ? "" : st.BrowserName,
                BrowserVersion = string.IsNullOrEmpty(st.BrowserVersion) ? "" : st.BrowserVersion,
                ConnectionSpeed = string.IsNullOrEmpty(st.ConnectionSpeed) ? "" : st.ConnectionSpeed,
                IPCountry = string.IsNullOrEmpty(st.IPCountry) ? "" : st.IPCountry,
                IPLocation = string.IsNullOrEmpty(st.IPLocation) ? "" : st.IPLocation,
                ISCrawler = st.ISCrawler ? true : false,
                IsMobile = st.IsMobile ? true : false,
                JScriptVersion = string.IsNullOrEmpty(st.JScriptVersion) ? "" : st.JScriptVersion,
                MobileDeviceManufacturer = string.IsNullOrEmpty(st.MobileDeviceManufacturer) ? "" : st.MobileDeviceManufacturer,
                MobileDeviceModel = string.IsNullOrEmpty(st.MobileDeviceModel) ? "" : st.MobileDeviceModel,
                Platform = string.IsNullOrEmpty(st.Platform) ? "" : st.Platform,
                PlatformLanguage = string.IsNullOrEmpty(st.PlatformLanguage) ? "" : st.PlatformLanguage,
                QueryString = string.IsNullOrEmpty(st.QueryString) ? "" : st.QueryString,
                ScreenPixelsHeight = string.IsNullOrEmpty(st.ScreenPixelsHeight) ? "" : st.ScreenPixelsHeight,
                ScreenPixelsWidth = string.IsNullOrEmpty(st.ScreenPixelsWidth) ? "" : st.ScreenPixelsWidth,
                Username = st.Username,
                userAgent = string.IsNullOrEmpty(st.userAgent) ? "" : st.userAgent,
                UtmCampaign = string.IsNullOrEmpty(st.UtmCampaign) ? "" : st.UtmCampaign,
                UtmContext = string.IsNullOrEmpty(st.UtmContext) ? "" : st.UtmContext,
                UtmMedium = string.IsNullOrEmpty(st.UtmMedium) ? "" : st.UtmMedium,
                UtmSource = string.IsNullOrEmpty(st.UtmSource) ? "" : st.UtmSource
            };

            return rtn;
        }

        public static void SaveStatistic(this Stat st)
        {
            ShorturlContext ctx = new ShorturlContext();
            ctx.Stats.Add(st);
            ctx.SaveChanges();
        }
    }
}