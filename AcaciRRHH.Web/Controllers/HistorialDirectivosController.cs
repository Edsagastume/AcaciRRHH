using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AcaciRRHH.Web.Controllers
{
    public class HistorialDirectivosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistorialDirectivosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HistorialDirectivos/Index/{idDirectivo}
        // Muestra el historial de un directivo específico
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index(int? idDirectivo)
        {
            if (idDirectivo == null)
            {
                TempData["ErrorMessage"] = "Debe especificar un directivo para ver su historial.";
                return RedirectToAction("Index", "Directivos");
            }

            var directivo = await _context.Directivos
                                        .Include(d => d.Persona)
                                        .Include(d => d.HistorialDirectivo!.OrderByDescending(hd => hd.IdHistorialDirectivo)) // Ordenar por ID para consistencia
                                        .FirstOrDefaultAsync(d => d.IdDirectivo == idDirectivo);

            if (directivo == null)
            {
                TempData["ErrorMessage"] = "Directivo no encontrado.";
                return RedirectToAction("Index", "Directivos");
            }

            ViewData["DirectivoNombreCompleto"] = $"{directivo.Persona?.Nombre} {directivo.Persona?.Apellidos}";
            ViewData["IdDirectivoActual"] = idDirectivo.Value;

            return View(directivo.HistorialDirectivo);
        }

        // GET: HistorialDirectivos/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialDirectivo = await _context.HistorialDirectivo
                .Include(hd => hd.Directivo)
                    .ThenInclude(d => d!.Persona)
                .FirstOrDefaultAsync(m => m.IdHistorialDirectivo == id);

            if (historialDirectivo == null)
            {
                return NotFound();
            }

            return View(historialDirectivo);
        }

        // GET: HistorialDirectivos/Create?idDirectivo={id}
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Create(int? idDirectivo)
        {
            if (idDirectivo == null)
            {
                TempData["ErrorMessage"] = "Debe especificar un directivo para registrar su historial.";
                return RedirectToAction("Index", "Directivos");
            }

            var directivo = await _context.Directivos
                                        .Include(d => d.Persona)
                                        .FirstOrDefaultAsync(d => d.IdDirectivo == idDirectivo);

            if (directivo == null)
            {
                TempData["ErrorMessage"] = "Directivo no encontrado.";
                return RedirectToAction("Index", "Directivos");
            }
            // Prellenar algunos campos del historial basados en el directivo actual
            var historial = new HistorialDirectivo
            {
                IdDirectivo = idDirectivo.Value,
                FechaInicioMandato = directivo.FechaInicioMandato, // Asumimos que el inicio del nuevo mandato puede ser la fecha actual del directivo
                FechaFinMandato = directivo.FechaFinMandato,     // Y el fin del mandato
                Cargo = directivo.CargoDirectivo.ToString(),     // El cargo actual del directivo será el "anterior" para un nuevo registro
                TipoMandato = "Cambio Cargo/Fecha",              // Tipo por defecto para un registro manual
                FechaRegistro = DateTime.Today,                  // Fecha por defecto para el formulario
            };

            ViewData["DirectivoNombreCompleto"] = $"{directivo.Persona?.Nombre} {directivo.Persona?.Apellidos}";
            ViewData["IdDirectivo"] = new SelectList(_context.Directivos, "IdDirectivo", "CargoDirectivo", idDirectivo); // Para dropdown si la vista lo necesita
            return View(historial);
        }

        // POST: HistorialDirectivos/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHistorialDirectivo,IdDirectivo,Cargo,FechaInicioMandato,FechaFinMandato,TipoMandato,Comentarios,RegistradoPor,FechaRegistro")] HistorialDirectivo historialDirectivo)
        {
            // Remover propiedades que serán asignadas por el controlador o son solo informativas
            ModelState.Remove(nameof(historialDirectivo.RegistradoPor));
            ModelState.Remove(nameof(historialDirectivo.FechaRegistro));
            ModelState.Remove(nameof(historialDirectivo.Directivo)); // Ignorar la propiedad de navegación

            if (ModelState.IsValid)
            {
                var directivo = await _context.Directivos.FindAsync(historialDirectivo.IdDirectivo);
                if (directivo == null)
                {
                    ModelState.AddModelError("", "Directivo asociado no encontrado. No se pudo registrar el historial.");
                    ViewData["IdDirectivo"] = new SelectList(_context.Directivos, "IdDirectivo", "CargoDirectivo", historialDirectivo.IdDirectivo);
                    return View(historialDirectivo);
                }

                // Asignar valores automáticos y asegurar UTC
                historialDirectivo.FechaInicioMandato = DateTime.SpecifyKind(historialDirectivo.FechaInicioMandato, DateTimeKind.Utc);
                if (historialDirectivo.FechaFinMandato.HasValue)
                {
                    historialDirectivo.FechaFinMandato = DateTime.SpecifyKind(historialDirectivo.FechaFinMandato.Value, DateTimeKind.Utc);
                }
                historialDirectivo.FechaRegistro = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
                historialDirectivo.RegistradoPor = User.Identity?.Name ?? "Sistema";

                if (Enum.TryParse(historialDirectivo.Cargo, out CargoDirectivoEnum newCargoEnum))
                {
                    directivo.CargoDirectivo = newCargoEnum;
                }
                else
                {
                    // Manejar el caso si el string no es un valor válido del enum
                    ModelState.AddModelError("Cargo", "El cargo especificado en el historial no es un cargo directivo válido.");
                    ViewData["DirectivoNombreCompleto"] = $"{directivo.Persona?.Nombre} {directivo.Persona?.Apellidos}";
                    ViewData["IdDirectivo"] = new SelectList(_context.Directivos, "IdDirectivo", "CargoDirectivo", historialDirectivo.IdDirectivo);
                    return View(historialDirectivo);
                }

                directivo.FechaInicioMandato = historialDirectivo.FechaInicioMandato;
                directivo.FechaFinMandato = historialDirectivo.FechaFinMandato;
                // No se actualiza TipoMiembro aquí a menos que el historial tenga un campo para ello.
                
                _context.Add(historialDirectivo);
                _context.Update(directivo); // Actualizar el directivo principal
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Historial de directivo registrado exitosamente.";
                return RedirectToAction("Details", "Directivos", new { id = historialDirectivo.IdDirectivo });
            }

            // Si hay errores de validación, recargar ViewData
            var currentDirectivo = await _context.Directivos
                                                .Include(d => d.Persona)
                                                .FirstOrDefaultAsync(d => d.IdDirectivo == historialDirectivo.IdDirectivo);
            if (currentDirectivo != null)
            {
                ViewData["DirectivoNombreCompleto"] = $"{currentDirectivo.Persona?.Nombre} {currentDirectivo.Persona?.Apellidos}";
            }
            ViewData["IdDirectivo"] = new SelectList(_context.Directivos, "IdDirectivo", "CargoDirectivo", historialDirectivo.IdDirectivo);
            return View(historialDirectivo);
        }

        // GET: HistorialDirectivos/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialDirectivo = await _context.HistorialDirectivo.FindAsync(id);
            if (historialDirectivo == null)
            {
                return NotFound();
            }
            var directivo = await _context.Directivos.Include(d => d.Persona).FirstOrDefaultAsync(d => d.IdDirectivo == historialDirectivo.IdDirectivo);
            if (directivo == null) { return NotFound(); }
            ViewData["DirectivoNombreCompleto"] = $"{directivo.Persona?.Nombre} {directivo.Persona?.Apellidos}";

            return View(historialDirectivo);
        }

        // POST: HistorialDirectivos/Edit/5
        [Authorize(Policy = "Administrador")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHistorialDirectivo,IdDirectivo,Cargo,FechaInicioMandato,FechaFinMandato,TipoMandato,Comentarios,RegistradoPor,FechaRegistro")] HistorialDirectivo historialDirectivo)
        {
            if (id != historialDirectivo.IdHistorialDirectivo)
            {
                return NotFound();
            }

            // No validar propiedades que asignamos nosotros o que no deben ser editables
            ModelState.Remove(nameof(historialDirectivo.RegistradoPor));
            ModelState.Remove(nameof(historialDirectivo.FechaRegistro));
            ModelState.Remove(nameof(historialDirectivo.Directivo)); // Ignorar la propiedad de navegación

            if (ModelState.IsValid)
            {
                try
                {
                    // Asegura que las fechas se guarden en UTC
                    historialDirectivo.FechaInicioMandato = DateTime.SpecifyKind(historialDirectivo.FechaInicioMandato, DateTimeKind.Utc);
                    if (historialDirectivo.FechaFinMandato.HasValue)
                    {
                        historialDirectivo.FechaFinMandato = DateTime.SpecifyKind(historialDirectivo.FechaFinMandato.Value, DateTimeKind.Utc);
                    }
                    
                    // Recupera valores originales de no editables
                    var originalHistorial = await _context.HistorialDirectivo.AsNoTracking().FirstOrDefaultAsync(h => h.IdHistorialDirectivo == id);
                    if (originalHistorial != null)
                    {
                        historialDirectivo.RegistradoPor = originalHistorial.RegistradoPor;
                        historialDirectivo.FechaRegistro = originalHistorial.FechaRegistro;
                    } else { return NotFound(); }


                    _context.Update(historialDirectivo);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Historial de directivo actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistorialDirectivoExists(historialDirectivo.IdHistorialDirectivo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Directivos", new { id = historialDirectivo.IdDirectivo });
            }

            var currentDirectivo = await _context.Directivos
                                                .Include(d => d.Persona)
                                                .FirstOrDefaultAsync(d => d.IdDirectivo == historialDirectivo.IdDirectivo);
            if (currentDirectivo != null)
            {
                ViewData["DirectivoNombreCompleto"] = $"{currentDirectivo.Persona?.Nombre} {currentDirectivo.Persona?.Apellidos}";
            }
            ViewData["IdDirectivo"] = new SelectList(_context.Directivos, "IdDirectivo", "CargoDirectivo", historialDirectivo.IdDirectivo);
            return View(historialDirectivo);
        }

        // GET: HistorialDirectivos/Delete/5
        [Authorize(Policy = "Administrador")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialDirectivo = await _context.HistorialDirectivo
                .Include(hd => hd.Directivo)
                    .ThenInclude(d => d!.Persona)
                .FirstOrDefaultAsync(m => m.IdHistorialDirectivo == id);
            if (historialDirectivo == null)
            {
                return NotFound();
            }

            return View(historialDirectivo);
        }

        // POST: HistorialDirectivos/Delete/5
        [Authorize(Policy = "Administrador")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historialDirectivo = await _context.HistorialDirectivo.FindAsync(id);
            if (historialDirectivo == null)
            {
                return NotFound();
            }
            int directivoId = historialDirectivo.IdDirectivo; // Guardamos el IdDirectivo antes de eliminar

            _context.HistorialDirectivo.Remove(historialDirectivo);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Registro de historial eliminado exitosamente.";
            return RedirectToAction("Details", "Directivos", new { id = directivoId });
        }

        private bool HistorialDirectivoExists(int id)
        {
            return _context.HistorialDirectivo.Any(e => e.IdHistorialDirectivo == id);
        }
    }
}