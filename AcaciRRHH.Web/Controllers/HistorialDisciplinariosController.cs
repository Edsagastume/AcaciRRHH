using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace AcaciRRHH.Web.Controllers
{
    public class HistorialDisciplinariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HistorialDisciplinariosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: HistorialDisciplinarios
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HistorialDisciplinario.Include(h => h.Empleado);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HistorialDisciplinarios/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialDisciplinario = await _context.HistorialDisciplinario
                .Include(h => h.Empleado)
                .FirstOrDefaultAsync(m => m.IdIncidente == id);
            if (historialDisciplinario == null)
            {
                return NotFound();
            }

            return View(historialDisciplinario);
        }

        // GET: HistorialDisciplinarios/Create
        [Authorize(Policy = "Administrador")]
        public IActionResult Create(int idEmpleado)
        {
            var viewModel = new HistorialDisciplinario
            {
                IdEmpleado = idEmpleado,
                FechaIncidente = DateTime.Today, // Fecha actual por defecto
                RegistradoPor = User.Identity?.Name // Nombre de usuario de dominio
            };
            ViewData["TipoIncidente"] = new SelectList(Enum.GetValues(typeof(TipoIncidente)));
            return View(viewModel);
        }

        // POST: HistorialDisciplinarios/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,FechaIncidente,TipoIncidente,Descripcion,AccionTomada,ArchivoIncidente")] HistorialDisciplinario historialDisciplinario)
        {
            //automatizar campos
            historialDisciplinario.FechaRegistro = DateTime.UtcNow;
            historialDisciplinario.RegistradoPor = User.Identity?.Name;

            if (ModelState.IsValid)
            {
                var empleado = await _context.Empleados.FindAsync(historialDisciplinario.IdEmpleado);
                if (empleado == null)
                {
                    ModelState.AddModelError("IdEmpleado", "El empleado especificado no existe.");
                    ViewData["TipoIncidente"] = new SelectList(Enum.GetValues(typeof(TipoIncidente)), historialDisciplinario.TipoIncidente);
                    return View(historialDisciplinario);
                }

                historialDisciplinario.IdPersona = empleado.IdPersona;

                await HandleFileUpload(historialDisciplinario);
                NormalizeDates(historialDisciplinario);

                _context.Add(historialDisciplinario);
                await _context.SaveChangesAsync();

                return await RedirectToPersonaDetails(historialDisciplinario.IdEmpleado);
            }
            else
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["TipoIncidente"] = new SelectList(Enum.GetValues(typeof(TipoIncidente)), historialDisciplinario.TipoIncidente);
            return View(historialDisciplinario);
        }

        // GET: HistorialDisciplinarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialDisciplinario = await _context.HistorialDisciplinario.FindAsync(id);
            if (historialDisciplinario == null)
            {
                return NotFound();
            }
            ViewData["TipoIncidente"] = new SelectList(Enum.GetValues(typeof(TipoIncidente)), historialDisciplinario.TipoIncidente);
            return View(historialDisciplinario);
        }

        // POST: HistorialDisciplinarios/Edit/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdIncidente,IdEmpleado,FechaIncidente,TipoIncidente,Descripcion,AccionTomada,ArchivoIncidente,RutaIncidente")] HistorialDisciplinario historialDisciplinario)
        {
            if (id != historialDisciplinario.IdIncidente)
            {
                return NotFound();
            }

            // automatizar campos
            historialDisciplinario.FechaRegistro = DateTime.UtcNow;
            historialDisciplinario.RegistradoPor = User.Identity?.Name;

            if (ModelState.IsValid)
            {
                try
                {
                    var historialToUpdate = await _context.HistorialDisciplinario.FindAsync(id);
                    if (historialToUpdate == null)
                    {
                        return NotFound();
                    }

                    await HandleFileUpload(historialDisciplinario);

                    historialToUpdate.FechaIncidente = historialDisciplinario.FechaIncidente;
                    historialToUpdate.TipoIncidente = historialDisciplinario.TipoIncidente;
                    historialToUpdate.Descripcion = historialDisciplinario.Descripcion;
                    historialToUpdate.AccionTomada = historialDisciplinario.AccionTomada;
                    historialToUpdate.RegistradoPor = historialDisciplinario.RegistradoPor;
                    historialToUpdate.FechaRegistro = historialDisciplinario.FechaRegistro;

                    //Actualizar la ruta solo si se ha subido un nuevo archivo
                    if (!string.IsNullOrEmpty(historialDisciplinario.RutaIncidente))
                    {
                        historialToUpdate.RutaIncidente = historialDisciplinario.RutaIncidente;
                    }

                    NormalizeDates(historialToUpdate);

                    _context.Update(historialToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!HistorialDisciplinarioExists(historialDisciplinario.IdIncidente))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine($"DbUpdateConcurrencyException: {ex.Message}");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Exception: {ex.Message}");
                    throw;
                }
                return await RedirectToPersonaDetails(historialDisciplinario.IdEmpleado);
            }
            else
            {
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                    }
                }
            }
            ViewData["TipoIncidente"] = new SelectList(Enum.GetValues(typeof(TipoIncidente)), historialDisciplinario.TipoIncidente);
            return View(historialDisciplinario);
        }

        // GET: HistorialDisciplinarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialDisciplinario = await _context.HistorialDisciplinario
                .Include(h => h.Empleado)
                .FirstOrDefaultAsync(m => m.IdIncidente == id);
            if (historialDisciplinario == null)
            {
                return NotFound();
            }

            return View(historialDisciplinario);
        }

        // POST: HistorialDisciplinarios/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historialDisciplinario = await _context.HistorialDisciplinario.FindAsync(id);
            if (historialDisciplinario != null)
            {
                if (!string.IsNullOrEmpty(historialDisciplinario.RutaIncidente))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, historialDisciplinario.RutaIncidente.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.HistorialDisciplinario.Remove(historialDisciplinario);
                await _context.SaveChangesAsync();
                return await RedirectToPersonaDetails(historialDisciplinario.IdEmpleado);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HistorialDisciplinarioExists(int id)
        {
            return _context.HistorialDisciplinario.Any(e => e.IdIncidente == id);
        }

        private async Task HandleFileUpload(HistorialDisciplinario model)
        {
            if (model.ArchivoIncidente != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/incidentes");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ArchivoIncidente.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ArchivoIncidente.CopyToAsync(fileStream);
                }
                model.RutaIncidente = "/uploads/incidentes/" + uniqueFileName;
            }
        }

        private void NormalizeDates(HistorialDisciplinario model)
        {
            model.FechaIncidente = DateTime.SpecifyKind(model.FechaIncidente, DateTimeKind.Utc);
            model.FechaRegistro = DateTime.SpecifyKind(model.FechaRegistro, DateTimeKind.Utc);
        }

        private async Task<IActionResult> RedirectToPersonaDetails(int idEmpleado)
        {
            var empleado = await _context.Empleados.FindAsync(idEmpleado);
            if (empleado != null)
            {
                return RedirectToAction("Details", "Personas", new { id = empleado.IdPersona });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
