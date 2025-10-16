using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class HistorialPuesto : IAuditable
    {
        [Key]
        public int IdHistorialPuesto { get; set; }
        [Required(ErrorMessage = "El ID de Empleado es obligatorio.")]


        public int IdEmpleado { get; set; } // FK a Empleado
        [Required(ErrorMessage = "El puesto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El puesto no puede exceder los 100 caracteres.")]


        [Display(Name = "Puesto")]
        public string Puesto { get; set; } = string.Empty;


        [Required(ErrorMessage = "La fecha de inicio del puesto es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Inicio del Puesto")]
        public DateTime? FechaInicioPuesto { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Fin del Puesto")]
        public DateTime? FechaFinPuesto { get; set; } // Nullable para el puesto actual


        // Propiedad de Navegación
        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }


        // IAuditable properties
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}