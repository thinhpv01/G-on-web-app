namespace G_ON_WEBAPP.Models
{
    public class UserGroup
    {
        public Int64 GroupID { get; set; }
        public Int64 UserID { get; set; }
        public Int64? TitleID { get; set; }
        public Int64? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
