using Microsoft.Reporting.WebForms;
using PSC.Shorturl.Web.Report.Models;
using PSC.Shorturl.Web.Report.XSD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace PSC.Shorturl.Web.Report
{
    public class ReportGenerator
    {
        public byte[] CreateTemplateReport(TemplateReportModels template)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);

            dsTemplateReport Header = new dsTemplateReport();
            DataRow hr = Header.Tables[0].NewRow();
            hr["Info"] = template.Info;
            Header.Tables[0].Rows.Add(hr);

            string pth = "";
            if (string.IsNullOrEmpty(template.ReportPath))
            {
                pth = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");
            }
            else
            {
                pth = template.ReportPath;
            }

            reportViewer.LocalReport.ReportPath = pth + @"\Reports\TemplateReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", Header.Tables[0]));

            ReportParameter p1 = new ReportParameter("LanguageInfo", string.IsNullOrEmpty(template.LanguageInfo) ? "en-GB" : template.LanguageInfo);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { p1 });

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            reportViewer.ProcessingMode = ProcessingMode.Local;
            byte[] bytes = reportViewer.LocalReport.Render(ReportBaseModels.GetExportFormatString(template.ExportFormat), null, out mimeType,
                                                           out encoding, out extension, out streamIds, out warnings);

            //RenderingExtension[] r = reportViewer.LocalReport.ListRenderingExtensions();

            return bytes;
        }

        public byte[] CreateStatsReport(StatsReportModel template)
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(100);
            reportViewer.Height = Unit.Percentage(100);
            
            // map the common info for this report
            dsStatsReport Header = new dsStatsReport();
            DataRow hr = Header.Tables["StatsInfo"].NewRow();
            hr["Segment"] = template.Segment;
            hr["FullUrl"] = template.FullUrl;
            hr["CreateAt"] = template.CreatedAt;
            hr["Visits"] = template.Visits;
            hr["FromDate"] = template.FromDate;
            hr["ToDate"] = template.ToDate;
            Header.Tables["StatsInfo"].Rows.Add(hr);

            // format for date and time
            IFormatProvider culture = new System.Globalization.CultureInfo("en-GB", true);

            // map stats data
            foreach (GenericClass gc in template.StatsData)
            {
                DataRow drStat = Header.Tables["Stats"].NewRow();
                drStat["StatDate"] = DateTime.Parse(gc.key, culture, System.Globalization.DateTimeStyles.AssumeLocal);
                drStat["StatCount"] = gc.value;
                Header.Tables["Stats"].Rows.Add(drStat);
            }

            // map stats data for browsers
            foreach (GenericClass gc in template.StatsBrowsers)
            {
                DataRow drStat = Header.Tables["StatsBrowser"].NewRow();
                drStat["BrowserName"] = gc.key;
                drStat["BrowserCount"] = gc.value;
                Header.Tables["StatsBrowser"].Rows.Add(drStat);
            }

            // map stats data for device
            foreach (GenericClass gc in template.StatsDevices)
            {
                DataRow drStat = Header.Tables["StatsDevice"].NewRow();
                drStat["DeviceName"] = gc.key;
                drStat["DeviceCount"] = gc.value;
                Header.Tables["StatsDevice"].Rows.Add(drStat);
            }

            // map stats data for device
            foreach (GenericClass gc in template.StatsPlatforms)
            {
                DataRow drStat = Header.Tables["StatsPlatform"].NewRow();
                drStat["PlatformName"] = gc.key;
                drStat["PlatformCount"] = gc.value;
                Header.Tables["StatsPlatform"].Rows.Add(drStat);
            }

            // map stats data for device
            foreach (GenericClass gc in template.StatsReferrer)
            {
                DataRow drRef = Header.Tables["StatsReferrer"].NewRow();
                drRef["Referrer"] = gc.key;
                drRef["ReferrerCount"] = gc.value;
                Header.Tables["StatsReferrer"].Rows.Add(drRef);
            }

            string pth = "";
            if (string.IsNullOrEmpty(template.ReportPath))
            {
                pth = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");
            }
            else
            {
                pth = template.ReportPath;
            }

            reportViewer.LocalReport.EnableHyperlinks = true;
            reportViewer.LocalReport.ReportPath = pth + @"\Reports\UrlReport.rdlc";
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsStats", Header.Tables["Stats"]));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsStatsInfo", Header.Tables["StatsInfo"]));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsStatsBrowsers", Header.Tables["StatsBrowser"]));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsStatsPlatforms", Header.Tables["StatsPlatform"]));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsStatsDevices", Header.Tables["StatsDevice"]));
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("dsStatsReferrer", Header.Tables["StatsReferrer"]));

            ReportParameter p1 = new ReportParameter("LanguageInfo", string.IsNullOrEmpty(template.LanguageInfo) ? "en-GB" : template.LanguageInfo);
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { p1 });

            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            // Setup the report viewer object and get the array of bytes
            reportViewer.ProcessingMode = ProcessingMode.Local;
            byte[] bytes = reportViewer.LocalReport.Render(ReportBaseModels.GetExportFormatString(template.ExportFormat), null, out mimeType,
                                                           out encoding, out extension, out streamIds, out warnings);

            //RenderingExtension[] r = reportViewer.LocalReport.ListRenderingExtensions();

            return bytes;
        }
    }
}