// AcaciRRHH.Web/ViewModels/EmpleadoReactivateViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace AcaciRRHH.Web.ViewModels
{
    public class EmpleadoReactivateViewModel
    {
        public int IdEmpleado { get; set; }

        [Display(Name = "Nombre Completo del Empleado")]
        public string? NombreCompletoEmpleado { get; set; }

        [Display(Name = "Fecha de Contratación")]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }


        [Display(Name = "Motivo de Reactivación")]
        [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres.")]
        public string? MotivoReactivacion { get; set; }
    }
}