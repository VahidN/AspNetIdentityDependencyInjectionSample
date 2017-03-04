using System;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;

namespace AspNetIdentityDependencyInjectionSample.Helpers
{
    /// <summary>
    /// source-1: http://katanaproject.codeplex.com/wikipage?title=System.Web%20response%20cookie%20integration%20issues
    /// source-2: http://appetere.com/post/owinresponse-cookies-not-set-when-remove-an-httpresponse-cookie
    /// source-3: http://stackoverflow.com/questions/20737578/asp-net-sessionid-owin-cookies-do-not-send-to-browser
    /// </summary>
    public class SystemWebCookieManager : ICookieManager
    {
        public string GetRequestCookie(IOwinContext context, string key)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var webContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);
            var cookie = webContext.Request.Cookies[key];
            return cookie?.Value;
        }

        public void AppendResponseCookie(IOwinContext context, string key, string value, CookieOptions options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var webContext = context.Get<HttpContextBase>(typeof(HttpContextBase).FullName);

            bool domainHasValue = !string.IsNullOrEmpty(options.Domain);
            bool pathHasValue = !string.IsNullOrEmpty(options.Path);
            bool expiresHasValue = options.Expires.HasValue;

            var cookie = new HttpCookie(key, value);
            if (domainHasValue)
            {
                cookie.Domain = options.Domain;
            }
            if (pathHasValue)
            {
                cookie.Path = options.Path;
            }
            if (expiresHasValue)
            {
                cookie.Expires = options.Expires.Value;
            }
            if (options.Secure)
            {
                cookie.Secure = true;
            }
            if (options.HttpOnly)
            {
                cookie.HttpOnly = true;
            }

            webContext.Response.AppendCookie(cookie);
        }

        public void DeleteCookie(IOwinContext context, string key, CookieOptions options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            AppendResponseCookie(
                context,
                key,
                string.Empty,
                new CookieOptions
                {
                    Path = options.Path,
                    Domain = options.Domain,
                    Expires = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                });
        }
    }
}