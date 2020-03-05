using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Repository;
using PSC.Log4Net.Async;
using PSC.Shorturl.Web.Business;
using PSC.Shorturl.Web.Business.Implementations;
using PSC.Shorturl.Web.Data;
using PSC.Shorturl.Web.Data.Extensions;
using PSC.Shorturl.Web.Entities;
using PSC.Shorturl.Web.Models;
using PSC.Shorturl.Web.Utility;
using PSC.Shorturl.Web.Utility.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using PSC.Shorturl.Web.Report.Models;
using PSC.Shorturl.Web.Report;
using PSC.Shorturl.Web.IPToCountry;
using PagedList;
using System.Net;
using PSC.QRCoder;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace PSC.Shorturl.Web.Controllers
{
    public class UrlController : Controller
    {
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IUrlManager _urlManager;

        #region " Functions "

        /// <summary>
        /// Creates a viewbag for top sites
        /// </summary>
        private void CreateViewBag()
        {
            List<Url> top = new List<Models.Url>();
            foreach (Entities.ShortUrl s in _urlManager.GetTopShortUrl())
            {
                Url u = new Url();
                u.Description = s.Description;
                u.ShortURL = s.Segment;
                top.Add(u);
            }
            ViewBag.TopShortUrlList = top;

            List<Url> recently = new List<Models.Url>();
            foreach (Entities.ShortUrl s in _urlManager.GetRecentlyAddedShortUrl())
            {
                Url u = new Url();
                u.Description = s.Description;
                u.ShortURL = s.Segment;
                recently.Add(u);
            }
            ViewBag.RecentlyAdded = recently;
        }

        #endregion
        #region " QRCode "

        private Bitmap getIconBitmap(string iconPath)
        {
            Bitmap img = null;
            if (iconPath.Length > 0)
            {
                try
                {
                    img = new Bitmap(iconPath);
                }
                catch (Exception)
                {
                }
            }
            return img;
        }

        private Bitmap renderQRCode(string text)
        {
            string level = "L";
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, eccLevel);
            QRCode qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(20, Color.Black, Color.White, getIconBitmap(""), 16);
        }

        // GET: QRCode
        public ActionResult QRCode(string text)
        {
            //Return Image
            MemoryStream ms = new MemoryStream();

            Image img = new Bitmap(100, 50);
            img = renderQRCode(text);
            img.Save(ms, ImageFormat.Png);

            ms.Position = 0;

            return new FileStreamResult(ms, "image/png");
        }

        #endregion

        public UrlController(IUrlManager urlManager)
        {
            this._urlManager = urlManager;
        }

        [HttpGet]
        public ActionResult Index()
        {
            log.Info("Application stat");

            CreateViewBag();

            Url url = new Url();
            return View(url);
        }

        public async Task<ActionResult> Index(Url url)
        {
            if (ModelState.IsValid)
            {
                string username = null;
                if (User.Identity.IsAuthenticated)
                {
                    username = User.Identity.Name;
                }

                Entities.ShortUrl shortUrl = await this._urlManager.ShortenUrl(url.LongURL, Request.UserHostAddress, url.CustomSegment, url.Description, username);
                url.ShortURL = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"), shortUrl.Segment);
            }

            CreateViewBag();
            return View(url);
        }

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)] // will disable caching
        public async Task<ActionResult> Click(string segment)
        {
            Statistics st = new Statistics();
            StatDetail sd = st.ExtractUserDetails(HttpContext);

            IPQuery qry = new IPQuery();
            IPCity city = qry.GetData(sd.IP);
            sd.IPCountry = city.CountryCode;
            sd.IPLocation = city.City;
            qry.Dispose();

            log.Debug("Request Click for " + segment);
            Stat stat = await _urlManager.Click(sd.Populate(), segment);

            log.Debug("Redirect Click for " + segment + " to " + stat.ShortUrl.LongUrl);
            return this.Redirect(stat.ShortUrl.LongUrl);
        }

        public async Task<ActionResult> Delete(string segment)
        {
            return View(_urlManager.GetShortUrl(HttpContext.User.Identity.Name, segment));
        }

        [HttpPost]
        public ActionResult Delete(Entities.ShortUrl shorturl)
        {
            _urlManager.Delete(HttpContext.User.Identity.Name, shorturl.Segment);
            return RedirectToAction("Index", "Url");
        }

        public ActionResult Edit(string segment)
        {
            if (string.IsNullOrEmpty(segment))
            {
                return View("ErrorParam");
            }

            return View(_urlManager.GetShortUrl(HttpContext.User.Identity.Name, segment));
        }

        [HttpPost]
        public ActionResult Edit(Entities.ShortUrl shorturl)
        {
            _urlManager.Update(HttpContext.User.Identity.Name, shorturl);
            return RedirectToAction("View", "Url");
        }

        public ActionResult ErrorParam()
        {
            return View();
        }

        public ActionResult ErrorUser()
        {
            return View();
        }

        public async Task<FileResult> Print(string ReportType, string segment, string start, string end)
        {
            ReportGenerator rg = new ReportGenerator();

            // read info about segment
            StatUrl stat = await _urlManager.Stats(segment);
            if (stat.Success)
            {
                StatManager sm = new StatManager();

                IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);

                DateTime dtStart = DateTime.Now;
                DateTime dtEnd = DateTime.Now;
                try
                {
                    dtStart = DateTime.Parse(start, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                    dtEnd = DateTime.Parse(end, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                }
                catch (Exception ex)
                {
                    log.Error("Date conversion error", ex);
                    log.Error("Start date: " + start);
                    log.Error("  End date: " + end);
                }

                StatsReportModel trm = new StatsReportModel();
                trm.CreatedAt = stat.CreatedAt;
                trm.FullUrl = stat.FullUrl;
                trm.Info = "";
                trm.ReportPath = "";
                trm.Segment = stat.Segment;
                trm.FromDate = dtStart;
                trm.ToDate = dtEnd;
                trm.Title = "Stats report";
                trm.Visits = Convert.ToInt32(stat.Visit);

                try
                {
                    trm.StatsData = new List<GenericClass>();
                    List<GenericResult> grStats = sm.GetStatSegment(segment, dtStart, dtEnd);
                    foreach (GenericResult s in grStats)
                    {
                        trm.StatsData.Add(new GenericClass() { key = s.key, value = s.cnt });
                    }
                }
                catch(Exception ex)
                {
                    log.Error("GetStatSegment error", ex);
                    log.Error("Start date: " + start);
                    log.Error("  End date: " + end);
                }

                trm.StatsBrowsers = new List<GenericClass>();
                List<TrafficData> grBrowsers = sm.StatSegmentForBrowser(segment, dtStart, dtEnd);
                foreach (TrafficData t in grBrowsers)
                {
                    trm.StatsBrowsers.Add(new GenericClass() { key = t.label, value = t.value });
                }

                trm.StatsDevices = new List<GenericClass>();
                List<TrafficData> grDevices = sm.StatSegmentForDevice(segment, dtStart, dtEnd);
                foreach (TrafficData t in grDevices)
                {
                    trm.StatsDevices.Add(new GenericClass() { key = t.label, value = t.value });
                }

                trm.StatsPlatforms = new List<GenericClass>();
                List<TrafficData> grPlatform = sm.StatSegmentForPlatform(segment, dtStart, dtEnd);
                foreach (TrafficData t in grPlatform)
                {
                    trm.StatsPlatforms.Add(new GenericClass() { key = t.label, value = t.value });
                }

                trm.StatsReferrer = new List<GenericClass>();
                List<GenericResult> grReferrer = sm.GetReferer(segment, dtStart, dtEnd);
                foreach (GenericResult r in grReferrer)
                {
                    trm.StatsReferrer.Add(new GenericClass() { key = r.key, value = r.cnt });
                }

                // create response for the request
                string contentType = "application/pdf";
                string filename = "report.pdf";

                if (string.IsNullOrEmpty(ReportType))
                    ReportType = "";

                switch (ReportType.ToUpper())
                {
                    case "EXCEL":
                        trm.ExportFormat = ExportFormat.Excel;
                        filename = "report.xls";
                        contentType = "application/msexcel";
                        break;
                    case "EXCEL2007":
                        trm.ExportFormat = ExportFormat.Excel2007;
                        filename = "report.xlsx";
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case "IMAGE":
                        trm.ExportFormat = ExportFormat.Image;
                        filename = "report.tif";
                        contentType = "text/tiff";
                        break;
                    case "WORD":
                        trm.ExportFormat = ExportFormat.Word;
                        filename = "report.doc";
                        contentType = "application/msword";
                        break;
                    case "WORD2007":
                        trm.ExportFormat = ExportFormat.Word2007;
                        filename = "report.docx";
                        contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    default:
                        trm.ExportFormat = ExportFormat.PDF;
                        break;
                }

                byte[] bytes = rg.CreateStatsReport(trm);

                return File(bytes, contentType, filename);
            }
            else
            {
                return null;
            }
        }

        public async Task<ActionResult> Stats(string segment)
        {
            log.Info("Create stats for " + segment);

            StatUrl stat = await _urlManager.Stats(segment);
            return View(stat);
        }

        public async Task<ActionResult> View (string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(_urlManager.GetListShortUrl(HttpContext.User.Identity.Name, 
                            sortOrder, searchString).ToPagedList(pageNumber, pageSize));
            }

            return View();
        }
    }
}