using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AcaciRRHH.Web.Models;
using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AcaciRRHH.Web.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    private static readonly Dictionary<Agencia, string> AgenciaNames = new Dictionary<Agencia, string>
    {
        { Agencia.AgenciaCentroFinanciero, "Agencia Centro Financiero" },
        { Agencia.AgenciaElPajonal, "Agencia El Pajonal" },
        { Agencia.AgenciaLasRamblasSantaAna, "Agencia Las Ramblas Santa Ana" }
    };

    public async Task<IActionResult> Index()
    {
        var totalEmpleados = await _context.Empleados.CountAsync(e => e.Estado == EstadoEmpleado.Activo);

        var empleadosPorAgenciaAgrupados = await _context.Empleados
            .Where(e => e.Estado == EstadoEmpleado.Activo)
            .GroupBy(e => e.Agencia)
            .Select(g => new { Agencia = g.Key, Count = g.Count() })
            .ToListAsync();

        var empleadosPorAgencia = new Dictionary<string, int>();
        foreach (var item in empleadosPorAgenciaAgrupados)
        {
            if (AgenciaNames.TryGetValue(item.Agencia, out var name))
            {
                empleadosPorAgencia[name] = item.Count;
            }
        }

        var comiteIntegrantes = await _context.Directivos
            .GroupBy(d => d.TipoMiembro)
            .Select(g => new { TipoMiembro = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.TipoMiembro.ToString(), x => x.Count);

        var viewModel = new DashboardViewModel
        {
            TotalEmpleados = totalEmpleados,
            EmpleadosPorAgencia = empleadosPorAgencia,
            ComiteIntegrantes = comiteIntegrantes
        };

        return View(viewModel);
    }
}
