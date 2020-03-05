using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Entities
{
    [Table("IPCities")]
    public class IPCity
    {
        [Key]
        [Required]
        [Column("IPFrom", Order = 0)]
        [StringLength(15)]
        public string IPFrom { get; set; }

        [Key]
        [Required]
        [Column("IPTo", Order = 1)]
        [StringLength(15)]
        public string IPTo { get; set; }

        [Key]
        [Required]
        [Column("CountryCode", Order = 2)]
        [StringLength(2)]
        public string CountryCode { get; set; }

        [Column("Region")]
        [StringLength(500)]
        public string Region { get; set; }

        [Column("City")]
        [StringLength(500)]
        public string City { get; set; }

        [Column("IPFromNumber")]
        public decimal IPFromNumber { get; set; }

        [Column("IPToNumber")]
        public decimal IPToNumber { get; set; }

        private DateTime lastUpdated = default(DateTime);
        [Column("LastUpdated")]
        public DateTime LastUpdated
        {
            get
            {
                return (this.lastUpdated == default(DateTime))
                   ? this.lastUpdated = DateTime.Now
                   : this.lastUpdated;
            }

            set { this.lastUpdated = value; }
        }
    }
}
