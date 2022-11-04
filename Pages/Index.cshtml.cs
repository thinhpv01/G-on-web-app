using G_AUTHORIZATION.Helpers;
using G_ON_WEBAPP.Models;
using G_ON_WEBAPP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace G_ON_WEBAPP.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Models.AppSettings _appSettings;

        public IndexModel(IOptions<Models.AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string Route { get; set; }

        public List<Translation> trans_g_on { get; set; }
        public List<Apps> Apps { get; set; }
        [BindProperty]
        public Users User { get; set; }
        [BindProperty]
        public Group GroupModel { get; set; }
        [BindProperty]
        public JobTitle? JobTitleModel { get; set; }
        public List<Users> Users { get; set; }
        public List<Users> UserGroup { get; set; }
        public List<Group> Groups { get; set; }
        public string? sUserID { get; set; }
        private string sCompany { get; set; }
        private string Token { get; set; }
        public string? sUserRoleID { get; set; }
        private string lang { get; set; }
        private string g_us { get; set; }
        private string g_trs { get; set; }

        public List<UserRole>? userRoles { get; set; }
        public string? sUserName { get; set; }
        public List<RoleModel> RolesList { get; set; }

        [BindProperty]
        public Int64 userID { get; set; }
        [BindProperty]
        public Int64 groupID { get; set; }
        public string html { get; set; }

        public async void OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
                string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                g_trs = await HashText.DecryptString(Cookie.Get(this, "g-trs", _appSettings.g_trs));
                lang = Cookie.Get(this, "lang", "vi");

                trans_g_on = await TranslationService.Translator(this, g_trs, "g-on", lang, "trs_g-on");

                sUserID = await HashText.DecryptString(HttpContext.Request.Cookies["UserID"]);

                //string sFullName = await HashText.DecryptString(HttpContext.Request.Cookies["FullName"]);
                sCompany = await HashText.DecryptString(HttpContext.Request.Cookies["company"]);

                g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
                Token = HttpContext.Request.Cookies["gtk"];
                var sUserRole = await HashText.DecryptString(HttpContext.Request.Cookies["UserRl"]);
                if (!string.IsNullOrWhiteSpace(sUserRole))
                {
                    userRoles = JsonSerializer.Deserialize<List<UserRole>>(sUserRole);
                    sUserRoleID = userRoles.FirstOrDefault(x => x.AppCode == "g-us")?.RoleID.ToString() ?? "";
                }
                GetRoleLisr();

                if (RouteData.Values["route"] == null)
                {
                    ViewData["Title"] = TranslationService.GetText(trans_g_on, "g_on_menu_trang_chu");
                }
                else
                {

                    Route = RouteData.Values["route"].ToString().ToLower();

                    switch (Route)
                    {
                        case "user":

                            UserInfoPage();
                            GetListGroupsUser();
                            if (User.titleID != null)
                            {
                                GetTitleJob(User.titleID);
                            }
                            break;
                        case "users":

                            ListUserPage();
                            break;
                        case "groups":
                            ViewData["Title"] = TranslationService.GetText(trans_g_on, "g_on_menu_nhom");
                            if (RouteData.Values["data"] == null)
                            {
                                ListGroupPage();
                            }
                            else
                            {
                                GetUserGroupPage();
                                GetGroup(RouteData.Values["data"].ToString());
                                ListUserPage();
                           
                            }


                            break;
                        case "apps": AppsPage(); break;
                        default:
                            ViewData["Title"] = TranslationService.GetText(trans_g_on, "g_on_menu_trang_chu");
                            break;
                    }

                }
            }
        }
        public async Task OnpostAsync()
        {
            g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
            Token = HttpContext.Request.Cookies["gtk"];
            var sUserRole = await HashText.DecryptString(HttpContext.Request.Cookies["UserRl"]);
            if (!string.IsNullOrWhiteSpace(sUserRole))
            {
                var userRoles = JsonSerializer.Deserialize<List<UserRole>>(sUserRole);
                sUserRoleID = userRoles.FirstOrDefault(x => x.AppCode == "g-us")?.RoleID.ToString() ?? "";

            }
            sUserID = await HashText.DecryptString(HttpContext.Request.Cookies["UserID"]);
            Route = RouteData.Values["route"].ToString().ToLower();

            switch (Route)
            {
                case "user":
                    UpdateUserPage();

                    break;
                case "groups":
                    if (RouteData.Values["data"] == null)
                    {
                        CreateGroupPage();
                    }
                    break;
            }

        }



        private void AppsPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
                string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                ViewData["Title"] = TranslationService.GetText(trans_g_on, "g_on_menu_cac_ung_dung");
                ApiResponse apiResponse = UserService.GetApp(g_us, Token, sUserID);
                if (apiResponse.StatusCode == 200)
                    Apps = JsonSerializer.Deserialize<List<Apps>>(apiResponse.Data);

            }
        }
        //Get info User 
        public void UserInfoPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
                 string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                sUserName = RouteData.Values["data"].ToString().ToLower();

                ApiResponse apiResponse = UserService.GetInfoUser(g_us, Token, sUserName, sCompany);
                if (apiResponse.StatusCode == 200)
                {
                    User = JsonSerializer.Deserialize<Users>(apiResponse.Data);

                }

            }
        }

        // Get List User In Company
        public async void ListUserPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
                 string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                ViewData["Title"] = TranslationService.GetText(trans_g_on, "g_on_menu_thanh_vien");
                string sCompany = await HashText.DecryptString(HttpContext.Request.Cookies["company"]);
                ApiResponse apiResponse = UserService.GetListUser(g_us, Token, sCompany);
                if (apiResponse.StatusCode == 200)
                {
                    Users = JsonSerializer.Deserialize<List<Users>>(apiResponse.Data);
                }

            }
        }

        //Get List Group
        private void ListGroupPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
                string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {

                ApiResponse apiResponse = UserService.GetGroup(g_us, Token, "", sCompany);
                if (apiResponse.StatusCode == 200)
                {
                    Groups = JsonSerializer.Deserialize<List<Group>>(apiResponse.Data);
                }

            }
        }

        // Update user
        public async void UpdateUserPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
               string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                sUserName = RouteData.Values["data"].ToString().ToLower();
                sCompany = await HashText.DecryptString(HttpContext.Request.Cookies["company"]);
                User.companyID = int.Parse(sCompany);

                if ((int.Parse(sUserRoleID)) > 2)
                {
                    User.userName = sUserName;
                }
                User.languageCode = "vi";
                User.tag = "@" + User.userName;
                ApiResponse userResponse = UserService.GetInfoUser(g_us, Token, sUserName,sCompany);
                User.userID = JsonSerializer.Deserialize<Users>(userResponse.Data).userID;
                ApiResponse apiResponses = UserService.UpdateUser(g_us, Token, User);
                if (apiResponses.StatusCode == 200)
                {
                    Response.Redirect($"/user/{User.userName}");
                }
            }
            OnGetAsync();
        }

        // Create User
        public async void OnPostCreateUserPage()
        {
            g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
            Token = HttpContext.Request.Cookies["gtk"];
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
             string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                sCompany = await HashText.DecryptString(HttpContext.Request.Cookies["company"]);
                User.companyID = int.Parse(sCompany);
                User.tag = $"@{User.userName}";
                User.languageCode = "vi";
                User.activated = true;
                ApiResponse apiResponses = UserService.CreateUser(g_us, Token, User);
                if (apiResponses.StatusCode != 200)
                {
                

                }

            }
            Response.Redirect("/users");



        }

        // Delete User
        public async Task<IActionResult> OnPostDeleteUserPage()
        {

            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
         string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
                Token = HttpContext.Request.Cookies["gtk"];
                ApiResponse apiResponses = UserService.DeleteUser(g_us, Token, userID);
                if (apiResponses.StatusCode == 200)
                {

                }

            }
            return RedirectToPage("./Index");
        }

        //Get a Group
        public void GetGroup(string groupName)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
        string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                ApiResponse apiResponse = UserService.GetGroup(g_us, Token, groupName, sCompany);
                if (apiResponse.StatusCode == 200)
                {
                    GroupModel = JsonSerializer.Deserialize<Group>(apiResponse.Data);
                }
            }
        }

        //Create Group
        public async void CreateGroupPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
                  string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                sCompany = await HashText.DecryptString(HttpContext.Request.Cookies["company"]);
                GroupModel.companyID = int.Parse(sCompany);

                sUserID = await HashText.DecryptString(HttpContext.Request.Cookies["UserID"]);
                GroupModel.updateBy = Int64.Parse(sUserID);
                ApiResponse apiResponse = UserService.CreateGroup(g_us, Token, GroupModel);
                if (apiResponse.StatusCode == 200)
                {
                    Response.Redirect("/groups");
                }
                else
                {


                    html = "1";
                }
            }

            OnGetAsync();
        }


        //Update Group
        public async void OnPostUpdateGroupPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
                 string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
                Token = HttpContext.Request.Cookies["gtk"];
                GroupModel.updateBy = Int64.Parse(await HashText.DecryptString(HttpContext.Request.Cookies["UserID"]));
                GroupModel.companyID = int.Parse(await HashText.DecryptString(HttpContext.Request.Cookies["company"]));
                ApiResponse apiResponse = UserService.UpdateGroup(g_us, Token, GroupModel);
                if (apiResponse.StatusCode == 200)
                {

                }

            }
            Response.Redirect("/groups");
            OnGetAsync();
        }

        //Delete Group
        public async void OnPostDeleteGroupPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
               string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
                Token = HttpContext.Request.Cookies["gtk"];
                ApiResponse apiResponse = UserService.DeleteGroup(g_us, Token, groupID);
                if (apiResponse.StatusCode == 200)
                {

                }
            }
            Response.Redirect("/groups");


        }

        //Get title job
        private void GetTitleJob(Int64? titleID)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
            string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                ApiResponse apiResponse = UserService.GetTitleJob(g_us, Token, titleID);
                if (apiResponse.StatusCode == 200)
                {
                    JobTitleModel = JsonSerializer.Deserialize<JobTitle>(apiResponse.Data);
                }
            }
        }

        public string GetTitleNamex(Int64? titleID)
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
            string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                ApiResponse apiResponse = UserService.GetTitleJob(g_us, Token, titleID);
                if (apiResponse.StatusCode == 200)
                {
                    return JsonSerializer.Deserialize<JobTitle>(apiResponse.Data).titleName;
                }

            }
            return "";
        }

        private void GetRoleLisr()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
           string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                ApiResponse apiResponse = UserService.GetRoleList(g_us, Token);
                if (apiResponse.StatusCode == 200)
                {
                    RolesList = JsonSerializer.Deserialize<List<RoleModel>>(apiResponse.Data);
                }
            }
        }

        public string getString(string a)
        {
            return a;
        }

        public void GetUserGroupPage()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
             string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {

                string sGroupName = RouteData.Values["data"].ToString().ToLower();
                ApiResponse apiResponse = UserService.GetUserGroup(g_us, Token, sGroupName, sCompany);
                if (apiResponse.StatusCode == 200)
                {
                    UserGroup = JsonSerializer.Deserialize<List<Users>>(apiResponse.Data);

                }

            }
        }

        public async void OnPostDeleteUserGroupPage()
        {
            string groupName = RouteData.Values["data"].ToString();
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
            string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
                Token = HttpContext.Request.Cookies["gtk"];
             
                ApiResponse apiResponse = UserService.DeleteUserGroup(g_us, Token, userID, groupID);
            }
            Response.Redirect($"/groups/{groupName}");
          
        }

        public List<Users> UserGroupFillter()
        {
            var userGroupCheckList = new List<Users>();
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
            string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
              
                var iDUsersCompany = new List<Int64>();
                foreach(var user in Users)
                {
                    iDUsersCompany.Add(user.userID);
                }

                var iDUserGroup = new List<Int64>();
                foreach (var user in UserGroup)
                {
                    iDUserGroup.Add(user.userID);
                }
                var  iDFillter = iDUsersCompany.Except(iDUserGroup);
              
                foreach(var id in iDFillter)
                {
                    var result = Users.FirstOrDefault(x=>x.userID==id);
                    if (result != null)
                    {
                        userGroupCheckList.Add(result);
                    }
                }
                
            }
            return userGroupCheckList;
        }
        public async void OnPostAddUserGroup()
        {
            string groupName = RouteData.Values["data"].ToString();
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
           string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                g_us = await HashText.DecryptString(Cookie.Get(this, "g-us", _appSettings.g_us));
                Token = HttpContext.Request.Cookies["gtk"];
                var ids = Request.Form["id"];

                foreach(var id in ids)
                {
                    var userGroupTemp = new UserGroup()
                    {
                        UserID = Int64.Parse(id),
                        GroupID = groupID,
                        UpdateBy = Int64.Parse(await HashText.DecryptString(HttpContext.Request.Cookies["UserID"]))
                    };
                    ApiResponse apiResponse= UserService.AddUserGroup(g_us, Token, userGroupTemp);
                }

            }
            Response.Redirect($"/groups/{groupName}");
        }

        public void GetListGroupsUser()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["gtk"]) ||
          string.IsNullOrWhiteSpace(HttpContext.Request.Cookies["trs_g-on"]))
                Response.Redirect("/login");
            else
            {
                ApiResponse apiResponse = UserService.GetListGroupsUser(g_us, Token, User.userID, sCompany);
                if (apiResponse.StatusCode == 200)
                {
                    Groups = JsonSerializer.Deserialize<List<Group>>(apiResponse.Data);
                }
            }
        }
    }
}