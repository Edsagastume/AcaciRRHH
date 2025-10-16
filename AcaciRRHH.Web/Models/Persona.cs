using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
namespace AcaciRRHH.Web.Models
{
    public class Persona : IAuditable
    {
        [Key]
        public int IdPersona { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]


        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden exceder los 100 caracteres.")]


        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = string.Empty;


        // Propiedad computada para el nombre completo
        [NotMapped] // Indicar a EF Core que esta propiedad no se mapea a una columna de la base de datos
        public string NombreCompleto => $"{Nombre} {Apellidos}";


        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)] // Asegura que solo se muestre el control de fecha
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] // Formato para la vista 
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }


        [Required(ErrorMessage = "El género es obligatorio.")]
        [Display(Name = "Género")]
        public Genero Genero { get; set; }


        [Required(ErrorMessage = "El DUI es obligatorio.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El DUI debe tener 10 caracteres (########-#).")]
        [RegularExpression(@"^\d{8}-\d{1}$", ErrorMessage = "Formato de DUI inválido. Use ########-#")]
        [Display(Name = "DUI")]
        public string DUI { get; set; } = string.Empty;



        [Display(Name = "Fecha de Vencimiento de DUI")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaVencimientoDUI { get; set; }


        [StringLength(17, MinimumLength = 17, ErrorMessage = "El NIT debe tener 17 caracteres (####-######-###-#).")]
        [RegularExpression(@"^\d{4}-\d{6}-\d{3}-\d{1}$", ErrorMessage = "Formato de NIT inválido. Use ####-######-###-#")]
        [Display(Name = "NIT")]
        public string NIT { get; set; } = string.Empty;


        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
        [StringLength(150, ErrorMessage = "El correo electrónico no puede exceder los 150 caracteres.")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = string.Empty;



        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Formato de teléfono inválido.")] // Este atributo valida un formato general de teléfono
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El teléfono debe tener 9 caracteres (####-####).")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "Formato de teléfono inválido. Use ####-####")]
        [Display(Name = "Número de Teléfono")]
        public string Telefono { get; set; } = string.Empty;


        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(250, ErrorMessage = "La dirección no puede exceder los 250 caracteres.")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; } = string.Empty;


        [StringLength(20, ErrorMessage = "El número de AFP no puede exceder los 20 caracteres.")]
        [Display(Name = "Número de AFP")]
        public string? NumeroAFP { get; set; }


        [Display(Name = "Tipo de AFP")]
        public TipoAFP? TipoAFP { get; set; }

        // Propiedades de Navegación para relaciones

        [InverseProperty("Persona")]
        public ICollection<NivelEducativo>? NivelesEducativos { get; set; } // Una persona puede tener varios niveles educativos

        [InverseProperty("Persona")]
        public ICollection<Atestado>? Atestados { get; set; } // Una persona puede tener varios atestados

        [InverseProperty("Persona")]
        public ICollection<CapacitacionEmpleado>? Capacitaciones { get; set; } // Una persona puede tener varias capacitaciones

        public Empleado? Empleado { get; set; }
        public ICollection<Directivo>? Directivos { get; set; }

        // IAuditable properties
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }

        // Validación personalizada para la fecha de nacimiento
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - FechaNacimiento.Year;

            // Ajuste si aún no ha cumplido años este año
            if (FechaNacimiento > hoy.AddYears(-edad))
            {
                edad--;
            }

            if (edad < 18)
            {
                yield return new ValidationResult(
                    "Debe tener al menos 18 años.",
                    new[] { nameof(FechaNacimiento) }
                );
            }

            
            // Validación de DUI no vencido
            if (FechaVencimientoDUI.HasValue && FechaVencimientoDUI.Value < hoy)
            {
                yield return new ValidationResult(
                "El DUI está vencido.",
                new[] { nameof(FechaVencimientoDUI) }
            );
            }


        }
        


    }
}