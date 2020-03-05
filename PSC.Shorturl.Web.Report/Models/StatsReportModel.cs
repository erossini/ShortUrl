using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Report.Models
{
    public class StatsReportModel : ReportBaseModels
    {
        public string Info { get; set; }
        public string Segment { get; set; }
        public string FullUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Visits { get; set; }

        public List<GenericClass> StatsData { get; set; }
        public List<GenericClass> StatsBrowsers { get; set; }
        public List<GenericClass> StatsDevices { get; set; }
        public List<GenericClass> StatsPlatforms { get; set; }
        public List<GenericClass> StatsReferrer { get; set; }
    }
}