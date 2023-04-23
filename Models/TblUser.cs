namespace LibManagementAPI.Models
{
    public partial class TblUser
    {
        public int UserId { get; set; }
        public string? UserDisplayName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public string? UserRole { get; set; }
        public DateTime? UserCreatedDate { get; set; }
    }
}
