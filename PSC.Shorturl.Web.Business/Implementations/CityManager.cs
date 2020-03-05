using log4net;
using PSC.Shorturl.Web.Data;
using PSC.Shorturl.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Business.Implementations
{
    public class CityManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets a number from a IPv4
        /// </summary>
        /// <param name="Ip">IPv4 to convert in a number</param>
        /// <returns></returns>
        public decimal GenerateIpNumber(string Ip)
        {
            decimal ipnum = 0;

            // verifiy the IP is an IPv4 
            // IPv6 has :
            if (Ip.IndexOf(":") < 0)
            {
                string[] split = Ip.Split('.');
                ipnum = Convert.ToInt64(split[0]) * (256 * 256 * 256) + Convert.ToInt64(split[1]) * (256 * 256) + Convert.ToInt64(split[2]) * 256 + Convert.ToInt64(split[3]);
            }

            return ipnum;
        }

        /// <summary>
        /// Gets a country code from an IPv4
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns>Country code or an empty string</returns>
        public string GetCountryCode(string strIP)
        {
            string rtn = "";

            decimal tmp = GenerateIpNumber(strIP);
            if (tmp > 0)
            {
                using (var ctx = new ShorturlContext())
                {
                    IPCity city = ctx.IPCities.Where(c => c.IPFromNumber >= tmp && c.IPToNumber <= tmp).FirstOrDefault();
                    if (city != null)
                    {
                        rtn = city.CountryCode;
                    }
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets a country code from an IPv4
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns>Country code or an empty string</returns>
        public string GetLocation(string strIP)
        {
            string rtn = "";

            decimal tmp = GenerateIpNumber(strIP);
            if (tmp > 0)
            {
                using (var ctx = new ShorturlContext())
                {
                    IPCity city = ctx.IPCities.Where(c => c.IPFromNumber >= tmp && c.IPToNumber <= tmp).FirstOrDefault();
                    if (city != null)
                    {
                        rtn = city.City.Trim();
                    }
                }
            }

            return rtn;
        }
    }
}