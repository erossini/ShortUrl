using PSC.Shorturl.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PSC.Shorturl.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			config.Filters.Add(new ShorturlApiErrorFilter());

            // Web API routes
            config.MapHttpAttributeRoutes();
		}
	}
}
