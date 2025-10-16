using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class NivelEducativo : IAuditable
    {
        [Key]
        public int IdNivelEducativo { get; set; }


        [Required]
        public int IdPersona { get; set; }


        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }


        [Required(ErrorMessage = "El grado académico es obligatorio.")]
        [DisplayName("Grado Académico")]
        public GradoAcademicoEnum GradoAcademico { get; set; }


        [Required(ErrorMessage = "El nombre de la institución es obligatorio.")]
        [StringLength(150)]
        [DisplayName("Institución")]
        public string Institucion { get; set; } = string.Empty;


        [Required(ErrorMessage = "El título obtenido es obligatorio.")]
        [StringLength(150)]
        [DisplayName("Título Obtenido")]
        public string TituloObtenido { get; set; } = string.Empty;


        [DataType(DataType.Date)]
        [DisplayName("Fecha de Inicio")]
        public DateTime? FechaInicio { get; set; }


        [DataType(DataType.Date)]
        [DisplayName("Fecha de Fin")]
        public DateTime? FechaFin { get; set; }


        [StringLength(500)]
        [DisplayName("Comentarios")]
        public string? Comentarios { get; set; }


        // Propiedades de auditoría
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}