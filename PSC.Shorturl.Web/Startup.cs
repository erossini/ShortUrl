using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PSC.Shorturl.Web.Startup))]
 namespace PSC.Shorturl.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}