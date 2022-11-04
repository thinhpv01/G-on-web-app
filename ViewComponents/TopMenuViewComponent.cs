using G_AUTHORIZATION.Helpers;
using G_ON_WEBAPP.Models;
using G_ON_WEBAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace WEB_APP.ViewComponents
{
    public class TopMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sNotification = string.Empty;
            string sUserName = await HashText.DecryptString(HttpContext.Request.Cookies["UserName"]);
            string sFullName = await HashText.DecryptString(HttpContext.Request.Cookies["FullName"]);
            string sPageTitle = string.Empty;

            try
            {
                var sCookie = Request.Cookies["trs_g-on"];
                if (!string.IsNullOrEmpty(sCookie))
                {
                    var rawTrans = await HashText.Decompress(sCookie.Substring(16));
                    List<Translation> trans_g_on = JsonSerializer.Deserialize<List<Translation>>(rawTrans);

                    sNotification = TranslationService.GetText(trans_g_on, "g_on_menu_notification");

                    if (ViewContext.RouteData.Values["route"] == null)
                    {
                        sPageTitle = "<i data-feather='home' width='20'></i> ";
                        ViewData["Title"] = TranslationService.GetText(trans_g_on, "g_on_menu_trang_chu");
                    }
                    else
                    {
                        switch (ViewContext.RouteData.Values["route"].ToString().ToLower())
                        {
                            case "user":
                                sPageTitle = "<i data-feather='user' width='20'></i> "; break;
                            case "users":
                                sPageTitle = "<i data-feather='users' width='20'></i> "; break;
                            case "groups":
                                sPageTitle = "<i data-feather='grid' width='20'></i> "; break;
                            case "apps":
                                sPageTitle = "<i data-feather='layers' width='20'></i> "; break;
                            default:
                                sPageTitle = "<i data-feather='home' width='20'></i> "; break;
                        }
                    }
                    sPageTitle += ViewData["Title"];
                }
            }
            catch { }

            (string sNotification, string sUserName, string sFullName, string sPageTitle) ModelData = (sNotification, sUserName, sFullName, sPageTitle);

            return View(ModelData);
        }
    }
}