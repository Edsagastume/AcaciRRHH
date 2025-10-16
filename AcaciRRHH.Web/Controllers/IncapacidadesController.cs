using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;

namespace AcaciRRHH.Web.Controllers
{
    public class IncapacidadesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncapacidadesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Incapacidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incapacidad = await _context.Incapacidades
                .Include(i => i.Empleado)
                .ThenInclude(e => e.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incapacidad == null)
            {
                return NotFound();
            }

            return View(incapacidad);
        }

        // GET: Incapacidades/Create
        public IActionResult Create(int? idEmpleado)
        {
            if (idEmpleado == null)
            {
                return NotFound();
            }
            var empleado = _context.Empleados.Include(e => e.Persona).FirstOrDefault(e => e.IdEmpleado == idEmpleado.Value);
            if (empleado == null)
            {
                return NotFound();
            }

            ViewData["NombreEmpleado"] = empleado.Persona?.NombreCompleto ?? "Empleado no encontrado";
            var incapacidad = new Incapacidad { IdEmpleado = idEmpleado.Value };
            return View(incapacidad);
        }

        // POST: Incapacidades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,FechaInicio,FechaFin,Motivo,Comentarios")] Incapacidad incapacidad)
        {
            try
            {
                var empleado = await _context.Empleados.FindAsync(incapacidad.IdEmpleado);
                if (empleado == null)
                {
                    ModelState.AddModelError("IdEmpleado", "Empleado no encontrado.");
                    ViewData["IdEmpleado"] = incapacidad.IdEmpleado;
                    var emp = _context.Empleados.Include(e => e.Persona).FirstOrDefault(e => e.IdEmpleado == incapacidad.IdEmpleado);
                    ViewData["NombreEmpleado"] = emp?.Persona?.NombreCompleto ?? "Empleado no encontrado";
                    return View(incapacidad);
                }

                incapacidad.Empleado = empleado;
                incapacidad.FechaInicio = DateTime.SpecifyKind(incapacidad.FechaInicio, DateTimeKind.Utc);
                incapacidad.FechaFin = DateTime.SpecifyKind(incapacidad.FechaFin, DateTimeKind.Utc);

                _context.Add(incapacidad);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Empleados", new { id = incapacidad.IdEmpleado });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Incapacidad: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Error al guardar la incapacidad: " + ex.Message);
            }

            ViewData["IdEmpleado"] = incapacidad.IdEmpleado;
            var empleado2 = _context.Empleados.Include(e => e.Persona).FirstOrDefault(e => e.IdEmpleado == incapacidad.IdEmpleado);
            ViewData["NombreEmpleado"] = empleado2?.Persona?.NombreCompleto ?? "Empleado no encontrado";
            return View(incapacidad);
        }

        // GET: Incapacidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incapacidad = await _context.Incapacidades.FindAsync(id);
            if (incapacidad == null)
            {
                return NotFound();
            }
            return View(incapacidad);
        }

        // POST: Incapacidades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpleado,FechaInicio,FechaFin,Motivo,Comentarios")] Incapacidad incapacidad)
        {
            if (id != incapacidad.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incapacidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncapacidadExists(incapacidad.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Empleados", new { id = incapacidad.IdEmpleado });
            }
            return View(incapacidad);
        }

        // GET: Incapacidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incapacidad = await _context.Incapacidades
                .Include(i => i.Empleado)
                .ThenInclude(e => e.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incapacidad == null)
            {
                return NotFound();
            }

            return View(incapacidad);
        }

        // POST: Incapacidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incapacidad = await _context.Incapacidades.FindAsync(id);
            if (incapacidad != null)
            {
                _context.Incapacidades.Remove(incapacidad);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Empleados", new { id = incapacidad.IdEmpleado });
            }
            return RedirectToAction("Index", "Empleados");
        }

        private bool IncapacidadExists(int id)
        {
            return _context.Incapacidades.Any(e => e.Id == id);
        }
    }
}

