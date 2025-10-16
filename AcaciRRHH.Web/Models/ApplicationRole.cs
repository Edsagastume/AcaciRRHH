using System.Collections.Generic;

namespace AcaciRRHH.Web.Models
{
    public class ApplicationRole
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}