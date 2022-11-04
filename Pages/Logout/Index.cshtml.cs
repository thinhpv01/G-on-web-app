using G_AUTHORIZATION.Helpers;
using G_ON_WEBAPP.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace WEB_APP.Pages.Logout
{
    public class IndexModel : PageModel
    {
        public async void OnGet()
        {
            try
            {
                UserService.Authlogout(await HashText.DecryptString(Request.Cookies["g-us"].ToString()), Request.Cookies["gtk"]);
            }
            catch
            {

            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(-50)
            };
            var cookieOptionsSub = new CookieOptions
            {
                Domain = GetSubDomain(Request.Host.Value),
                HttpOnly = true,
                Expires = DateTime.Now.AddYears(-50)
            };

            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie, cookieOptions);
                Response.Cookies.Delete(cookie, cookieOptionsSub);
            }

            Response.Redirect("../login?url=" + HttpUtility.UrlEncode(Request.Query["url"].ToString()));
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
