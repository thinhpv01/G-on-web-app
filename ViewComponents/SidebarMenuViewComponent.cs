using G_AUTHORIZATION.Helpers;
using G_ON_WEBAPP.Models;
using G_ON_WEBAPP.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WEB_APP.ViewComponents
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sHome = string.Empty, sInfo = string.Empty, sApp = string.Empty, sUser = string.Empty, sGroup = string.Empty, sLogout = string.Empty;
            string sUserName = await HashText.DecryptString(HttpContext.Request.Cookies["UserName"] ?? "");
            try
            {
                var sCookie = Request.Cookies["trs_g-on"];
                if (!string.IsNullOrEmpty(sCookie))
                {
                    var rawTrans = await HashText.Decompress(sCookie.Substring(16));
                    List<Translation> trans_g_on = JsonSerializer.Deserialize<List<Translation>>(rawTrans);

                    sHome = TranslationService.GetText(trans_g_on, "g_on_menu_trang_chu");
                    sInfo = TranslationService.GetText(trans_g_on, "g_on_menu_thong_tin_ca_nhan");
                    sApp = TranslationService.GetText(trans_g_on, "g_on_menu_cac_ung_dung");
                    sUser = TranslationService.GetText(trans_g_on, "g_on_menu_thanh_vien");
                    sGroup = TranslationService.GetText(trans_g_on, "g_on_menu_nhom");
                    sLogout = TranslationService.GetText(trans_g_on, "g_on_menu_dang_xuat");
                }
            }
            catch { }

            (string sHome, string sInfo, string sApp, string sUser, string sGroup, string sLogout, string sUserName) ModelData = (sHome, sInfo, sApp, sUser, sGroup, sLogout, sUserName);

            return View(ModelData);
        }
    }
}