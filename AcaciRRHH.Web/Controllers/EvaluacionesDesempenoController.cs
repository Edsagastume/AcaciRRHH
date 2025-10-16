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
    public class EvaluacionesDesempenoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EvaluacionesDesempenoController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: EvaluacionesDesempeno
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EvaluacionesDesempeno
                .Include(e => e.Empleado)
                .Where(e => e.Empleado != null && e.Empleado.Estado == EstadoEmpleado.Activo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EvaluacionesDesempeno/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacionDesempeno = await _context.EvaluacionesDesempeno
                .Include(e => e.Empleado)
                .FirstOrDefaultAsync(m => m.IdEvaluacion == id);
            if (evaluacionDesempeno == null)
            {
                return NotFound();
            }

            return View(evaluacionDesempeno);
        }

        // GET: EvaluacionesDesempeno/Create
        [Authorize(Policy = "Administrador")]
        public IActionResult Create(int idEmpleado)
        {
            var viewModel = new EvaluacionDesempeno
            {
                IdEmpleado = idEmpleado,
                FechaEvaluacion = DateTime.Today
            };
            return View(viewModel);
        }

        // POST: EvaluacionesDesempeno/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,FechaEvaluacion,Periodo,Puntuacion,Evaluador,Comentarios,ArchivoInforme")] EvaluacionDesempeno evaluacionDesempeno)
        {
            if (ModelState.IsValid)
            {
                var empleado = await _context.Empleados.FindAsync(evaluacionDesempeno.IdEmpleado);
                if (empleado == null)
                {
                    ModelState.AddModelError("IdEmpleado", "El empleado especificado no existe.");
                    return View(evaluacionDesempeno);
                }
                evaluacionDesempeno.IdPersona = empleado.IdPersona;

                await HandleFileUpload(evaluacionDesempeno);
                NormalizeDates(evaluacionDesempeno);

                _context.Add(evaluacionDesempeno);
                await _context.SaveChangesAsync();

                return await RedirectToPersonaDetails(evaluacionDesempeno.IdEmpleado);
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
            return View(evaluacionDesempeno);
        }

        // GET: EvaluacionesDesempeno/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacionDesempeno = await _context.EvaluacionesDesempeno.FindAsync(id);
            if (evaluacionDesempeno == null)
            {
                return NotFound();
            }
            return View(evaluacionDesempeno);
        }

        // POST: EvaluacionesDesempeno/Edit/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEvaluacion,IdEmpleado,FechaEvaluacion,Periodo,Puntuacion,Evaluador,Comentarios,ArchivoInforme,RutaInforme")] EvaluacionDesempeno evaluacionDesempeno)
        {
            if (id != evaluacionDesempeno.IdEvaluacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var evaluacionToUpdate = await _context.EvaluacionesDesempeno.FindAsync(id);
                    if (evaluacionToUpdate == null)
                    {
                        return NotFound();
                    }

                    await HandleFileUpload(evaluacionDesempeno);
                    evaluacionToUpdate.FechaEvaluacion = evaluacionDesempeno.FechaEvaluacion;
                    evaluacionToUpdate.Periodo = evaluacionDesempeno.Periodo;
                    evaluacionToUpdate.Puntuacion = evaluacionDesempeno.Puntuacion;
                    evaluacionToUpdate.Evaluador = evaluacionDesempeno.Evaluador;
                    evaluacionToUpdate.Comentarios = evaluacionDesempeno.Comentarios;

                    if (!string.IsNullOrEmpty(evaluacionDesempeno.RutaInforme))
                    {
                        evaluacionToUpdate.RutaInforme = evaluacionDesempeno.RutaInforme;
                    }

                    NormalizeDates(evaluacionToUpdate);

                    _context.Update(evaluacionToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!EvaluacionDesempenoExists(evaluacionDesempeno.IdEvaluacion))
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
                return await RedirectToPersonaDetails(evaluacionDesempeno.IdEmpleado);
            }
            return View(evaluacionDesempeno);
        }

        // GET: EvaluacionesDesempeno/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluacionDesempeno = await _context.EvaluacionesDesempeno
                .Include(e => e.Empleado)
                .FirstOrDefaultAsync(m => m.IdEvaluacion == id);
            if (evaluacionDesempeno == null)
            {
                return NotFound();
            }

            return View(evaluacionDesempeno);
        }

        // POST: EvaluacionesDesempeno/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluacionDesempeno = await _context.EvaluacionesDesempeno.FindAsync(id);
            if (evaluacionDesempeno != null)
            {
                if (!string.IsNullOrEmpty(evaluacionDesempeno.RutaInforme))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, evaluacionDesempeno.RutaInforme.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.EvaluacionesDesempeno.Remove(evaluacionDesempeno);
                await _context.SaveChangesAsync();
                return await RedirectToPersonaDetails(evaluacionDesempeno.IdEmpleado);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EvaluacionDesempenoExists(int id)
        {
            return _context.EvaluacionesDesempeno.Any(e => e.IdEvaluacion == id);
        }

        private async Task HandleFileUpload(EvaluacionDesempeno model)
        {
            if (model.ArchivoInforme != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/evaluaciones");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ArchivoInforme.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ArchivoInforme.CopyToAsync(fileStream);
                }
                model.RutaInforme = "/uploads/evaluaciones/" + uniqueFileName;
            }
        }

        private void NormalizeDates(EvaluacionDesempeno model)
        {
            model.FechaEvaluacion = DateTime.SpecifyKind(model.FechaEvaluacion, DateTimeKind.Utc);
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