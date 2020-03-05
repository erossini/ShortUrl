using PSC.ExtensionTypes;
using PSC.Shorturl.Web.Utility.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PSC.Shorturl.Web.Utility
{
    public class Statistics
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the param value
        /// </summary>
        /// <param name="hc">Context</param>
        /// <param name="paramName">Parameter name to find</param>
        /// <returns>Parameter value or empty</returns>
        public string CheckParameter(HttpContextBase hc, string paramName)
        {
            string rtn = "";

            if (!string.IsNullOrEmpty(hc.Request[paramName]))
            {
                rtn = hc.Request[paramName];
            }

            return rtn;
        }

        /// <summary>
        /// Verify the correct browser version
        /// </summary>
        /// <param name="userAgent"></param>
        /// <param name="browser"></param>
        /// <param name="platform"></param>
        /// <returns></returns>
        private string VerifyBrowserName(string userAgent, string browser, string platform)
        {
            if (platform == "Windows 10")
            {
                if (userAgent.IndexOf("Edge/") > 0)
                {
                    browser = "Microsoft Edge";
                }
            }
            else if(platform == "Android")
            {
                if (userAgent.IndexOf("Silk/") > 0)
                {
                    browser = "Amazon Silk";
                }
            }

            return browser;
        }

        /// <summary>
        /// Verify the correct browser version
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        private string VerifyBrowserVersion(string userAgent, string browser, string platform, string version)
        {
            if (platform == "Windows 10")
            {
                if (userAgent.IndexOf("Edge/") > 0)
                {
                    version = userAgent.SubstringBetween("Edge/", " ");
                }
            }

            return version;
        }

        /// <summary>
        /// Saves user detail
        /// </summary>
        /// <param name="hc"></param>
        /// <param name="trackElement"></param>
        /// <param name="trackElementType"></param>
        public StatDetail ExtractUserDetails(HttpContextBase hc, string trackElement = "", string trackElementType = "")
        {
            StatDetail ud = new StatDetail();

            ud.TrackElement = trackElement;
            ud.TrackElementType = trackElementType;

            HttpBrowserCapabilitiesBase browse = hc.Request.Browser;
            OSVersion os = new OSVersion();

            string platform = os.Platform(hc.Request.UserAgent);
            if (string.IsNullOrEmpty(platform))
            {
                platform = browse.Platform;
            }

            // detail about the user
            ud.BrowserName = VerifyBrowserName(hc.Request.UserAgent, browse.Browser, platform);
            ud.BrowserVersion = VerifyBrowserVersion(hc.Request.UserAgent, browse.Browser, platform, browse.Version);
            ud.Platform = platform;
            ud.IsMobile = browse.IsMobileDevice;
            ud.ISCrawler = browse.Crawler;
            ud.JScriptVersion = browse.JScriptVersion.ToString();
            ud.MobileDeviceModel = browse.MobileDeviceModel;
            ud.MobileDeviceManufacturer = browse.MobileDeviceManufacturer;
            ud.ScreenPixelsHeight = browse.ScreenPixelsHeight.ToString();
            ud.ScreenPixelsWidth = browse.ScreenPixelsWidth.ToString();
            ud.IP = hc.Request.UserHostAddress;
            ud.Url = hc.Request.Url.ToString();
            ud.userAgent = hc.Request.UserAgent;
            ud.UtmCampaign = CheckParameter(hc, "utm_campaign");
            ud.UtmContext = CheckParameter(hc, "utm_context");
            ud.UtmMedium = CheckParameter(hc, "utm_medium");
            ud.UtmSource = CheckParameter(hc, "utm_source");

            if (hc.Request.UrlReferrer != null)
            {
                ud.Referrer = hc.Request.UrlReferrer.ToString();
            }

            if (hc.Request.IsAuthenticated)
            {
                ud.Username = hc.User.Identity.Name.Trim();
            }

            return ud;
        }
    }
}