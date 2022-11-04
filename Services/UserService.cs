using G_ON_WEBAPP.Models;
using System.Text.Json;

namespace G_ON_WEBAPP.Services
{
    public class UserService
    {
        public static ApiResponse Authlogin(string host, string email, string password)
        {
            var sBody = JsonSerializer.Serialize(new { email, password });
            return ApiService.Post($"{host}/v1/g-user/authentication/login", null, null, sBody);
        }

        public static ApiResponse Authlogout(string host, string token)
        {
            return ApiService.Post($"{host}/v1/g-user/authentication/revoke-token", "Cookie", $"gtk={token}", null); ;
        }

        public static ApiResponse GetApp(string host, string token, string userID)
        {
            return ApiService.Get($"{host}/v1/g-user/app/registration-info?userid={userID}", "Cookie", $"gtk={token}"); ;
        }

        public static ApiResponse GetInfoUser(string host, string token, string userName, string companyID)
        {
            return ApiService.Get($"{host}/v1/g-user/user/get?userName={userName}&companyID={companyID}", "Cookie", $"gtk={token}"); ;
        }

        public static ApiResponse GetGroup(string host, string token, string? groupName, string companyID)
        {

            return ApiService.Get($"{host}/v1/g-user/group/get?groupName={groupName}&companyID={companyID}", "Cookie", $"gtk={token}"); ;
        }

        public static ApiResponse GetListUser(string host, string token, string companyID)
        {
            return ApiService.Get($"{host}/v1/g-user/user/getlist?companyID={companyID}", "Cookie", $"gtk={token}"); ;
        }

        public static ApiResponse UpdateUser(string host, string token, Users user)
        {
            var sBody = JsonSerializer.Serialize<Users>(user);
            return ApiService.Put($"{host}/v1/g-user/user/update", "Cookie", $"gtk={token}", sBody);

        }

        public static ApiResponse CreateUser(string host, string token, Users user)
        {
            var sBody = JsonSerializer.Serialize<Users>(user);
            return ApiService.Post($"{host}/v1/g-user/user/create", "Cookie", $"gtk={token}", sBody);
        }

        public static ApiResponse DeleteUser(string host, string token, Int64 userID)
        {
            return ApiService.Delete($"{host}/v1/g-user/user/delete?userid={userID}", "Cookie", $"gtk={token}", null);
        }

        public static ApiResponse CreateGroup(string host, string token, Group group)
        {
            var sBody = JsonSerializer.Serialize<Group>(group);
            return ApiService.Post($"{host}/v1/g-user/group/create", "Cookie", $"gtk={token}", sBody);
        }

        public static ApiResponse UpdateGroup(string host, string token, Group group)
        {
            var sBody = JsonSerializer.Serialize<Group>(group);
            return ApiService.Put($"{host}/v1/g-user/group/update", "Cookie", $"gtk={token}", sBody);

        }

        public static ApiResponse DeleteGroup(string host, string token, Int64 groupID)
        {
            return ApiService.Delete($"{host}/v1/g-user/group/delete?groupId={groupID}", "Cookie", $"gtk={token}", null);
        }

        public static ApiResponse GetTitleJob(string host, string token, Int64? titleID)
        {
            return ApiService.Get($"{host}/v1/g-user/title/get?titleId={titleID}", "Cookie", $"gtk={token}");
        }

        public static ApiResponse GetRoleList(string host, string token)
        {
            return ApiService.Get($"{host}/v1/g-user/permission/get-role-list", "Cookie", $"gtk={token}");
        }

        public static ApiResponse GetUserGroup(string host, string token, string userName, string companyID)
        {
            return ApiService.Get($"{host}/v1/g-user/user-group/get-list-member?groupName={userName}&companyID={companyID}", "Cookie", $"gtk={token}");
        }

        public static ApiResponse DeleteUserGroup(string host, string token, Int64 userID, Int64 groupID)
        {
            return ApiService.Delete($"{host}/v1/g-user/user-group/remove-users?groupID={groupID}&userID={userID}", "Cookie", $"gtk={token}","");
        }

        public static ApiResponse AddUserGroup(string host, string token, UserGroup userGroup)
        {
            var sBody = JsonSerializer.Serialize<UserGroup>(userGroup);
            return ApiService.Post($"{host}/v1/g-user/user-group/add-user", "Cookie", $"gtk={token}", sBody);
        }

        public static ApiResponse GetListGroupsUser(string host,string token,Int64 userID, string companyID)
        {
            return ApiService.Get($"{host}/v1/g-user/group/get-list-group?userID={userID}&companyID={companyID}", "Cookie", $"gtk={token}");

        }
    }

}
