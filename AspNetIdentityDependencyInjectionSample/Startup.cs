using AspNetIdentityDependencyInjectionSample.Helpers;
using AspNetIdentityDependencyInjectionSample.IocConfig;
using AspNetIdentityDependencyInjectionSample.ServiceLayer.Contracts;
using AspNetIdentityDependencyInjectionSample.ServiceLayer;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin;
using Owin;
using StructureMap.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace AspNetIdentityDependencyInjectionSample
{
    //[assembly: OwinStartupAttribute(typeof(AspNetIdentityDependencyInjectionSample.Startup))]
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            configureAuth(app);
        }

        private static void configureAuth(IAppBuilder app)
        {
            var container = SmObjectFactory.Container;
            container.Configure(config =>
            {
                config.For<IDataProtectionProvider>()
                      .HybridHttpOrThreadLocalScoped()
                      .Use(() => app.GetDataProtectionProvider());
            });
            container.GetInstance<IApplicationUserManager>().SeedDatabase();

            // This is necessary for `GenerateUserIdentityAsync` and `SecurityStampValidator` to work internally by ASP.NET Identity 2.x
            app.CreatePerOwinContext(() => (ApplicationUserManager)container.GetInstance<IApplicationUserManager>());

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieName = ".mySampleAppCookieName",
                ExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.
                    OnValidateIdentity = context =>
                    {
                        if(shouldIgnoreRequest(context)) // How to ignore Authentication Validations for static files in ASP.NET Identity
                        {
                            return Task.FromResult(0);
                        }
                        return container.GetInstance<IApplicationUserManager>().OnValidateIdentity().Invoke(context);
                    }
                },
                SlidingExpiration = false,
                CookieManager = new SystemWebCookieManager()
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            app.MapSignalR();
        }

        private static bool shouldIgnoreRequest(CookieValidateIdentityContext context)
        {
            string[] reservedPath =
            {
                "/__browserLink",
                "/img",
                "/fonts",
                "/Scripts",
                "/Content",
                "/Uploads",
                "/Images"
            };
            return reservedPath.Any(path => context.OwinContext.Request.Path.Value.StartsWith(path, StringComparison.OrdinalIgnoreCase)) ||
                               BundleTable.Bundles.Select(bundle => bundle.Path.TrimStart('~')).Any(bundlePath => context.OwinContext.Request.Path.Value.StartsWith(bundlePath,StringComparison.OrdinalIgnoreCase));
        }
    }
}