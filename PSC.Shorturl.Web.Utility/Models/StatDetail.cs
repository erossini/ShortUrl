using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Utility.Models
{
    public class StatDetail
    {
        public int IDStat { get; set; }
        public string TrackElement { get; set; }
        public string TrackElementType { get; set; }
        public DateTime DateView { get; set; }
        public string userAgent { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string BrowserLanguage { get; set; }
        public string ConnectionSpeed { get; set; }
        public string Platform { get; set; }
        public string PlatformLanguage { get; set; }
        public string IP { get; set; }
        public string IPCountry { get; set; }
        public string IPLocation { get; set; }
        public bool ISCrawler { get; set; }
        public string JScriptVersion { get; set; }
        public bool IsMobile { get; set; }
        public string MobileDeviceModel { get; set; }
        public string MobileDeviceManufacturer { get; set; }
        public string ScreenPixelsHeight { get; set; }
        public string ScreenPixelsWidth { get; set; }
        public string Referrer { get; set; }
        public string QueryString { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
        public string UtmMedium { get; set; }
        public string UtmSource { get; set; }
        public string UtmCampaign { get; set; }
        public string UtmContext { get; set; }
    }
}
