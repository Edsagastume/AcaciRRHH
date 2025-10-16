#nullable disable

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
    public class VacacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VacacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vacaciones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vacaciones.Include(v => v.Empleado);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vacaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacion = await _context.Vacaciones
                .Include(v => v.Empleado)
                    .ThenInclude(e => e.Persona)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacacion == null)
            {
                return NotFound();
            }

            return View(vacacion);
        }

        // GET: Vacaciones/Create
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

            if (empleado.Persona is not null)
            {
                ViewData["NombreEmpleado"] = empleado.Persona.NombreCompleto;
            }
            else
            {
                ViewData["NombreEmpleado"] = "Empleado no encontrado";
            }
            var vacacion = new Vacacion { IdEmpleado = idEmpleado.Value };
            return View(vacacion);
        }

        // POST: Vacaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,FechaSolicitud,FechaInicio,FechaFin,Aprobado,Comentarios,FechasNoConsecutivas,SonConsecutivas")] Vacacion vacacion)
        {
            try
            {
                vacacion.FechaSolicitud = DateTime.SpecifyKind(vacacion.FechaSolicitud, DateTimeKind.Utc);
                if (vacacion.FechaInicio.HasValue)
                {
                    vacacion.FechaInicio = DateTime.SpecifyKind(vacacion.FechaInicio.Value, DateTimeKind.Utc);
                }
                if (vacacion.FechaFin.HasValue)
                {
                    vacacion.FechaFin = DateTime.SpecifyKind(vacacion.FechaFin.Value, DateTimeKind.Utc);
                }

                if (vacacion.SonConsecutivas)
                {
                    if (vacacion.FechaInicio.HasValue && vacacion.FechaFin.HasValue)
                    {
                        vacacion.DiasSolicitados = (int)(vacacion.FechaFin.Value - vacacion.FechaInicio.Value).TotalDays + 1;
                    }
                    else
                    {
                        vacacion.DiasSolicitados = 0;
                    }
                }
                else if (!string.IsNullOrEmpty(vacacion.FechasNoConsecutivas))
                {
                    vacacion.DiasSolicitados = vacacion.FechasNoConsecutivas.Split(',').Length;
                }
                else
                {
                    vacacion.DiasSolicitados = 0;
                }

                _context.Add(vacacion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Empleados", new { id = vacacion.IdEmpleado });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Vacacion: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Error al guardar la vacación: " + ex.Message);
            }

            var empleado = await _context.Empleados.Include(e => e.Persona).FirstOrDefaultAsync(e => e.IdEmpleado == vacacion.IdEmpleado);
            if (empleado is not null && empleado.Persona is not null)
            {
                ViewData["NombreEmpleado"] = empleado.Persona.NombreCompleto;
            }
            else
            {
                ViewData["NombreEmpleado"] = "Empleado no encontrado";
            }
            return View(vacacion);
        }

        // GET: Vacaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacion = await _context.Vacaciones
                .Include(v => v.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vacacion == null)
            {
                return NotFound();
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "CodigoEmpleado", vacacion.IdEmpleado);
            return View(vacacion);
        }

        // POST: Vacaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var vacacionToUpdate = await _context.Vacaciones
                .Include(v => v.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vacacionToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Vacacion>(
                vacacionToUpdate,
                "",
                v => v.FechaSolicitud, v => v.FechaInicio, v => v.FechaFin, v => v.Aprobado, v => v.Comentarios, v => v.FechasNoConsecutivas, v => v.SonConsecutivas))
            {
                if (vacacionToUpdate.SonConsecutivas)
                {
                    if (vacacionToUpdate.FechaInicio.HasValue && vacacionToUpdate.FechaFin.HasValue)
                    {
                        vacacionToUpdate.DiasSolicitados = (int)(vacacionToUpdate.FechaFin.Value - vacacionToUpdate.FechaInicio.Value).TotalDays + 1;
                    }
                    else
                    {
                        vacacionToUpdate.DiasSolicitados = 0;
                    }
                }
                else if (!string.IsNullOrEmpty(vacacionToUpdate.FechasNoConsecutivas))
                {
                    vacacionToUpdate.DiasSolicitados = vacacionToUpdate.FechasNoConsecutivas.Split(',').Length;
                }
                else
                {
                    vacacionToUpdate.DiasSolicitados = 0;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VacacionExists(vacacionToUpdate.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Empleados", new { id = vacacionToUpdate.IdEmpleado });
            }

            return View(vacacionToUpdate);
        }

        // GET: Vacaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vacacion = await _context.Vacaciones
                .Include(v => v.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vacacion == null)
            {
                return NotFound();
            }

            return View(vacacion);
        }

        // POST: Vacaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vacacion = await _context.Vacaciones.FindAsync(id);
            if (vacacion != null)
            {
                _context.Vacaciones.Remove(vacacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VacacionExists(int id)
        {
            return _context.Vacaciones.Any(e => e.Id == id);
        }
    }
}
