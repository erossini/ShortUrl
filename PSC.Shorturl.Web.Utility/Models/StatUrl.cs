using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Utility.Models
{
    public class StatUrl
    {
        public string FullUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Segment { get; set; }
        public bool Success { get; set; }
        public string Visit { get; set; }
    }
}