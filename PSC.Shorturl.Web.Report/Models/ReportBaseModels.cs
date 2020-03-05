using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Report.Models
{
    public class ReportBaseModels
    {
        public ExportFormat ExportFormat { get; set; }
        public string Title { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ReportPath { get; set; }
        public string LanguageInfo { get; set; }

        /// <summary>
        /// Gets the string export format of the specified enum.
        /// </summary>
        /// <param name="f">export format string</param>
        /// <returns>enum export format</returns>
        public static ExportFormat GetExportFormatFromString(string f)
        {
            switch (f)
            {
                case "IMAGE": return ExportFormat.Image;
                case "PDF": return ExportFormat.PDF;
                case "EXCEL": return ExportFormat.Excel;
                case "EXCELOPENXML": return ExportFormat.Excel2007;
                case "WORD": return ExportFormat.Word;
                case "WORDOPENXML": return ExportFormat.Word2007;

                default:
                    return ExportFormat.PDF;
            }
        }

        /// <summary>
        /// Gets the string export format of the specified enum.
        /// </summary>
        /// <param name="f">export format enum</param>
        /// <returns>enum equivalent string export format</returns>
        public static string GetExportFormatString(ExportFormat f)
        {
            switch (f)
            {
                case ExportFormat.Image: return "IMAGE";
                case ExportFormat.PDF: return "PDF";
                case ExportFormat.Excel: return "EXCEL";
                case ExportFormat.Excel2007: return "EXCELOPENXML";
                case ExportFormat.Word: return "WORD";
                case ExportFormat.Word2007: return "WORDOPENXML";

                default:
                    return "PDF";
            }
        }
    }

    public enum ExportFormat
    {
        /// <summary>TIFF image</summary>
        Image,
        /// <summary>PDF</summary>
        PDF,
        /// <summary>Excel</summary>
        Excel,
        /// <summary>Excel 2007-2010</summary>
        Excel2007,
        /// <summary>Word</summary>
        Word,
        /// <summary>Word 2007-2010</summary>
        Word2007
    }
}