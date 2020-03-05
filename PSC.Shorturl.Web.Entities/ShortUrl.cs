using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Entities
{
	[Table("short_urls")]
	public class ShortUrl
	{
		[Key]
		[Column("id")]
		public int Id { get; set; }

		[Required]
		[Column("long_url")]
		[StringLength(1000)]
		public string LongUrl { get; set; }

        [Column("description")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
		[Column("segment")]
		[StringLength(20)]
        public string Segment { get; set; }

		[Required]
		[Column("added")]
		public DateTime Added { get; set; }

		[Required]
		[Column("ip")]
		[StringLength(50)]
		public string Ip { get; set; }

		[Required]
		[Column("num_of_clicks")]
		public int NumOfClicks { get; set; }

        [Column("username")]
        public string Username { get; set; }

        public Stat[] Stats { get; set; }

        public Category CategoryID { get; set; }
	}
}
