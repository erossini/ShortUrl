using PSC.Shorturl.Web.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace PSC.Shorturl.Web.Filters
{
	public class ShorturlApiErrorFilter : ExceptionFilterAttribute
	{
		public override void OnException(HttpActionExecutedContext ctx)
		{
			HttpStatusCode code = HttpStatusCode.InternalServerError;
			var ex = ctx.Exception;

			if (ex is ShorturlConflictException)
			{
				code = HttpStatusCode.Conflict;
			}
			else if (ex is ShorturlNotFoundException)
			{
				code = HttpStatusCode.NotFound;
			}
			else if (ex is ArgumentException)
			{
				code = HttpStatusCode.BadRequest;
			}

			ctx.Response = ctx.Request.CreateResponse(code);
		}
	}
}