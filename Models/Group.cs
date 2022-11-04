namespace G_ON_WEBAPP.Models
{
    public class Group
    {
        public Int64 groupID { get; set; }
        public int companyID { get; set; }
        public Int64? groupTypeID { get; set; }
        public string? groupName { get; set; }
        public int? priority { get; set; }
        public Int64? parentID { get; set; }
        public string? tag { get; set; }
        public Int64? updateBy { get; set; }
        public DateTime? updateDate { get; set; }
    }
}
