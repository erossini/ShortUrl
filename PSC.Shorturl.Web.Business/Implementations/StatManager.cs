using log4net;
using PSC.ExtensionTypes;
using PSC.Shorturl.Web.Data;
using PSC.Shorturl.Web.Entities;
using PSC.Shorturl.Web.Utility.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Business.Implementations
{
    public class StatManager : IStatManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets a shorturl
        /// </summary>
        /// <param name="segment">Segment</param>
        /// <param name="username">Username to search</param>
        /// <returns></returns>
        public List<GenericResult> GetReferer(string segment, DateTime dtStart, DateTime dtEnd)
        {
            List<GenericResult> rtn = new List<GenericResult>();
            dtStart = Convert.ToDateTime(dtStart.ToShortDateString() + " 00:00:00");
            dtEnd = Convert.ToDateTime(dtEnd.ToShortDateString() + " 23:59:59");

            using (var ctx = new ShorturlContext())
            {
                var qry = from r in ctx.Stats
                          where r.ClickDate >= dtStart && r.ClickDate <= dtEnd && r.ShortUrl.Segment == segment
                          select r.Referer;

                foreach (string s in qry)
                {
                    string tmp = s.ExtractDomainNameFromURL();
                    if (string.IsNullOrEmpty(tmp))
                        tmp = "(direct)";

                    var ext = from i in rtn where i.key == tmp select i;
                    if (ext.Count() > 0)
                    {
                        ext.First().cnt += 1;
                    }
                    else
                    {
                        rtn.Add(new GenericResult() { key = tmp, cnt = 1 });
                    }
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets the ID for this segment
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public int GetSegmentId(string segment)
        {
            int rtn = -1;

            using (var ctx = new ShorturlContext())
            {
                ShortUrl url = ctx.ShortUrls.Where(u => u.Segment == segment).FirstOrDefault();
                if (url != null)
                {
                    rtn = url.Id;
                }
            }

            return rtn;
        }

        public List<GenericResult> GetStatSegment(string segment, DateTime dtStart, DateTime dtEnd)
        {
            List<GenericResult> rtn = new List<GenericResult>();
            dtStart = Convert.ToDateTime(dtStart.ToShortDateString() + " 00:00:00");
            dtEnd = Convert.ToDateTime(dtEnd.ToShortDateString() + " 23:59:59");

            using (var ctx = new ShorturlContext())
            {
                try
                {
                    var qry = from r in ctx.Stats
                              orderby r.ClickDate
                              where r.ClickDate >= dtStart && r.ClickDate <= dtEnd && r.ShortUrl.Segment == segment
                              group r by SqlFunctions.DatePart("dd", r.ClickDate).ToString() + "/" +
                                         SqlFunctions.DatePart("mm", r.ClickDate).ToString() + "/" +
                                         SqlFunctions.DatePart("yyyy", r.ClickDate).ToString()
                          into grp
                              select new { key = grp.Key, cnt = grp.Count() };

                    TimeSpan ts = dtEnd - dtStart;
                    for (int i = 0; i < ts.Days + 1; i++)
                    {
                        string dtTmp = dtStart.AddDays(i).ToString("d/M/yyyy");
                        int iVlu = 0;

                        var q = qry.Where(x => x.key == dtTmp);
                        if (q.Count() > 0)
                        {
                            iVlu = q.First().cnt;
                        }

                        rtn.Add(new GenericResult { cnt = iVlu, key = dtTmp });
                    }
                }
                catch (Exception ex)
                {
                    log.Error("SQL Error", ex);
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets a structure with all details for the main graph
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<object> StatSegmentForGraph(string segment, DateTime dtStart, DateTime dtEnd)
        {
            List<object> iData = new List<object>();
            List<string> labels = new List<string>();
            List<int> values = new List<int>();

            List<GenericResult> qry = GetStatSegment(segment, dtStart, dtEnd);
            foreach (GenericResult gr in qry)
            {
                labels.Add(gr.key);
                values.Add(gr.cnt);
            }

            iData.Add(labels);
            iData.Add(values);

            return iData;
        }

        /// <summary>
        /// Generate a random hex color
        /// </summary>
        /// <returns></returns>
        public string GenerateRandomColor()
        {
            Random rnd = new Random();
            string hexOutput = String.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
            while (hexOutput.Length < 6)
                hexOutput = "0" + hexOutput;

            return "#" + hexOutput;
        }

        /// <summary>
        /// Gets a color for a graph
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GraphColor(int index)
        {
            string rtn = "#ffffff";

            string[] arrColor = new string[] { "#eaf1f5", "#dde8ef", "#cfdfe8", "#c2d6e2", "#b5cedc", "#a8c5d5", "#9abccf",
                                               "#8db3c9", "#80aac2", "#72a1bc", "#6598b6", "#5890af", "#4f86a5", "#487b98",
                                               "#42708a", "#3c657d", "#355b70", "#2f5063", "#294555", "#223a48", "#1c303b" };
            if (index > arrColor.Length)
            {
                rtn = GenerateRandomColor();
            }
            else
            {
                rtn = arrColor[index];
            }

            return rtn;
        }

        /// <summary>
        /// Gets a structure with data for browser
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<TrafficData> StatSegmentForBrowser(string segment, DateTime dtStart, DateTime dtEnd)
        {
            List<TrafficData> iData = new List<TrafficData>();

            dtStart = Convert.ToDateTime(dtStart.ToShortDateString() + " 00:00:00");
            dtEnd = Convert.ToDateTime(dtEnd.ToShortDateString() + " 23:59:59");

            using (var ctx = new ShorturlContext())
            {
                try
                {
                    var qry = from r in ctx.Stats
                              orderby r.BrowserName
                              where r.ClickDate >= dtStart && r.ClickDate <= dtEnd && r.ShortUrl.Segment == segment
                              group r by r.BrowserName into grp
                              select new { key = grp.Key, cnt = grp.Count() };

                    int index = 0;
                    foreach (var t in qry)
                    {
                        TrafficData td = new TrafficData();
                        td.label = t.key;
                        td.value = t.cnt;
                        td.color = GraphColor(index);
                        iData.Add(td);

                        index++;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("SQL Error", ex);
                }
            }

            return iData;
        }

        /// <summary>
        /// Gets a structure with data for device
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<TrafficData> StatSegmentForDevice(string segment, DateTime dtStart, DateTime dtEnd)
        {
            List<TrafficData> iData = new List<TrafficData>();

            dtStart = Convert.ToDateTime(dtStart.ToShortDateString() + " 00:00:00");
            dtEnd = Convert.ToDateTime(dtEnd.ToShortDateString() + " 23:59:59");

            using (var ctx = new ShorturlContext())
            {
                try
                {
                    var qry = from r in ctx.Stats
                              orderby r.ClickDate
                              where r.ClickDate >= dtStart && r.ClickDate <= dtEnd && r.ShortUrl.Segment == segment
                              group r by r.IsMobile into grp
                              select new { key = grp.Key, cnt = grp.Count() };

                    int index = 0;
                    foreach (var t in qry)
                    {
                        TrafficData td = new TrafficData();
                        td.label = t.key ? "Mobile" : "Desktop";
                        td.value = t.cnt;
                        td.color = GraphColor(index);
                        iData.Add(td);

                        index++;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("SQL Error", ex);
                }
            }

            return iData;
        }

        /// <summary>
        /// Gets a structure with data for device
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<TrafficData> StatSegmentForMap(string segment, DateTime dtStart, DateTime dtEnd)
        {
            List<TrafficData> iData = new List<TrafficData>();

            dtStart = Convert.ToDateTime(dtStart.ToShortDateString() + " 00:00:00");
            dtEnd = Convert.ToDateTime(dtEnd.ToShortDateString() + " 23:59:59");

            using (var ctx = new ShorturlContext())
            {
                try
                {
                    var qry = from r in ctx.Stats
                              orderby r.ClickDate
                              where r.ClickDate >= dtStart && r.ClickDate <= dtEnd && r.ShortUrl.Segment == segment
                              group r by r.IPCountry into grp
                              select new { key = grp.Key, cnt = grp.Count() };

                    int index = 0;
                    foreach (var t in qry)
                    {
                        TrafficData td = new TrafficData();
                        td.label = t.key;
                        td.value = t.cnt;
                        iData.Add(td);

                        index++;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("SQL Error", ex);
                }
            }

            return iData;
        }

        /// <summary>
        /// Gets a structure with data for operating system
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public List<TrafficData> StatSegmentForPlatform(string segment, DateTime dtStart, DateTime dtEnd)
        {
            List<TrafficData> iData = new List<TrafficData>();

            dtStart = Convert.ToDateTime(dtStart.ToShortDateString() + " 00:00:00");
            dtEnd = Convert.ToDateTime(dtEnd.ToShortDateString() + " 23:59:59");

            using (var ctx = new ShorturlContext())
            {
                try
                {
                    var qry = from r in ctx.Stats
                              orderby r.Platform
                              where r.ClickDate >= dtStart && r.ClickDate <= dtEnd && r.ShortUrl.Segment == segment
                              group r by r.Platform into grp
                              select new { key = grp.Key, cnt = grp.Count() };

                    int index = 0;
                    foreach (var t in qry)
                    {
                        TrafficData td = new TrafficData();
                        td.label = t.key;
                        td.value = t.cnt;
                        td.color = GraphColor(index);
                        iData.Add(td);

                        index++;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("SQL Error", ex);
                }
            }

            return iData;
        }
    }
}