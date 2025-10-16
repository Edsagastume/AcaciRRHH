using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Necesario para [Key], [Required], [EmailAddress], [Phone], [RegularExpression]

namespace AcaciRRHH.Web.Models
{
    public class Atestado : IAuditable
    {
        [Key]
        public int IdAtestado { get; set; }

        [Display(Name = "Persona Asociada")]
        [Required(ErrorMessage = "La persona asociada es obligatoria.")]
        public int IdPersona { get; set; } // FK a Persona

        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; } // Propiedad de navegación

        [Display(Name = "Tipo de Atestado")]
        [Required(ErrorMessage = "El tipo de atestado es obligatorio.")]
        [StringLength(100, ErrorMessage = "El tipo de atestado no puede exceder los 100 caracteres.")]
        public string? TipoAtestado { get; set; } // Ej: "DUI", "Pasaporte", "Título Universitario", "Certificado Curso"

        [Display(Name = "Ruta del Archivo")]
        [StringLength(500)]
        public string? RutaArchivo { get; set; } // Ruta relativa al wwwroot (ej: /uploads/atestados/documento.pdf)

        [Display(Name = "Nombre del Archivo")]
        [StringLength(255)]
        public string? NombreOriginalArchivo { get; set; } // Nombre que tenía el archivo al ser subido

        [Display(Name = "Extensión del Archivo")]
        [StringLength(10)]
        public string? ExtensionArchivo { get; set; } // Ej: ".pdf", ".jpg"

        [Display(Name = "Fecha de Subida")]
        [DataType(DataType.DateTime)]
        public DateTime FechaSubida { get; set; } = DateTime.Now;

        [Display(Name = "Subido Por")]
        [StringLength(100)]
        public string? SubidoPor { get; set; } // Nombre de usuario o rol que subió el documento

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true; // Habilitar/deshabilitar el atestado

        [Display(Name = "Comentarios")]
        [StringLength(500, ErrorMessage = "Los comentarios no pueden exceder los 500 caracteres.")]
        public string? Comentarios { get; set; }

        // IAuditable properties
        [StringLength(100)]
        [Display(Name = "Subido Por")]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}