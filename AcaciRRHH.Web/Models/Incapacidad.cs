using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class Incapacidad : IAuditable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [ForeignKey("IdEmpleado")]
        public Empleado Empleado { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de finalización")]
        public DateTime FechaFin { get; set; }

        [Required]
        [Display(Name = "Motivo de la incapacidad")]
        public string Motivo { get; set; } = string.Empty;

        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }

        // IAuditable properties
        [StringLength(100)]
        
        [Display(Name = "Creado por")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Fecha de creación")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Actualizado por")]
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }

        [Display(Name = "Fecha de actualización")]
        public DateTime LastModifiedAt { get; set; }
    }
}
