
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AcaciRRHH.Web.ViewModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Nombre de Usuario")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "El nombre de usuario no puede contener espacios ni caracteres especiales.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y como máximo {1} caracteres de longitud.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "La contraseña debe tener al menos 6 caracteres, incluyendo una mayúscula, una minúscula, un número y un caracter especial.")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de la contraseña no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public List<RoleSelection> Roles { get; set; } = new List<RoleSelection>();
    }
}
