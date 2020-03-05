using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Entities
{
	[Table("stats")]
	public class Stat
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Required]
		[Column("click_date")]
		public DateTime ClickDate { get; set; }

		[Required]
		[Column("IP")]
		[StringLength(50)]
		public string Ip { get; set; }

		[Column("referer")]
		[StringLength(500)]
		public string Referer { get; set; }

		public ShortUrl ShortUrl { get; set; }

        [Column("userAgent")]
        [StringLength(500)]
        public string userAgent { get; set; }

        [Column("BrowserName")]
        [StringLength(150)]
        public string BrowserName { get; set; }

        [Column("BrowserVersion")]
        [StringLength(50)]
        public string BrowserVersion { get; set; }

        [Column("BrowserLanguage")]
        [StringLength(50)]
        public string BrowserLanguage { get; set; }

        [Column("ConnectionSpeed")]
        [StringLength(50)]
        public string ConnectionSpeed { get; set; }

        [Column("Platform")]
        [StringLength(50)]
        public string Platform { get; set; }

        [Column("PlatformLanguage")]
        [StringLength(50)]
        public string PlatformLanguage { get; set; }

        [Column("IPCountry")]
        [StringLength(150)]
        public string IPCountry { get; set; }

        [Column("IPLocation")]
        [StringLength(150)]
        public string IPLocation { get; set; }

        [Column("ISCrawler")]
        public bool ISCrawler { get; set; }

        [Column("JScriptVersion")]
        [StringLength(50)]
        public string JScriptVersion { get; set; }

        [Column("IsMobile")]
        public bool IsMobile { get; set; }

        [Column("MobileDeviceModel")]
        [StringLength(50)]
        public string MobileDeviceModel { get; set; }

        [Column("MobileDeviceManufacturer")]
        [StringLength(50)]
        public string MobileDeviceManufacturer { get; set; }

        [Column("ScreenPixelsHeight")]
        [StringLength(10)]
        public string ScreenPixelsHeight { get; set; }

        [Column("ScreenPixelsWidth")]
        [StringLength(10)]
        public string ScreenPixelsWidth { get; set; }

        [Column("QueryString")]
        public string QueryString { get; set; }

        [Column("Username")]
        [StringLength(256)]
        public string Username { get; set; }

        [Column("UtmMedium")]
        public string UtmMedium { get; set; }

        [Column("UtmSource")]
        public string UtmSource { get; set; }

        [Column("UtmCampaign")]
        public string UtmCampaign { get; set; }

        [Column("UtmContext")]
        public string UtmContext { get; set; }
    }
}