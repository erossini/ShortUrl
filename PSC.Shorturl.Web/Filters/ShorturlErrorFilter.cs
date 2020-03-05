using log4net;
using PSC.Shorturl.Web.Exceptions;
using PSC.Shorturl.Web.Utility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PSC.Shorturl.Web.Filters
{
	public class ShorturlErrorFilter : HandleErrorAttribute
	{
        private ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(ExceptionContext filterContext)
		{
			HttpStatusCode code = HttpStatusCode.InternalServerError;
			var ex = filterContext.Exception;
			string viewName = "Error500";

            ErrorModel err = new ErrorModel();
            err.ErrorDetail = filterContext.Exception.Message;
            err.ErrorStack = filterContext.Exception.StackTrace;
            err.ErrorSource = filterContext.Exception.Source;

            // write error in the log
            log.Error(err.ErrorDetail);
            log.Error(err.ErrorStack);
            log.Error(err.ErrorSource);

			if (ex is ShorturlNotFoundException)
			{
				code = HttpStatusCode.NotFound;
				viewName = "Error404";
			}
			if (ex is ShorturlConflictException)
			{
				code = HttpStatusCode.Conflict;
				viewName = "Error409";
			}
			if (ex is ArgumentException)
			{
				code = HttpStatusCode.BadRequest;
				viewName = "Error400";
			}

			filterContext.Result = new ViewResult()
			{
				ViewName = viewName,
                ViewData = new ViewDataDictionary(err)
            };

			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = (int)code;
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;  
		}
	}
}