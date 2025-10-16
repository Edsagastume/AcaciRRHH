namespace AcaciRRHH.Web.Models
{
    public class ApplicationUserRole
    {
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int RoleId { get; set; }
        public ApplicationRole? Role { get; set; }
    }
}