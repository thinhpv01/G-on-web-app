using G_ON_WEBAPP.Models;
using System.Text;
using System.Text.Json;

namespace G_ON_WEBAPP.Services
{
    public class ApiService
    {
        public static ApiResponse Get(string uri, string? headerKey, string? headerValue)
        {
            var response = new ApiResponse();

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                if (headerKey != null && headerValue != null)
                    client.DefaultRequestHeaders.Add(headerKey, headerValue);
                var result = client.GetAsync(uri).Result;

                response.StatusCode = (int)result.StatusCode;
                if (response.StatusCode == StatusCodes.Status401Unauthorized)
                    response.Message = "us_33_loi_xac_thuc"; 
                else if (response.StatusCode == StatusCodes.Status405MethodNotAllowed)
                    response.Message = "us_33_khong_co_quyen_su_dung";
                else
                    response.Data = result.Content.ReadAsStringAsync().Result;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "us_33_loi_cu_phap";
                return response;
            }
        }

        public static ApiResponse Post(string uri, string? headerKey, string? headerValue,string? apiBody)
        {
            var response = new ApiResponse();

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                if (headerKey != null && headerValue != null)
                    client.DefaultRequestHeaders.Add(headerKey, headerValue);
                StringContent? stringContent = new StringContent(apiBody?? "", Encoding.UTF8, "application/json");
                var result = client.PostAsync(uri, stringContent).Result;

                response.StatusCode = (int)result.StatusCode;
                if (response.StatusCode == StatusCodes.Status401Unauthorized)
                    response.Data = "us_33_loi_xac_thuc";
                else if (response.StatusCode == StatusCodes.Status405MethodNotAllowed)
                    response.Data = "us_33_khong_co_quyen_su_dung";
                else
                    response.Data = result.Content.ReadAsStringAsync().Result;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "us_33_loi_cu_phap";
                return response;
            }
        }

        public static ApiResponse Put(string uri, string? headerKey, string? headerValue, string? apiBody)
        {
            var response = new ApiResponse();

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                if (headerKey != null && headerValue != null)
                    client.DefaultRequestHeaders.Add(headerKey, headerValue);
                StringContent? stringContent = new StringContent(apiBody, Encoding.UTF8, "application/json");
                var result = client.PutAsync(uri, stringContent).Result;

                response.StatusCode = (int)result.StatusCode;
                if (response.StatusCode == StatusCodes.Status401Unauthorized)
                    response.Message = "us_33_loi_xac_thuc";
                else if (response.StatusCode == StatusCodes.Status405MethodNotAllowed)
                    response.Message = "us_33_khong_co_quyen_su_dung";
                else
                    response.Data = result.Content.ReadAsStringAsync().Result;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "us_33_loi_cu_phap";
                return response;
            }
        }

        public static ApiResponse Delete(string uri, string? headerKey, string? headerValue, string? apiBody)
        {
            var response = new ApiResponse();

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                if (headerKey != null && headerValue != null)
                    client.DefaultRequestHeaders.Add(headerKey, headerValue);
                if (apiBody!=null)
                {
                    StringContent? stringContent = new StringContent(apiBody, Encoding.UTF8, "application/json");
                }
                var result = client.DeleteAsync(uri).Result;

                response.StatusCode = (int)result.StatusCode;
                if (response.StatusCode == StatusCodes.Status401Unauthorized)
                    response.Message = "us_33_loi_xac_thuc";
                else if (response.StatusCode == StatusCodes.Status405MethodNotAllowed)
                    response.Message = "us_33_khong_co_quyen_su_dung";
                else
                    response.Data = result.Content.ReadAsStringAsync().Result;
                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = "us_33_loi_cu_phap";
                return response;
            }
        }
    }
}
