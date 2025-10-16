using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class EvaluacionDesempeno : IAuditable
    {
        [Key]
        public int IdEvaluacion { get; set; }
        public int IdEmpleado { get; set; } // FK
        public int IdPersona { get; set; } // FK

        [Display(Name = "Fecha de Evaluación")]
        [DataType(DataType.Date)]
        public DateTime FechaEvaluacion { get; set; }


        [Display(Name = "Período")]
        public string Periodo { get; set; } = string.Empty;


        [Display(Name = "Puntuación")]
        public decimal Puntuacion { get; set; }

        [Display(Name = "Evaluador")]
        public string? Evaluador { get; set; } // Puedes hacer esto FK a Persona si registras evaluadores en sistema

        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        [Display(Name = "Ruta del Informe")]
        public string? RutaInforme { get; set; } // Para atestados digitales

        [NotMapped]
        public IFormFile? ArchivoInforme { get; set; }

        // Propiedad de Navegación
        public Empleado? Empleado { get; set; }
        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }


        // IAuditable properties
        [StringLength(100)]

        [Display(Name = "Registrado por")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Modificado por")]
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime LastModifiedAt { get; set; }
    }
}