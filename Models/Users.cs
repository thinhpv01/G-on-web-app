namespace G_ON_WEBAPP.Models
{
    public class Users
    {
        public Int64 userID { set; get; }

        public int? companyID { set; get; }

        public Int64? titleID { set; get; }

        public string? firstName { set; get; }

        public string? lastName { set; get; }

        public string? userName { set; get; }

        public string? email { set; get; }
        public string? passwordHash { set; get; }
        public DateTime? dateOfBirth { set; get; }

        public string? phone { set; get; }

        public string? address { set; get; }

        public bool? activated { set; get; }

        public string? tag { set; get; }

        public string? languageCode { set; get; }

        public int? timeZone { set; get; }
    }
}
