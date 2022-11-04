using Microsoft.AspNetCore.Mvc.RazorPages;

namespace G_ON_WEBAPP.Services
{
    public class Cookie
    {
        public static string Get(PageModel pageModel, string key, string value)
        {
            return (string.IsNullOrWhiteSpace(pageModel.Request.Cookies[key])) ? SetAndReturn(pageModel, key, value) : pageModel.Request.Cookies[key];
        }

        public static void Set(PageModel pageModel, string key, string value)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(20)
            };
            var cookieOptionsSub = new CookieOptions
            {
                Domain = GetSubDomain(pageModel.Request.Host.Value),
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(20)
            };
            pageModel.Response.Cookies.Append(key, value, cookieOptions);
            pageModel.Response.Cookies.Append(key, value, cookieOptionsSub);
        }

        private static string SetAndReturn(PageModel pageModel, string key, string value)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(20)
            };
            var cookieOptionsSub = new CookieOptions
            {
                Domain = GetSubDomain(pageModel.Request.Host.Value),
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(20)
            };
            pageModel.Response.Cookies.Append(key, value, cookieOptions);
            pageModel.Response.Cookies.Append(key, value, cookieOptionsSub);
            return value;
        }
        private static string SetAndReturn(PageModel pageModel, string key, string value, int expiredTTL)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddDays(expiredTTL)
            };
            var cookieOptionsSub = new CookieOptions
            {
                Domain = GetSubDomain(pageModel.Request.Host.Value),
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(20)
            };
            pageModel.Response.Cookies.Append(key, value, cookieOptions);
            pageModel.Response.Cookies.Append(key, value, cookieOptionsSub);
            return value;
        }

        private static string GetSubDomain(string host)
        {
            var domain = host.Split('.');
            if (domain.Length < 2)
                return host;
            else
                return $".{domain[domain.Length - 2]}.{domain[domain.Length - 1]}";

        }
    }
}
