using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AcaciRRHH.Web.Models
{
    public class Vacacion : IAuditable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [ForeignKey("IdEmpleado")]
        public Empleado Empleado { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Solicitud")]
        public DateTime FechaSolicitud { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime? FechaInicio { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Fin")]
        public DateTime? FechaFin { get; set; }

        [Display(Name = "Días Solicitados")]
        public int DiasSolicitados { get; set; }

        [Display(Name = "Días Aprobados")]
        public bool Aprobado { get; set; }

        [StringLength(500)] 
        [Display(Name = "Comentarios")]
        public string? Comentarios { get; set; }


        [Display(Name = "Fechas No Consecutivas (separadas por coma)")]
        public string? FechasNoConsecutivas { get; set; }

        [Display(Name = "Son Consecutivas")]
        public bool SonConsecutivas { get; set; }

        [NotMapped]
        public bool IsVencida
        {
            get
            {
                if (SonConsecutivas)
                {
                    return FechaFin.HasValue && FechaFin.Value < DateTime.Today;
                }
                else if (!string.IsNullOrEmpty(FechasNoConsecutivas))
                {
                    var lastDate = FechasNoConsecutivas.Split(',').Select(f => DateTime.Parse(f.Trim())).Max();
                    return lastDate < DateTime.Today;
                }
                return false;
            }
        }

        // IAuditable properties
        [StringLength(100)]
        [Display(Name = "Creado por")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Actualizado por")]
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }

        [Display(Name = "Fecha de Actualización")]
        public DateTime LastModifiedAt { get; set; }
    }
}