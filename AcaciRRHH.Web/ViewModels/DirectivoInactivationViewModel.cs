using System.ComponentModel.DataAnnotations;

namespace AcaciRRHH.Web.ViewModels
{
    public class DirectivoInactivationViewModel
    {
        public int IdDirectivo { get; set; }
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El motivo de la inactivación es obligatorio.")]
        [Display(Name = "Motivo de Inactivación")]
        public string? MotivoInactivacion { get; set; }
    }
}
