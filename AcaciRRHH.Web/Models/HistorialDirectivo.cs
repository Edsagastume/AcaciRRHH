using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class HistorialDirectivo : IAuditable
    {
        [Key]
        public int IdHistorialDirectivo { get; set; }

        [Display(Name = "Directivo")]
        [Required(ErrorMessage = "El directivo es obligatorio.")]
        public int IdDirectivo { get; set; }

        [ForeignKey("IdDirectivo")]
        public Directivo? Directivo { get; set; } // Propiedad de navegación al Directivo

        [Display(Name = "Cargo")]
        [Required(ErrorMessage = "El cargo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El cargo no puede exceder los 100 caracteres.")]
        public string Cargo { get; set; } = string.Empty;

        [Display(Name = "Fecha Inicio Mandato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha de inicio de mandato es obligatoria.")]
        public DateTime FechaInicioMandato { get; set; }

        [Display(Name = "Fecha Fin Mandato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaFinMandato { get; set; } // Puede ser nula si el mandato está activo

        [Display(Name = "Tipo de Mandato")]
        [Required(ErrorMessage = "El tipo de mandato es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de mandato no puede exceder los 50 caracteres.")]
        // Ej: "Nuevo", "Renovación", "Cambio Cargo", "Finalización"
        public string TipoMandato { get; set; } = string.Empty;

        [Display(Name = "Comentarios")]
        [StringLength(500, ErrorMessage = "Los comentarios no pueden exceder los 500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string? Comentarios { get; set; }

        [Display(Name = "Registrado Por")]
        [StringLength(255)]
        public string? RegistradoPor { get; set; }

        [Display(Name = "Fecha Registro")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime FechaRegistro { get; set; }

        // IAuditable properties
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}