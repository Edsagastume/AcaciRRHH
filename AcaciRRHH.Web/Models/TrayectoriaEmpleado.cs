using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class TrayectoriaEmpleado : IAuditable
    {
        [Key]
        public int IdHistorialLaboral { get; set; }

        [Required]
        [Display(Name = "ID de Empleado")]
        [Column("EmpleadoIdEmpleado")] // Mantén esto si tu columna en la DB se llama "EmpleadoIdEmpleado"
        public int IdEmpleado { get; set; }

        [Required]
        [Display(Name = "Fecha de Cambio")]
        public DateTime FechaCambio { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tipo de Cambio")]
        public string TipoCambio { get; set; } = string.Empty; // Ej: "Contratación", "Promoción", "Inactivación", "Reactivación"

        [StringLength(100)]
        [Display(Name = "Puesto Anterior")]
        public string? PuestoAnterior { get; set; }

        [StringLength(100)]
        [Display(Name = "Puesto Nuevo")]
        public string? PuestoNuevo { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Salario Anterior")]
        public decimal? SalarioAnterior { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Salario Nuevo")]
        public decimal? SalarioNuevo { get; set; }

        [StringLength(100)]
        [Display(Name = "Departamento Anterior")]
        public string? DepartamentoAnterior { get; set; }

        [StringLength(100)]
        [Display(Name = "Departamento Nuevo")]
        public string? DepartamentoNuevo { get; set; }

        [StringLength(500)]
        [Display(Name = "Motivo del Cambio")]
        public string? MotivoCambio { get; set; } // Detalle del cambio (ej. "Renuncia", "Despido", "Reactivación por... ")

        [StringLength(100)]
        [Display(Name = "Registrado Por")]
        public string? RegistradoPor { get; set; }

        [Required]
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow; // Siempre usa UTC para el registro

        // Propiedad de Navegación
        [ForeignKey(nameof(IdEmpleado))]
        [Required] // Si cada TrayectoriaEmpleado debe estar ligada a un Empleado
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