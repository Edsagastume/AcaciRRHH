using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class CapacitacionEmpleado : IAuditable, IValidatableObject // Consider renaming to just 'Capacitacion'
    {
        [Key]
        public int IdCapacitacionEmpleado { get; set; }

        public int IdPersona { get; set; } // FK to Persona

        [DisplayName("Nombre del Curso")]
        public string NombreCurso { get; set; } = string.Empty;


        [DisplayName("Institución")]
        public string? Institucion { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha de Inicio")]    
        public DateTime? FechaInicio { get; set; }


        [DisplayName("Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }


        [DisplayName("Certificado Obtenido")]
        public bool CertificadoObtenido { get; set; }   


        [DisplayName("Ruta del Certificado")]
        public string? RutaCertificado { get; set; } // Para atestados digitales


        [DisplayName("Archivo del Certificado")]
        [NotMapped]
        public IFormFile? ArchivoCertificado { get; set; }


        [DisplayName("Comentarios")]
        public string? Comentarios { get; set; }


        // Propiedad de Navegación
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
            if (FechaInicio.HasValue && FechaFin.HasValue && FechaInicio > FechaFin)
            {
                yield return new ValidationResult("La fecha de inicio no puede ser posterior a la fecha de fin.", new[] { nameof(FechaInicio), nameof(FechaFin) });
            }
            // No need for IdEmpleado/IdDirectivo validation here anymore
        }
    }
}