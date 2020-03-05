using log4net;
using PSC.Shorturl.Web.Data;
using PSC.Shorturl.Web.Entities;
using PSC.Shorturl.Web.Exceptions;
using PSC.Shorturl.Web.Utility;
using PSC.Shorturl.Web.Utility.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PSC.Shorturl.Web.Business.Implementations
{
    public class UrlManager : IUrlManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Task<ShortUrl> ShortenUrl(string longUrl, string ip, string segment = "", string description = "", string username = null)
        {
            return Task.Run(() =>
            {
                using (var ctx = new ShorturlContext())
                {
                    ShortUrl url;

                    url = ctx.ShortUrls.Where(u => u.LongUrl == longUrl).FirstOrDefault();
                    if (url != null)
                    {
                        return url;
                    }

                    if (!longUrl.StartsWith("http://") && !longUrl.StartsWith("https://"))
                    {
                        throw new ArgumentException("Invalid URL format");
                    }

                    Uri urlCheck = new Uri(longUrl);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
                    request.Timeout = 10000;
                    try
                    {
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    }
                    catch (Exception)
                    {
                        throw new ShorturlNotFoundException();
                    }

                    int cap = 0;
                    string capString = ConfigurationManager.AppSettings["MaxNumberShortUrlsPerHour"];
                    int.TryParse(capString, out cap);
                    DateTime dateToCheck = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
                    int count = ctx.ShortUrls.Where(u => u.Ip == ip && u.Added >= dateToCheck).Count();
                    if (cap != 0 && count > cap)
                    {
                        throw new ArgumentException("Your hourly limit has exceeded");
                    }

                    if (!string.IsNullOrEmpty(segment))
                    {
                        if (ctx.ShortUrls.Where(u => u.Segment == segment).Any())
                        {
                            throw new ShorturlConflictException();
                        }
                        if (segment.Length > 20 || !Regex.IsMatch(segment, @"^[A-Za-z\d_-]+$"))
                        {
                            throw new ArgumentException("Malformed or too long segment");
                        }
                    }
                    else
                    {
                        segment = this.NewSegment();
                    }

                    if (string.IsNullOrEmpty(segment))
                    {
                        throw new ArgumentException("Segment is empty");
                    }

                    url = new ShortUrl()
                    {
                        Added = DateTime.Now,
                        Description = description,
                        Ip = ip,
                        LongUrl = longUrl,
                        NumOfClicks = 0,
                        Segment = segment,
                        Username = username
                    };

                    ctx.ShortUrls.Add(url);
                    ctx.SaveChanges();

                    return url;
                }
            });
        }

        public Stat SaveStatistic(Stat stat, string segment)
        {
            ShorturlContext ctx = new ShorturlContext();
            
            ShortUrl url = ctx.ShortUrls.Where(u => u.Segment == segment).FirstOrDefault();
            if (url == null)
            {
                log.Warn("Segment: " + segment + " ShorturlNotFoundException");
                throw new ShorturlNotFoundException();
            }

            log.Debug("Segment: " + segment + " Num of clicks: " + url.NumOfClicks);
            url.NumOfClicks = url.NumOfClicks + 1;
            stat.ShortUrl = url;

            ctx.Stats.Add(stat);
            ctx.SaveChanges();

            return stat;
        }

        public Task<Stat> Click(Stat stat, string segment)
        {
            return Task.Run(() => SaveStatistic(stat, segment));
        }

        /// <summary>
        /// Delete a shorturl
        /// </summary>
        /// <param name="segment">Segment</param>
        /// <param name="username">Username to search</param>
        /// <returns></returns>
        public void Delete(string username, string shorturl)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(shorturl))
            {
                using (var ctx = new ShorturlContext())
                {
                    var qry = ctx.ShortUrls.Where(u => u.Username == username && u.Segment == shorturl);

                    if (qry.Count() > 0)
                    {
                        ctx.ShortUrls.Remove(qry.First());
                        ctx.SaveChanges();
                    }
                }
            }
        }

        private string NewSegment()
        {
            using (var ctx = new ShorturlContext())
            {
                int i = 0;
                while (true)
                {
                    string segment = Guid.NewGuid().ToString().Substring(0, 6);
                    if (!ctx.ShortUrls.Where(u => u.Segment == segment).Any())
                    {
                        return segment;
                    }
                    if (i > 30)
                    {
                        break;
                    }
                    i++;
                }
                return string.Empty;
            }
        }

        public async Task<StatUrl> Stats(string segment)
        {
            StatUrl rtn = new StatUrl();
            rtn.Segment = segment;
            rtn.Success = false;

            using (var ctx = new ShorturlContext())
            {
                try
                {
                    var qry = ctx.ShortUrls.Where(x => x.Segment == segment).First();
                    rtn.CreatedAt = qry.Added;
                    rtn.FullUrl = qry.LongUrl;
                    rtn.Visit = qry.NumOfClicks.ToString();
                    rtn.Success = true;
                }
                catch (Exception ex)
                {
                    log.Error("SQL Error", ex);
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets the list of recent n popular records
        /// </summary>
        /// <param name="ntop">Number of records (default first 10)</param>
        /// <param name="username">Username to search (default public short url)</param>
        /// <returns></returns>
        public List<Entities.ShortUrl> GetListShortUrl(string username, string sortOrder = "", string searchString = "")
        {
            List<Entities.ShortUrl> rtn = new List<Entities.ShortUrl>();

            if (!string.IsNullOrEmpty(username))
            {
                using (var ctx = new ShorturlContext())
                {
                    var qry = ctx.ShortUrls.Where(u => u.Username == username);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        qry = qry.Where(s => s.Segment.Contains(searchString) ||
                                             s.Description.Contains(searchString));
                    }

                    switch (sortOrder)
                    {
                        case "name_desc":
                            qry = qry.OrderByDescending(s => s.Segment);
                            break;
                        case "date":
                            qry = qry.OrderBy(s => s.Added);
                            break;
                        case "date_desc":
                            qry = qry.OrderByDescending(s => s.Added);
                            break;
                        default:
                            qry = qry.OrderBy(s => s.Segment);
                            break;
                    }

                    rtn = qry.ToList();
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets the list of recent n popular records
        /// </summary>
        /// <param name="ntop">Number of records (default first 10)</param>
        /// <param name="username">Username to search (default public short url)</param>
        /// <returns></returns>
        public List<Entities.ShortUrl> GetRecentlyAddedShortUrl(int ntop = 10, string username = "")
        {
            List<Entities.ShortUrl> rtn = new List<Entities.ShortUrl>();

            using (var ctx = new ShorturlContext())
            {
                if (string.IsNullOrEmpty(username))
                {
                    rtn = ctx.ShortUrls.Where(u => u.Username == null || u.Username == "").OrderByDescending(u => u.Added).Take(ntop).ToList();
                }
                else
                {
                    rtn = ctx.ShortUrls.Where(u => u.Username == username).OrderByDescending(u => u.Added).Take(ntop).ToList();
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets a shorturl
        /// </summary>
        /// <param name="segment">Segment</param>
        /// <param name="username">Username to search</param>
        /// <returns></returns>
        public Entities.ShortUrl GetShortUrl(string username, string shorturl)
        {
            Entities.ShortUrl rtn = new Entities.ShortUrl();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(shorturl))
            {
                using (var ctx = new ShorturlContext())
                {
                    var qry = ctx.ShortUrls.Where(u => u.Username == username && u.Segment == shorturl);
                    rtn = qry.First();
                }
            }

            return rtn;
        }

        /// <summary>
        /// Gets the list of first n popular records
        /// </summary>
        /// <param name="ntop">Number of records (default first 10)</param>
        /// <param name="username">Username to search (default public short url)</param>
        /// <returns></returns>
        public List<Entities.ShortUrl> GetTopShortUrl(int ntop = 10, string username = "")
        {
            List<Entities.ShortUrl> rtn = new List<Entities.ShortUrl>();

            using (var ctx = new ShorturlContext())
            {
                if (string.IsNullOrEmpty(username))
                {
                    rtn = ctx.ShortUrls.Where(u => u.Username == null || u.Username == "").OrderByDescending(u => u.NumOfClicks).Take(ntop).ToList();
                }
                else
                {
                    rtn = ctx.ShortUrls.Where(u => u.Username == username).OrderByDescending(u => u.NumOfClicks).Take(ntop).ToList();
                }
            }

            return rtn;
        }

        /// <summary>
        /// Delete a shorturl
        /// </summary>
        /// <param name="segment">Segment</param>
        /// <param name="username">Username to search</param>
        /// <returns></returns>
        public void Update(string username, Entities.ShortUrl shorturl)
        {
            using (var ctx = new ShorturlContext())
            {
                var qry = ctx.ShortUrls.Where(u => u.Username == username && u.Segment == shorturl.Segment);

                if (qry.Count() > 0)
                {
                    qry.First().Description = shorturl.Description;
                    qry.First().LongUrl = shorturl.LongUrl;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
