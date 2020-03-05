using PSC.Shorturl.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace PSC.Shorturl.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new ShorturlErrorFilter());
            filters.Add(new HandleErrorAttribute());
        }
	}
}