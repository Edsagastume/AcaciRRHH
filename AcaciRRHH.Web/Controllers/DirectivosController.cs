using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using AcaciRRHH.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AcaciRRHH.Web.Controllers
{
    public class DirectivosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Directivos
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Administrador"))
            {
                var directivos = await _context.Directivos.Where(d => d.Estado == EstadoDirectivo.Activo).ToListAsync();
                var today = DateTime.UtcNow;
                int updatedCount = 0;

                foreach (var directivo in directivos)
                {
                    if (directivo.FechaFinMandato.HasValue && directivo.FechaFinMandato.Value < today)
                    {
                        directivo.Estado = EstadoDirectivo.Vencido;
                        directivo.MotivoInactivacion = "Vencimiento de mandato";
                        updatedCount++;
                    }
                }

                if (updatedCount > 0)
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"{updatedCount} directivos han sido actualizados a 'Vencido'.";
                }
                else
                {
                    TempData["InfoMessage"] = "No hay directivos para actualizar.";
                }
            }

            // Incluimos la Persona asociada para mostrar el nombre completo del directivo
            var applicationDbContext = _context.Directivos.Include(d => d.Persona);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Directivos/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directivo = await _context.Directivos
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.IdDirectivo == id);

            if (directivo == null)
            {
                return NotFound();
            }

            var historialCompleto = await _context.HistorialDirectivo
                .Include(h => h.Directivo)
                .Where(h => h.Directivo != null && h.Directivo.IdPersona == directivo.IdPersona)
                .OrderByDescending(h => h.FechaRegistro)
                .ToListAsync();

            ViewData["HistorialCompleto"] = historialCompleto;

            return View(directivo);
        }

        // GET: Directivos/Create
        [Authorize(Policy = "Administrador")]
        public IActionResult Create()
        {
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "NombreCompleto");
            return View();
        }

                // POST: Directivos/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDirectivo,IdPersona,CargoDirectivo,FechaInicioMandato,FechaFinMandato,TipoMiembro")] Directivo directivo)
        {
            if (ModelState.IsValid)
            {
                // Asegurar que las fechas se guarden en UTC
                directivo.FechaInicioMandato = DateTime.SpecifyKind(directivo.FechaInicioMandato, DateTimeKind.Utc);
                if (directivo.FechaFinMandato.HasValue)
                {
                    directivo.FechaFinMandato = DateTime.SpecifyKind(directivo.FechaFinMandato.Value, DateTimeKind.Utc);
                }

                directivo.Estado = EstadoDirectivo.Activo;

                _context.Add(directivo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Directivo creado exitosamente.";

                // Opcional: Crear el primer registro de historial para este nuevo directivo
                var primerHistorial = new HistorialDirectivo
                {
                    IdDirectivo = directivo.IdDirectivo,
                    Cargo = directivo.CargoDirectivo.ToString(),
                    FechaInicioMandato = directivo.FechaInicioMandato,
                    FechaFinMandato = directivo.FechaFinMandato,
                    TipoMandato = "Nuevo", // O el valor que consideres apropiado
                    Comentarios = "Registro inicial al crear el directivo.",
                    RegistradoPor = User.Identity?.Name ?? "Sistema",
                    FechaRegistro = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)
                };
                _context.Add(primerHistorial);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Details), new { id = directivo.IdDirectivo });
            }
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "NombreCompleto", directivo.IdPersona);
            return View(directivo);
        }

        // GET: Directivos/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directivo = await _context.Directivos.FindAsync(id);
            if (directivo == null)
            {
                return NotFound();
            }
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "NombreCompleto", directivo.IdPersona);
            return View(directivo);
        }

        // POST: Directivos/Edit/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDirectivo,IdPersona,CargoDirectivo,FechaInicioMandato,FechaFinMandato,TipoMiembro,Estado,MotivoInactivacion")] Directivo directivo)
        {
            if (id != directivo.IdDirectivo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Asegurar que las fechas se guarden en UTC
                    directivo.FechaInicioMandato = DateTime.SpecifyKind(directivo.FechaInicioMandato, DateTimeKind.Utc);
                    if (directivo.FechaFinMandato.HasValue)
                    {
                        directivo.FechaFinMandato = DateTime.SpecifyKind(directivo.FechaFinMandato.Value, DateTimeKind.Utc);
                    }

                    _context.Update(directivo);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Directivo actualizado exitosamente.";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectivoExists(directivo.IdDirectivo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = directivo.IdDirectivo });
            }
            ViewData["IdPersona"] = new SelectList(_context.Personas, "IdPersona", "NombreCompleto", directivo.IdPersona);
            return View(directivo);
        }

        // GET: Directivos/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directivo = await _context.Directivos
                .Include(d => d.Persona)
                .FirstOrDefaultAsync(m => m.IdDirectivo == id);
            if (directivo == null)
            {
                return NotFound();
            }

            return View(directivo);
        }

        // POST: Directivos/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var directivo = await _context.Directivos.FindAsync(id);
            if (directivo != null)
            {
                _context.Directivos.Remove(directivo);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Directivo eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        

        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Inactivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var directivo = await _context.Directivos.Include(d => d.Persona).FirstOrDefaultAsync(d => d.IdDirectivo == id);

            if (directivo == null)
            {
                return NotFound();
            }

            var model = new DirectivoInactivationViewModel
            {
                IdDirectivo = directivo.IdDirectivo,
                NombreCompleto = directivo.Persona?.NombreCompleto
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inactivate(DirectivoInactivationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var directivo = await _context.Directivos.FindAsync(model.IdDirectivo);
                if (directivo == null)
                {
                    return NotFound();
                }

                directivo.Estado = EstadoDirectivo.Inactivo;
                directivo.MotivoInactivacion = model.MotivoInactivacion;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Directivo inactivado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private bool DirectivoExists(int id)
        {
            return _context.Directivos.Any(e => e.IdDirectivo == id);
        }
    }
}
