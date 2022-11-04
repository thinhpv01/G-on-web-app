namespace G_ON_WEBAPP.Models
{
    public class JobTitle
    {
        public Int64 titleID { get; set; }
        public int companyID { get; set; }
        public string? titleName { get; set; }
        public int? priority { get; set; }
        public Int64? updateBy { get; set; }
        public DateTime? updateDate { get; set; }
    }
}
