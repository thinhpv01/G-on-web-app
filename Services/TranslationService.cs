using G_AUTHORIZATION.Helpers;
using G_ON_WEBAPP.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace G_ON_WEBAPP.Services
{
    public class TranslationService
    {
        public static string GetText(List<Translation> trans, string TranslationCode)
        {
            if (trans == null || string.IsNullOrWhiteSpace(TranslationCode)) return string.Empty;
            return trans.FirstOrDefault(x => x.translationCode == TranslationCode)?.text ?? TranslationCode;
        }

        public static string GetTranslation(string uri, string translationCode, string lang)
        {
            var apiResponse = ApiService.Get($"{uri}/v1/g-trans/translate/get?translationCode={translationCode}&languageCode={lang}", null, null);
            if (apiResponse != null && apiResponse.StatusCode == 200)
                return apiResponse.Data;

            return "";
        }

        public static string GetTranslationByApp(string? uri, string? appCode, string? lang)
        {
            var apiResponse = ApiService.Get($"{uri}/v1/g-trans/translate/getlist?appCode={appCode}&languageCode={lang}", null, null);
            if (apiResponse != null && apiResponse.StatusCode == 200)
                return apiResponse.Data;

            return "";
        }

        public static string CheckTranslationByApp(string uri, string appCode, string languageCode, string version)
        {
            var apiResponse = ApiService.Get($"{uri}/v1/g-trans/translate/checkversion?appCode={appCode}&languageCode={languageCode}&version={version}", null, null);
            if (apiResponse != null && apiResponse.StatusCode == 200)
                return apiResponse.Data;

            return "";
        }

        public static async Task<List<Translation>> Translator(PageModel pageModel, string uri_g_trs, string AppCode, string lang, string CookieName)
        {
            string rawTrans;
            if (string.IsNullOrWhiteSpace(pageModel.Request.Cookies[CookieName]))
            {
                // Get new
                rawTrans = TranslationService.GetTranslationByApp(uri_g_trs, AppCode, lang);
                if (rawTrans != "")
                {
                    Cookie.Set(pageModel, CookieName, "v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + await HashText.Compress(rawTrans));
                }
            }
            else
            {
                var sCookie = pageModel.Request.Cookies[CookieName];
                if (sCookie.Substring(0, 2) == "v=")
                {
                    // Check version
                    rawTrans = TranslationService.CheckTranslationByApp(uri_g_trs, AppCode, lang, sCookie.Substring(2, 14));
                    if (rawTrans != "")
                        // Get New version
                        Cookie.Set(pageModel, CookieName, "v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + await HashText.Compress(rawTrans));
                    else
                        // Use Current Version
                        rawTrans = await HashText.Decompress(sCookie.Substring(16));
                }
                else
                {
                    // Get new
                    rawTrans = TranslationService.GetTranslationByApp(uri_g_trs, AppCode, lang);
                    if (rawTrans != "")
                    {
                        Cookie.Set(pageModel, CookieName, "v=" + DateTime.Now.ToString("yyyyMMddHHmmss") + await HashText.Compress(rawTrans));
                    }
                }
            }
            return JsonSerializer.Deserialize<List<Translation>>(rawTrans);
        }
    }
}
