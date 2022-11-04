namespace G_ON_WEBAPP.Models
{
    public class AuthenticateResponse
    {
        public Int64 id { get; set; }
        public int company { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string language { get; set; }
        public Int64 tokenID { get; set; }
        public string jwtToken { get; set; }
        public List<Server> servers { get; set; }
        public string userRoles { get; set; }
    }

    public class Server
    {
        public string hostURL { get; set; }
        public string appCode { get; set; }
        public string? serverIP { get; set; }
        public string? serverName { get; set; }
        public Int64 totalUsers { get; set; }
        public Int64 activeUsers { get; set; }
    }
}
