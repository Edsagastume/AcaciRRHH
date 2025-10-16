namespace AcaciRRHH.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmpleados { get; set; }
        public Dictionary<string, int> EmpleadosPorAgencia { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ComiteIntegrantes { get; set; } = new Dictionary<string, int>();
        // Add other relevant dashboard data properties here
    }
}
