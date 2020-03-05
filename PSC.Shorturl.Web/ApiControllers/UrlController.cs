using log4net;
using PSC.Chartjs.Mvc.ComplexChart;
using PSC.Shorturl.Web.Business;
using PSC.Shorturl.Web.Business.Implementations;
using PSC.Shorturl.Web.Data;
using PSC.Shorturl.Web.Entities;
using PSC.Shorturl.Web.Models;
using PSC.Shorturl.Web.Utility.Models;
using PSC.ShortUrl.Web.IPToCountry;
using PSC.ShortUrl.Web.IPToCountry.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PSC.Shorturl.Web.ApiControllers
{
	[RoutePrefix("api/url")]
    public class UrlController : ApiController
    {
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IUrlManager _urlManager;

        public UrlController()
        {
            _urlManager = new UrlManager();
        }

		public UrlController(IUrlManager urlManager)
		{
			this._urlManager = urlManager;
		}

        [Route("short")]
        [HttpGet]
        public async Task<Url> Short([FromUri]string url, [FromUri]string segment = "")
        {
            Entities.ShortUrl shortUrl = await _urlManager.ShortenUrl(HttpUtility.UrlDecode(url), HttpContext.Current.Request.UserHostAddress, segment);
            Url urlModel = new Url()
            {
                LongURL = url,
                ShortURL = string.Format("{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, shortUrl.Segment)
            };
            return urlModel;
        }

        [HttpPost]
        [Route("stats")]
        public async Task<List<object>> Stats([FromUri]string segment, [FromUri]string start, [FromUri]string end)
        {
            List<object> iData = new List<object>();

            IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
            DateTime dtStart = DateTime.Parse(start, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime dtEnd = DateTime.Parse(end, culture, System.Globalization.DateTimeStyles.AssumeLocal);

            try
            {
                StatManager sm = new StatManager();

                iData = sm.StatSegmentForGraph(segment, dtStart, dtEnd);
            }
            catch(Exception ex)
            {
                log.Error("Stats API Error", ex);
                log.Error("  Parameters: ");
                log.Error("     Segment: " + segment);
                log.Error("       Start: " + start);
                log.Error("         End: " + end);
            }

            return iData;
        }

        [HttpPost]
        [Route("statsbrowser")]
        public async Task<List<TrafficData>> StatsBrowser([FromUri]string segment, [FromUri]string start, [FromUri]string end)
        {
            List<TrafficData> iData = new List<TrafficData>();

            IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
            DateTime dtStart = DateTime.Parse(start, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime dtEnd = DateTime.Parse(end, culture, System.Globalization.DateTimeStyles.AssumeLocal);

            StatManager sm = new StatManager();

            try
            {
                iData = sm.StatSegmentForBrowser(segment, dtStart, dtEnd);
            }
            catch (Exception ex)
            {
                log.Error("StatsBrowser API Error", ex);
                log.Error("  Parameters: ");
                log.Error("     Segment: " + segment);
                log.Error("       Start: " + start);
                log.Error("         End: " + end);
            }

            return iData;
        }

        [HttpPost]
        [Route("statsdevice")]
        public async Task<List<TrafficData>> StatsDevice([FromUri]string segment, [FromUri]string start, [FromUri]string end)
        {
            List<TrafficData> iData = new List<TrafficData>();
            StatManager sm = new StatManager();

            IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
            DateTime dtStart = DateTime.Parse(start, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime dtEnd = DateTime.Parse(end, culture, System.Globalization.DateTimeStyles.AssumeLocal);

            try
            {
                iData = sm.StatSegmentForDevice(segment, dtStart, dtEnd);
            }
            catch (Exception ex)
            {
                log.Error("StatsDevice API Error", ex);
                log.Error("  Parameters: ");
                log.Error("     Segment: " + segment);
                log.Error("       Start: " + start);
                log.Error("         End: " + end);
            }

            return iData;
        }

        /// <summary>
        /// Renders out javascript
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        [Route("statsmap")]
        public async Task<HttpResponseMessage> StatsMap([FromUri]string segment, [FromUri]string start, [FromUri]string end)
        {
            List<TrafficData> iData = new List<TrafficData>();
            StatManager sm = new StatManager();

            IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
            DateTime dtStart = DateTime.Parse(start, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime dtEnd = DateTime.Parse(end, culture, System.Globalization.DateTimeStyles.AssumeLocal);

            string responseBody = "";

            try
            {
                iData = sm.StatSegmentForMap(segment, dtStart, dtEnd);
                foreach(TrafficData td in iData)
                {
                    if (!string.IsNullOrEmpty(td.label))
                    {
                        if (responseBody.Length > 0)
                            responseBody += ", ";

                        responseBody += "'" + td.label + "': " + td.value.ToString();
                    }
                }

                if (responseBody.Length > 0)
                {
                    responseBody = "var gdpData = { " + responseBody + " };";
                }
            }
            catch (Exception ex)
            {
                log.Error("StatsMap API Error", ex);
                log.Error("  Parameters: ");
                log.Error("     Segment: " + segment);
                log.Error("       Start: " + start);
                log.Error("         End: " + end);
            }

            HttpResponseMessage response = Request.CreateResponse(System.Net.HttpStatusCode.OK, responseBody);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-javascript");
            return response;
        }

        [HttpPost]
        [Route("statsplatform")]
        public async Task<List<TrafficData>> StatsPlatform([FromUri]string segment, [FromUri]string start, [FromUri]string end)
        {
            List<TrafficData> iData = new List<TrafficData>();
            StatManager sm = new StatManager();

            IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
            DateTime dtStart = DateTime.Parse(start, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime dtEnd = DateTime.Parse(end, culture, System.Globalization.DateTimeStyles.AssumeLocal);

            try
            {
                iData = sm.StatSegmentForPlatform(segment, dtStart, dtEnd);
            }
            catch (Exception ex)
            {
                log.Error("StatsPlatform API Error", ex);
                log.Error("  Parameters: ");
                log.Error("     Segment: " + segment);
                log.Error("       Start: " + start);
                log.Error("         End: " + end);
            }

            return iData;
        }

        [HttpPost]
        [Route("statsreferrer")]
        public async Task<List<GenericResult>> StatsReferrer([FromUri]string segment, [FromUri]string start, [FromUri]string end)
        {
            List<GenericResult> iData = new List<GenericResult>();
            StatManager sm = new StatManager();

            IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);
            DateTime dtStart = DateTime.Parse(start, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            DateTime dtEnd = DateTime.Parse(end, culture, System.Globalization.DateTimeStyles.AssumeLocal);

            try
            {
                iData = sm.GetReferer(segment, dtStart, dtEnd);
            }
            catch (Exception ex)
            {
                log.Error("StatsReferrer API Error", ex);
                log.Error("  Parameters: ");
                log.Error("     Segment: " + segment);
                log.Error("       Start: " + start);
                log.Error("         End: " + end);
            }

            return iData;
        }
    }
}