using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Utility.Models
{
    public class TrafficData
    {
        public string label { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }

    public class TrafficDataForBar
    {
        public string label { get; set; }
        public List<string> value { get; set; }
        public string color { get; set; }
        public string hightlight { get; set; }
    }
}
