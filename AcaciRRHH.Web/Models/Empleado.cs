using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcaciRRHH.Web.Models
{
    public class Empleado : IAuditable
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El ID de Persona es obligatorio.")]
        public int IdPersona { get; set; } // FK a Persona

        [Required(ErrorMessage = "El código de empleado es obligatorio.")]
        [StringLength(50, ErrorMessage = "El código de empleado no puede exceder los 50 caracteres.")]
        [Display(Name = "Código de Empleado")]
        public string CodigoEmpleado { get; set; } = string.Empty;

        // REMOVER [Required] DE AQUÍ para evitar conflictos con el model binder y la cultura.
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Contratación")]
        public DateTime FechaContratacion { get; set; }

        [Required(ErrorMessage = "El puesto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El puesto no puede exceder los 100 caracteres.")]
        public string Puesto { get; set; } = string.Empty;

        [Range(0, 9999999.99, ErrorMessage = "El salario debe ser un valor positivo.")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Salario { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El departamento no puede exceder los 100 caracteres.")]
        public string Departamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La agencia es obligatoria.")]
        public Agencia Agencia { get; set; }

        public EstadoEmpleado Estado { get; set; } = EstadoEmpleado.Activo; // Por defecto "Activo"

        [DataType(DataType.Date)] // Añadir para formato de fecha
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] // Añadir para formato de fecha
        [Display(Name = "Fecha de Fin de Contrato")] // Añadir DisplayName
        public DateTime? FechaFinContrato { get; set; } // Puede ser nula si el contrato está activo

        [StringLength(500)]
        [Display(Name = "Motivo de Finalización")] // Añadir DisplayName
        public string? MotivoFinalizacion { get; set; } // Para describir la razón

        [Display(Name = "Tipo de Finalización")] // Añadir DisplayName
        public TipoFinalizacion? TipoFinalizacion { get; set; } // Ahora es un Enum, puede ser nulo para empleados activos


        // Propiedades de Navegación
        [ForeignKey("IdPersona")]
        public Persona? Persona { get; set; }

        public ICollection<CapacitacionEmpleado> Capacitaciones { get; set; } = new List<CapacitacionEmpleado>();
        public ICollection<EvaluacionDesempeno> EvaluacionesDesempeno { get; set; } = new List<EvaluacionDesempeno>();
        public ICollection<HistorialDisciplinario> HistorialDisciplinario { get; set; } = new List<HistorialDisciplinario>();
        public ICollection<HistorialPuesto> HistorialPuestos { get; set; } = new List<HistorialPuesto>();
        public ICollection<TrayectoriaEmpleado> HistorialTrayectoria { get; set; } = new List<TrayectoriaEmpleado>();

        public ICollection<Incapacidad> Incapacidades { get; set; } = new List<Incapacidad>();
        public ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
        public ICollection<Vacacion> Vacaciones { get; set; } = new List<Vacacion>();

        // IAuditable properties
        [StringLength(100)]
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(100)]
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}