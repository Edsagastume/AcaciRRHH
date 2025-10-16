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
using Microsoft.AspNetCore.Authorization;

namespace AcaciRRHH.Web.Controllers
{
    public class CapacitacionesEmpleadoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CapacitacionesEmpleadoController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: CapacitacionesEmpleado
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CapacitacionesEmpleados.Include(c => c.Persona);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CapacitacionesEmpleado/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacionEmpleado = await _context.CapacitacionesEmpleados
                .Include(c => c.Persona)
                .FirstOrDefaultAsync(m => m.IdCapacitacionEmpleado == id);
            if (capacitacionEmpleado == null)
            {
                return NotFound();
            }

            return View(capacitacionEmpleado);
        }

        // GET: CapacitacionesEmpleado/Create
        [Authorize(Policy = "Administrador")]
        [HttpGet]
        public IActionResult Create(int idPersona)
        {
            var viewModel = new CapacitacionEmpleado
            {
                IdPersona = idPersona
            };
            return View(viewModel);
        }

        // POST: CapacitacionesEmpleado/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCapacitacionEmpleado,IdPersona,NombreCurso,Institucion,FechaInicio,FechaFin,CertificadoObtenido,ArchivoCertificado,Comentarios")] CapacitacionEmpleado capacitacionEmpleado)
        {
            var personaExists = await _context.Personas.AnyAsync(p => p.IdPersona == capacitacionEmpleado.IdPersona);
            if (!personaExists)
            {
                ModelState.AddModelError("", "La persona especificada no existe.");
                return View(capacitacionEmpleado);
            }

            if (ModelState.IsValid)
            {
                await HandleFileUpload(capacitacionEmpleado);
                NormalizeDates(capacitacionEmpleado);

                _context.Add(capacitacionEmpleado);
                await _context.SaveChangesAsync();

                return await RedirectToPersonaDetails(capacitacionEmpleado.IdPersona);
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
            return View(capacitacionEmpleado);
        }

        // GET: CapacitacionesEmpleado/Edit/5
        [Authorize(Policy = "Visualizador")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacionEmpleado = await _context.CapacitacionesEmpleados.FindAsync(id);
            if (capacitacionEmpleado == null)
            {
                return NotFound();
            }
            return View(capacitacionEmpleado);
        }

        // POST: CapacitacionesEmpleado/Edit/5
        [Authorize(Policy = "Administrador")]
        
        public async Task<IActionResult> Edit(int id, [Bind("IdCapacitacionEmpleado,IdPersona,NombreCurso,Institucion,FechaInicio,FechaFin,CertificadoObtenido,ArchivoCertificado,RutaCertificado,Comentarios")] CapacitacionEmpleado capacitacionEmpleado)
        {
            if (id != capacitacionEmpleado.IdCapacitacionEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var capacitacionToUpdate = await _context.CapacitacionesEmpleados.FindAsync(id);
                    if (capacitacionToUpdate == null)
                    {
                        return NotFound();
                    }

                    await HandleFileUpload(capacitacionEmpleado);

                    capacitacionToUpdate.NombreCurso = capacitacionEmpleado.NombreCurso;
                    capacitacionToUpdate.Institucion = capacitacionEmpleado.Institucion;
                    capacitacionToUpdate.FechaInicio = capacitacionEmpleado.FechaInicio;
                    capacitacionToUpdate.FechaFin = capacitacionEmpleado.FechaFin;
                    capacitacionToUpdate.CertificadoObtenido = capacitacionEmpleado.CertificadoObtenido;
                    capacitacionToUpdate.Comentarios = capacitacionEmpleado.Comentarios;
                    capacitacionToUpdate.IdPersona = capacitacionEmpleado.IdPersona;


                    //Solo actualiza la ruta si se ha subido un nuevo archivo
                    if (!string.IsNullOrEmpty(capacitacionEmpleado.RutaCertificado))
                    {
                        capacitacionToUpdate.RutaCertificado = capacitacionEmpleado.RutaCertificado;
                    }

                    NormalizeDates(capacitacionToUpdate);

                    _context.Update(capacitacionToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!CapacitacionEmpleadoExists(capacitacionEmpleado.IdCapacitacionEmpleado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        //Log de la excepción para depuración
                        Console.WriteLine($"DbUpdateConcurrencyException: {ex.Message}");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    //Log de cualquier otra excepción para depuración
                    Console.WriteLine($"General Exception: {ex.Message}");
                    throw;
                }
                return await RedirectToPersonaDetails(capacitacionEmpleado.IdPersona);
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
            return View(capacitacionEmpleado);
        }

        // GET: CapacitacionesEmpleado/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacionEmpleado = await _context.CapacitacionesEmpleados
                .Include(c => c.Persona)
                .FirstOrDefaultAsync(m => m.IdCapacitacionEmpleado == id);
            if (capacitacionEmpleado == null)
            {
                return NotFound();
            }

            return View(capacitacionEmpleado);
        }

        // POST: CapacitacionesEmpleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var capacitacionEmpleado = await _context.CapacitacionesEmpleados.FindAsync(id);
            if (capacitacionEmpleado != null)
            {
                var personaId = capacitacionEmpleado.IdPersona; // Save IdPersona before deleting

                if (!string.IsNullOrEmpty(capacitacionEmpleado.RutaCertificado))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, capacitacionEmpleado.RutaCertificado.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.CapacitacionesEmpleados.Remove(capacitacionEmpleado);
                await _context.SaveChangesAsync();
                return await RedirectToPersonaDetails(personaId);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CapacitacionEmpleadoExists(int id)
        {
            return _context.CapacitacionesEmpleados.Any(e => e.IdCapacitacionEmpleado == id);
        }

        private async Task HandleFileUpload(CapacitacionEmpleado model)
        {
            if (model.ArchivoCertificado != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/certificados");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ArchivoCertificado.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ArchivoCertificado.CopyToAsync(fileStream);
                }
                model.RutaCertificado = "/uploads/certificados/" + uniqueFileName;
            }
        }
        [Authorize(Policy = "Visualizador")]
        private void NormalizeDates(CapacitacionEmpleado model)
        {
            if (model.FechaInicio.HasValue)
            {
                model.FechaInicio = DateTime.SpecifyKind(model.FechaInicio.Value, DateTimeKind.Utc);
            }
            if (model.FechaFin.HasValue)
            {
                model.FechaFin = DateTime.SpecifyKind(model.FechaFin.Value, DateTimeKind.Utc);
            }
        }
        [Authorize(Policy = "Visualizador")]
        private async Task<IActionResult> RedirectToPersonaDetails(int idPersona)
        {
            var persona = await _context.Personas.FindAsync(idPersona);
            if (persona != null)
            {
                return RedirectToAction("Details", "Personas", new { id = persona.IdPersona });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

