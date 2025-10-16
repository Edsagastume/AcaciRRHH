using System.Collections.Generic;
using System.ComponentModel;

namespace AcaciRRHH.Web.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [DisplayName("Nombre de Usuario")]
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }

        [DisplayName("Roles")]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}