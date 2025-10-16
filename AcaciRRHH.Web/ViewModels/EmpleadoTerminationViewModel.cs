// AcaciRRHH.Web/ViewModels/EmpleadoTerminationViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using AcaciRRHH.Web.Models; // If EstadoEmpleado is in Models

namespace AcaciRRHH.Web.ViewModels
{
    public class EmpleadoTerminationViewModel
    {
        [Required]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "La fecha de fin de relación laboral es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFinContrato { get; set; } // Can be nullable if you want to initially allow empty

        [StringLength(500, ErrorMessage = "El motivo de finalización no puede exceder los 500 caracteres.")]
        public string? MotivoFinalizacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de finalización.")]
        public TipoFinalizacion? TipoFinalizacion { get; set; } // Assuming TipoFinalizacion is an enum or class

        // Add any other properties from Empleado that you need to *display*
        // but not *bind* from the form, for example, the employee's name for confirmation:
        public string? NombreCompletoEmpleado { get; set; }
        public DateTime FechaContratacion { get; set; } // To display for validation checks
    }
}