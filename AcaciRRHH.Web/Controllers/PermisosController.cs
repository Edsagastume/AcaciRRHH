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
    public class PermisosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PermisosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Permisos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Permisos.Include(p => p.Empleado);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Permisos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // GET: Permisos/Create
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
            var permiso = new Permiso { IdEmpleado = idEmpleado.Value };
            return View(permiso);
        }

                // POST: Permisos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,FechaSolicitud,FechaInicio,FechaFin,Motivo,Aprobado,Comentarios")] Permiso permiso)
        {
            try
            {
                permiso.FechaSolicitud = DateTime.SpecifyKind(permiso.FechaSolicitud, DateTimeKind.Utc);
                permiso.FechaInicio = DateTime.SpecifyKind(permiso.FechaInicio, DateTimeKind.Utc);
                permiso.FechaFin = DateTime.SpecifyKind(permiso.FechaFin, DateTimeKind.Utc);

                _context.Add(permiso);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Empleados", new { id = permiso.IdEmpleado });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving Permiso: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Error al guardar el permiso: " + ex.Message);
            }

            var empleado = await _context.Empleados.Include(e => e.Persona).FirstOrDefaultAsync(e => e.IdEmpleado == permiso.IdEmpleado);
            if (empleado != null && empleado.Persona != null)
            {
                ViewData["NombreEmpleado"] = empleado.Persona.NombreCompleto;
            }
            else
            {
                ViewData["NombreEmpleado"] = "Empleado no encontrado";
            }
            return View(permiso);
        }

        // GET: Permisos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "CodigoEmpleado", permiso.IdEmpleado);
            return View(permiso);
        }

        // POST: Permisos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpleado,FechaSolicitud,FechaInicio,FechaFin,Motivo,Aprobado,Comentarios,CreatedBy,CreatedAt,LastModifiedBy,LastModifiedAt")] Permiso permiso)
        {
            if (id != permiso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permiso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermisoExists(permiso.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "CodigoEmpleado", permiso.IdEmpleado);
            return View(permiso);
        }

        // GET: Permisos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permiso = await _context.Permisos
                .Include(p => p.Empleado)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (permiso == null)
            {
                return NotFound();
            }

            return View(permiso);
        }

        // POST: Permisos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso != null)
            {
                _context.Permisos.Remove(permiso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Empleados", new { id = permiso?.IdEmpleado });
        }

        private bool PermisoExists(int id)
        {
            return _context.Permisos.Any(e => e.Id == id);
        }
    }
}