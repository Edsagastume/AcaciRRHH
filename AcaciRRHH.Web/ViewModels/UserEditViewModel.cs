
using AcaciRRHH.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcaciRRHH.Web.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        public string UserName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña (dejar en blanco para no cambiar)")]
        public string? NewPassword { get; set; }

        public List<RoleSelection> Roles { get; set; } = new List<RoleSelection>();
    }

    public class RoleSelection
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
    }
}
