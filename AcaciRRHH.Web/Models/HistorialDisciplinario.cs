using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class HistorialDisciplinario : IAuditable, IValidatableObject
    {
        [Key]
        public int IdIncidente { get; set; }
        public int IdEmpleado { get; set; } // FK
        public int IdPersona { get; set; } // FK

        [Display(Name = "Fecha del incidente")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha del incidente es obligatoria.")]
        public DateTime FechaIncidente { get; set; }

        [Display(Name = "Tipo de incidente")]
        [Required(ErrorMessage = "El tipo de incidente es obligatorio.")]
        public TipoIncidente TipoIncidente { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Acción tomada")]
        [Required(ErrorMessage = "La acción tomada es obligatoria.")]
        public string AccionTomada { get; set; } = string.Empty;
        

        public string? RutaIncidente { get; set; } // Para atestados digitales

        [NotMapped]
        [Display(Name = "Archivo digital")]
        public IFormFile? ArchivoIncidente { get; set; }

        [StringLength(100)]
        [Display(Name = "Registrado por")]
        public string? RegistradoPor { get; set; }

        [Display(Name = "Fecha de registro")]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        // Propiedad de Navegación
        public Empleado? Empleado { get; set; }
        public Persona? Persona { get; set; }

        // IAuditable properties
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaIncidente > DateTime.Today)
            {
                yield return new ValidationResult("La fecha del incidente no puede ser en el futuro.", new[] { nameof(FechaIncidente) });
            }

        }
    }
}