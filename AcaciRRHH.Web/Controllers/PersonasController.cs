using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Npgsql;

namespace AcaciRRHH.Web.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Personas
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index(string searchString)
        {
            var personas = from p in _context.Personas
                        select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                personas = personas.Where(s => s.Nombre.Contains(searchString)
                                            || s.Apellidos.Contains(searchString));
            }

            personas = personas.OrderBy(p => p.Nombre);

            ViewData["CurrentFilter"] = searchString;

            // Incluimos Empleado aquí también para saber si es empleado en la lista
            // Si la relación es 1 a 0..1 (una Persona puede o no ser un Empleado)
            return View(await personas.Include(p => p.Empleado).Include(p => p.Directivos).ToListAsync());
        }

        // GET: Personas/Create
        [Authorize(Policy = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellidos,FechaNacimiento,Genero,DUI,FechaVencimientoDUI,NIT,Email,Telefono,Direccion,NumeroAFP,TipoAFP")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                persona.FechaNacimiento = DateTime.SpecifyKind(persona.FechaNacimiento, DateTimeKind.Utc);
                if (persona.FechaVencimientoDUI.HasValue)
                {
                    persona.FechaVencimientoDUI = DateTime.SpecifyKind(persona.FechaVencimientoDUI.Value, DateTimeKind.Utc);
                }
                _context.Add(persona);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    var postgresException = ex.InnerException as Npgsql.PostgresException;
                    if (postgresException != null && postgresException.SqlState == "23505")
                    {
                        if (postgresException.ConstraintName == "IX_Personas_NIT")
                        {
                            ModelState.AddModelError("NIT", "El NIT ingresado ya existe. Por favor, ingrese un NIT único.");
                        }
                        else if (postgresException.ConstraintName == "IX_Personas_DUI")
                        {
                            ModelState.AddModelError("DUI", "El DUI ingresado ya existe. Por favor, ingrese un DUI único.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Error de duplicidad. Un registro con los mismos datos ya existe.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar la persona. Por favor, inténtelo de nuevo.");
                    }
                }
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        // Ahora solo carga los datos de la Persona (sin Empleado en el formulario)
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var persona = await _context.Personas
                                        .FirstOrDefaultAsync(m => m.IdPersona == id);

            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Ahora solo actualiza los datos de la Persona
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersona,Nombre,Apellidos,FechaNacimiento,Genero,DUI,FechaVencimientoDUI,NIT,Email,Telefono,Direccion,NumeroAFP,TipoAFP")] Persona persona)
        {
            if (id != persona.IdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    persona.FechaNacimiento = DateTime.SpecifyKind(persona.FechaNacimiento, DateTimeKind.Utc);
                    if (persona.FechaVencimientoDUI.HasValue)
                    {
                        persona.FechaVencimientoDUI = DateTime.SpecifyKind(persona.FechaVencimientoDUI.Value, DateTimeKind.Utc);
                    }
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.IdPersona))
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
            return View(persona);
        }

        // GET: Personas/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.Capacitaciones)
                .Include(p => p.Atestados)
                .Include(p => p.NivelesEducativos)
                .Include(p => p.Empleado)
                    .ThenInclude(e => e!.HistorialDisciplinario)
                .Include(p => p.Empleado)
                    .ThenInclude(e => e!.EvaluacionesDesempeno)
                .Include(p => p.Directivos)
                .FirstOrDefaultAsync(m => m.IdPersona == id);

            if (persona == null)
            {
                return NotFound();
            }

            // Order the collections
            persona.Capacitaciones = persona.Capacitaciones?.OrderByDescending(c => c.IdCapacitacionEmpleado).ToList() ?? new List<CapacitacionEmpleado>();
            persona.Atestados = persona.Atestados?.OrderByDescending(a => a.IdAtestado).ToList() ?? new List<Atestado>();
            persona.NivelesEducativos = persona.NivelesEducativos?.OrderByDescending(n => n.IdNivelEducativo).ToList() ?? new List<NivelEducativo>();

            if (persona.Empleado != null)
            {
                persona.Empleado.HistorialDisciplinario = persona.Empleado.HistorialDisciplinario?.OrderByDescending(h => h.IdIncidente).ToList() ?? new List<HistorialDisciplinario>();
                persona.Empleado.EvaluacionesDesempeno = persona.Empleado.EvaluacionesDesempeno?.OrderByDescending(ev => ev.IdEvaluacion).ToList() ?? new List<EvaluacionDesempeno>();
            }

            return View(persona);
        }

        // GET: Personas/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Incluimos Empleado para verificar si la persona es un empleado antes de borrar
            var persona = await _context.Personas
                .Include(p => p.Empleado)
                .FirstOrDefaultAsync(m => m.IdPersona == id);

            if (persona == null)
            {
                return NotFound();
            }

            if (persona.Empleado != null)
            {
                ViewData["ErrorMessage"] = "No se puede eliminar una persona que está registrada como empleado. Primero debe dar de baja al empleado.";
                return View(persona); // Retorna la vista con el error.
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona != null)
            {
                // Re-verificar si es empleado antes de eliminar para evitar problemas de FK
                var isEmpleado = await _context.Empleados.AnyAsync(e => e.IdPersona == id);
                if (isEmpleado)
                {
                    return RedirectToAction(nameof(Details), new { id = id, ErrorMessage = "No se puede eliminar una persona que es empleado." });
                }

                _context.Personas.Remove(persona);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> AllCapacitaciones(int id)
        {
            var persona = await _context.Personas
                                        .Include(p => p.Capacitaciones)
                                        .FirstOrDefaultAsync(p => p.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }
            return View("AllCapacitaciones", persona);
        }

        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> AllNivelesEducativos(int id)
        {
            var persona = await _context.Personas
                                        .Include(p => p.NivelesEducativos)
                                        .FirstOrDefaultAsync(p => p.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }
            return View("AllNivelesEducativos", persona);
        }

        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> AllAtestados(int id)
        {
            var persona = await _context.Personas
                                        .Include(p => p.Atestados)
                                        .FirstOrDefaultAsync(p => p.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }
            return View("AllAtestados", persona);
        }

        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> AllHistorialDisciplinario(int id)
        {
            var persona = await _context.Personas
                                        .Include(p => p.Empleado)
                                            .ThenInclude(e => e!.HistorialDisciplinario)
                                        .FirstOrDefaultAsync(p => p.IdPersona == id);
            if (persona == null || persona.Empleado == null)
            {
                return NotFound();
            }
            return View("AllHistorialDisciplinario", persona);
        }

        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> AllEvaluacionesDesempeno(int id)
        {
            var persona = await _context.Personas
                                        .Include(p => p.Empleado)
                                            .ThenInclude(e => e!.EvaluacionesDesempeno)
                                        .FirstOrDefaultAsync(p => p.IdPersona == id);
            if (persona == null || persona.Empleado == null)
            {
                return NotFound();
            }
            return View("AllEvaluacionesDesempeno", persona);
        }


        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.IdPersona == id);
        }
    }
}