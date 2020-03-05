using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Twitter;
using Owin;
using Microsoft.Owin.Security;

namespace PSC.Shorturl.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseMicrosoftAccountAuthentication(
             clientId: "000000004C179CE5",
             clientSecret: "ptEJ6IRtmyoqMgLUvTYQWyn8rJ7gUC2V");

            app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            {
                ConsumerKey = "WqkPiLaDaZIrpO7fZj4TNPEfh",
                ConsumerSecret = "nyJfBMzi70tmcCERk9hQpQjV9PHqKDVOxITXnYMWMFubQDeByM",
                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
                    {
                        // VeriSign Class 3 Secure Server CA - G2
                        "A5EF0B11CEC04103A34A659048B21CE0572D7D47",
                        // VeriSign Class 3 Secure Server CA - G3
                        "0D445C165344C1827E1D20AB25F40163D8BE79A5",
                        // VeriSign Class 3 Public Primary Certification Authority - G5
                        "7FD365A7C2DDECBBF03009F34339FA02AF333133", 
                        // Symantec Class 3 Secure Server CA - G4
                        "39A55D933676616E73A761DFA16A7E59CDE66FAD", 
                        // Symantec Class 3 EV SSL CA - G3
                        "‎add53f6680fe66e383cbac3e60922e3b4c412bed", 
                        // VeriSign Class 3 Primary CA - G5
                        "4eb6d578499b1ccf5f581ead56be3d9b6744a5e5", 
                        // DigiCert SHA2 High Assurance Server C‎A 
                        "5168FF90AF0207753CCCD9656462A212B859723B", 
                        // DigiCert High Assurance EV Root CA
                        "B13EC36903F8BF4701D498261A0802EF63642BC3"
                    })
            });

            app.UseFacebookAuthentication(
             appId: "220312764968956",
             appSecret: "9f1051ccda792f6266612f70f9eb8cee");

            app.UseGoogleAuthentication(clientId: "346925793832-fomgjpci6vrukukfekgma1f16ri2l2e2.apps.googleusercontent.com",
                                        clientSecret: "s5lUbiRMzYAkUvEsMySUqtHL");
        }
    }
}