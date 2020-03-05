using System.Threading.Tasks;
using PSC.Shorturl.Web.Entities;
using System.Web;
using PSC.Shorturl.Web.Utility.Models;
using System.Collections.Generic;

namespace PSC.Shorturl.Web.Business
{
    public interface IUrlManager
    {
        Task<Stat> Click(Stat stat, string segment);
        void Delete(string username, string shorturl);
        List<Entities.ShortUrl> GetListShortUrl(string username, string sortOrder = "", string searchString = "");
        List<Entities.ShortUrl> GetRecentlyAddedShortUrl(int ntop = 10, string username = "");
        Entities.ShortUrl GetShortUrl(string username, string shorturl);
        List<Entities.ShortUrl> GetTopShortUrl(int ntop = 10, string username = "");
        Task<ShortUrl> ShortenUrl(string longUrl, string ip, string segment = "", string description = "", string username = null);
        Task<StatUrl> Stats(string segment);
        void Update(string username, Entities.ShortUrl shorturl);
    }
}