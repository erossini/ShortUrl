using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSC.Shorturl.Web.Utility.Models
{
    public class ErrorModel
    {
        public string ErrorDetail { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorStack { get; set; }
    }
}
