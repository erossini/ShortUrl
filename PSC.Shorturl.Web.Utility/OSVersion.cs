using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSC.Shorturl.Web.Utility
{
    public class OSVersion
    {
        public OSVersion()
        {
        }

        /// <summary>
        /// Returns client operating system
        /// </summary>
        /// <param name="userAgent">userAgent string from the context</param>
        /// <returns>Platform name or empty</returns>
        public string Platform(string userAgent)
        {
            string rtn = "";
            if (userAgent.IndexOf("Windows NT 10.0") > 0)
            {
                rtn = "Windows 10";
            }
            else if (userAgent.IndexOf("Windows NT 6.3") > 0)
            {
                rtn = "Windows 8.1";
            }
            else if (userAgent.IndexOf("Windows NT 6.2") > 0)
            {
                rtn = "Windows 8";
            }
            else if (userAgent.IndexOf("Windows NT 6.1") > 0)
            {
                rtn = "Windows 7";
            }
            else if (userAgent.IndexOf("Windows NT 6.0") > 0)
            {
                rtn = "Windows Vista";
            }
            else if (userAgent.IndexOf("Windows NT 5.2") > 0)
            {
                rtn = "Windows Server 2003; Windows XP x64 Edition";
            }
            else if (userAgent.IndexOf("Windows NT 5.1") > 0)
            {
                rtn = "Windows XP";
            }
            else if (userAgent.IndexOf("Windows NT 5.01") > 0)
            {
                rtn = "Windows 2000, Service Pack 1 (SP1)";
            }
            else if (userAgent.IndexOf("Windows NT 5.0") > 0)
            {
                rtn = "Windows 2000";
            }
            else if (userAgent.IndexOf("Windows NT 4.0") > 0)
            {
                rtn = "Microsoft Windows NT 4.0";
            }
            else if (userAgent.IndexOf("Win 9x 4.90") > 0)
            {
                rtn = "Windows Millennium Edition (Windows Me)";
            }
            else if (userAgent.IndexOf("Windows 98") > 0)
            {
                rtn = "Windows 98";
            }
            else if (userAgent.IndexOf("Windows 95") > 0)
            {
                rtn = "Windows 95";
            }
            else if (userAgent.IndexOf("Windows CE") > 0)
            {
                rtn = "Windows CE";
            }
            else if (userAgent.IndexOf("iPhone OS") > 0)
            {
                rtn = "iPhone OS";
            }
            else if (userAgent.IndexOf("Mac OS") > 0)
            {
                rtn = "Max OS";
            }
            else if (userAgent.IndexOf("Android") > 0)
            {
                rtn = "Android";
            }
            else if (userAgent.IndexOf("facebook") > 0)
            {
                rtn = "Facebook External Hit";
            }
            else if (userAgent.IndexOf("Twitterbot") > 0)
            {
                rtn = "Twitterbot";
            }
            else if (userAgent.IndexOf("WhatsApp") > 0)
            {
                rtn = "WhatsApp";
            }
            else
            {
                //Others
            }

            return rtn;
        }
    }
}