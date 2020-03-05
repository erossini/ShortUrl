﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PSC.Shorturl.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Click",
				url: "{segment}",
				defaults: new { controller = "Url", action = "Click" }
			);

            routes.MapRoute(
                name: "Stats",
                url: "{segment}/stats",
                defaults: new { controller = "Url", action = "Stats" }
            );

            routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Url", action = "Index", id = UrlParameter.Optional }
			);
        }
    }
}