using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class Directivo : IAuditable
    {
        [Key]
        public int IdDirectivo { get; set; }

        [Display(Name = "Persona Asociada")]
        [Required(ErrorMessage = "La persona es obligatoria.")]
        public int IdPersona { get; set; }

        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; } // Propiedad de navegación a la Persona

        [Display(Name = "Cargo Directivo")]
        [Required(ErrorMessage = "El cargo directivo es obligatorio.")]
        public CargoDirectivoEnum CargoDirectivo { get; set; } // Ahora es un enum


        [Display(Name = "Fecha Inicio Mandato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha de inicio de mandato es obligatoria.")]
        public DateTime FechaInicioMandato { get; set; }

        [Display(Name = "Fecha Fin Mandato")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        // La fecha fin puede ser nula si el mandato está activo
        public DateTime? FechaFinMandato { get; set; }

        [Display(Name = "Tipo de Miembro / Órgano")] // Nombre más descriptivo
        [Required(ErrorMessage = "El tipo de miembro es obligatorio.")]
        public TipoMiembroOrganoEnum TipoMiembro { get; set; } // Ahora es un enum

        [Display(Name = "Estado")]
        public EstadoDirectivo Estado { get; set; }

        [Display(Name = "Motivo de Inactivación")]
        public string? MotivoInactivacion { get; set; }

        // Propiedad de navegación para el historial de este directivo
        public ICollection<HistorialDirectivo>? HistorialDirectivo { get; set; }

        // IAuditable properties
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
        if (FechaFinMandato.HasValue && FechaFinMandato.Value < FechaInicioMandato)
        {
            yield return new ValidationResult(
                "La fecha de fin de mandato no puede ser anterior a la fecha de inicio.",
                new[] { nameof(FechaFinMandato) }

                );
            }
        
        }

    }
}