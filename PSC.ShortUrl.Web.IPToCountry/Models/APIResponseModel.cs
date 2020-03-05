using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.ShortUrl.Web.IPToCountry.Models
{
    public class APIResponseModel
    {
        public string address { get; set; }
        public string country { get; set; }
        public string stateprov { get; set; }
        public string city { get; set; }
        public bool Success { get; set; }
    }
}