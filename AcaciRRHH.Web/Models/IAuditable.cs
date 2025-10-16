using System;
using System.ComponentModel.DataAnnotations;

namespace AcaciRRHH.Web.Models
{
    public interface IAuditable
    {
        [Display(Name = "Creado por")]
        string? CreatedBy { get; set; }

        [Display(Name = "Fecha de creación")]
        DateTime CreatedAt { get; set; }

        [Display(Name = "Modificado por")]
        string? LastModifiedBy { get; set; }

        [Display(Name = "Fecha de modificación")]
        DateTime LastModifiedAt { get; set; }
    }
}
