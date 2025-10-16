// AcaciRRHH.Web/Models/Enums.cs

namespace AcaciRRHH.Web.Models
{
    public enum EstadoEmpleado
    {
        Activo,
        Inactivo,
        Suspendido
    }

    public enum TipoFinalizacion
    {
        Renuncia,
        Despido,
        FinDeContrato, // Usamos camelCase para C#
        Otros
    }

    public enum CargoDirectivoEnum
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Presidente")]
        Presidente,
        [System.ComponentModel.DataAnnotations.Display(Name = "Vicepresidente")]
        Vicepresidente,
        [System.ComponentModel.DataAnnotations.Display(Name = "Tesorero")]
        Tesorero,
        [System.ComponentModel.DataAnnotations.Display(Name = "Vocal")]
        Vocal
        // Agrega otros cargos si los hay
    }

    public enum TipoMiembroOrganoEnum
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Consejo de Administración")]
        ConsejoAdministracion,
        [System.ComponentModel.DataAnnotations.Display(Name = "Junta de Vigilancia")]
        JuntaVigilancia,
        [System.ComponentModel.DataAnnotations.Display(Name = "Comité de Crédito")]
        ComiteCredito,
        [System.ComponentModel.DataAnnotations.Display(Name = "Comité de Educación")]
        ComiteEducacion,
        [System.ComponentModel.DataAnnotations.Display(Name = "Comité de Prevención de LDA/FT/FPADM")]
        ComitePrevencionLDAFTFPADM
    }

    public enum TipoIncidente
    {
        Grave,
        Leve
    }

    public enum GradoAcademicoEnum
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Educación Básica")]
        EducacionBasica,
        [System.ComponentModel.DataAnnotations.Display(Name = "Educación Media")]
        EducacionMedia,
        [System.ComponentModel.DataAnnotations.Display(Name = "Educación Superior")]
        EducacionSuperior,
        [System.ComponentModel.DataAnnotations.Display(Name = "Educación Técnica")]
        EducacionTecnica
    }

    public enum Agencia
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Agencia Centro Financiero")]
        AgenciaCentroFinanciero = 2101,
        [System.ComponentModel.DataAnnotations.Display(Name = "Agencia El Pajonal")]
        AgenciaElPajonal = 2102,
        [System.ComponentModel.DataAnnotations.Display(Name = "Agencia Las Ramblas Santa Ana")]
        AgenciaLasRamblasSantaAna = 2103
    }

    public enum Genero
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Masculino")]
        Masculino,
        [System.ComponentModel.DataAnnotations.Display(Name = "Femenino")]
        Femenino
    }

    public enum TipoAFP
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "AFP Crecer")]
        Crecer,
        [System.ComponentModel.DataAnnotations.Display(Name = "AFP Confia")]
        Confia,
        [System.ComponentModel.DataAnnotations.Display(Name = "IPSFA")]
        IPSFA
    }

    public enum EstadoDirectivo
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "Activo")]
        Activo,
        [System.ComponentModel.DataAnnotations.Display(Name = "Inactivo")]
        Inactivo,
        [System.ComponentModel.DataAnnotations.Display(Name = "Vencido")]
        Vencido
    }
}