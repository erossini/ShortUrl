using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using PSC.ExtensionTypes;
using PSC.ShortUrl.Web.IPToCountry.Models;
using PSC.Shorturl.Web.Data;
using PSC.Shorturl.Web.Entities;

namespace PSC.Shorturl.Web.IPToCountry
{
    public class IPQuery : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IPCity GetData(string Ip)
        {
            IPCity rtn = new IPCity();

            string[] local = { "127.0.0.1", "::1 "};
            if (local.Contains(Ip))
            {
                rtn.City = "";
                rtn.CountryCode = "";
            }
            else
            {
                using (var ctx = new ShorturlContext())
                {
                    decimal tmp = Ip.IPToNumber();
                    rtn = ctx.IPCities.Where(c => c.IPFromNumber >= tmp && c.IPToNumber <= tmp).FirstOrDefault();
                    if (rtn == null)
                    {
                        APIResponseModel model = GetIPFromAPI(Ip);
                        if (model.Success)
                        {
                            rtn = SaveIP(model);
                        }
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
        public string GetCountryCode(string strIP)
        {
            return GetData(strIP).CountryCode;
        }

        /// <summary>
        /// Gets a country code from an IPv4
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns>Country code or an empty string</returns>
        public string GetLocation(string strIP)
        {
            return GetData(strIP).City;
        }

        /// <summary>
        /// Returns JSON string
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public APIResponseModel GetIPFromAPI(string IP)
        {
            APIResponseModel objResponse = new APIResponseModel();

            try
            {
                string url = string.Format(Properties.Settings.Default.APIUrl, IP);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(APIResponseModel));
                        objResponse = (APIResponseModel)jsonSerializer.ReadObject(response.GetResponseStream());
                        objResponse.Success = true;
                    }
                    else
                    {
                        objResponse.Success = false;
                        log.Debug("Request for " + url + "\nStatus code: " + response.StatusCode + "\nError: " + response.StatusDescription);
                    }
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    string errorText = reader.ReadToEnd();
                    log.Error("GetIPFromAPI Error: " + errorText, ex);
                }

                objResponse.Success = false;
            }

            return objResponse;
        }

        public IPCity SaveIP(APIResponseModel model)
        {
            IPCity ip = new IPCity();

            ip.City = model.city;
            ip.CountryCode = model.country;
            ip.IPFrom = model.address;
            ip.IPFromNumber = model.address.IPToNumber();
            ip.IPTo = model.address;
            ip.IPToNumber = model.address.IPToNumber();
            ip.Region = model.stateprov;

            using (var ctx = new ShorturlContext())
            {
                ctx.IPCities.Add(ip);
                ctx.SaveChanges();
            }

            return ip;
        }

        public void Dispose()
        {
        }
    }
}