using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSC.Shorturl.Web.Models
{
	public class Url
	{
		[Required]
		[JsonProperty("longUrl")]
        [MinLength(10, ErrorMessage = "Url must be minimum 10 characters")]
        [MaxLength(1000, ErrorMessage = "Url must be maximum 1000 characters")]
        [StringLength(1000)]
        [Url(ErrorMessage = "Url is not valid")]
        public string LongURL { get; set; }
        
        [MaxLength(20)]
        [JsonProperty("shortUrl")]
        public string ShortURL { get; set; }

		[JsonIgnore]
        [MinLength(3, ErrorMessage = "Short url must be minimum 3 characters")]
        [MaxLength(20, ErrorMessage = "Short url must be maximum 20 characters")]
        [JsonProperty("shortUrl")]
        [RegularExpression(@"^[A-Za-z\d_-]+$", ErrorMessage = "Invalid short url (only alphanumeric characters)")]
        public string CustomSegment { get; set; }

        [JsonProperty("description")]
        [MaxLength(1000, ErrorMessage = "Description must be maximum 1000 characters")]
        [StringLength(1000)]
        public string Description { get; set; }
    }
}