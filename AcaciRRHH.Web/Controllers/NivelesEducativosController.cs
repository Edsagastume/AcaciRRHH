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

namespace AcaciRRHH.Web.Controllers
{
    public class NivelesEducativosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NivelesEducativosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NivelesEducativos
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NivelesEducativos.Include(n => n.Persona);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NivelesEducativos/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nivelEducativo = await _context.NivelesEducativos
                .Include(n => n.Persona)
                .FirstOrDefaultAsync(m => m.IdNivelEducativo == id);
            if (nivelEducativo == null)
            {
                return NotFound();
            }

            return View(nivelEducativo);
        }

        // GET: NivelesEducativos/Create
        [Authorize(Policy = "Administrador")]
        public IActionResult Create(int idPersona)
        {
            var viewModel = new NivelEducativo
            {
                IdPersona = idPersona
            };
            return View(viewModel);
        }

        // POST: NivelesEducativos/Create
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,GradoAcademico,Institucion,TituloObtenido,FechaInicio,FechaFin,Comentarios")] NivelEducativo nivelEducativo)
        {
            if (ModelState.IsValid)
            {
                if (nivelEducativo.FechaInicio.HasValue)
                {
                    nivelEducativo.FechaInicio = DateTime.SpecifyKind(nivelEducativo.FechaInicio.Value, DateTimeKind.Utc);
                }
                if (nivelEducativo.FechaFin.HasValue)
                {
                    nivelEducativo.FechaFin = DateTime.SpecifyKind(nivelEducativo.FechaFin.Value, DateTimeKind.Utc);
                }
                _context.Add(nivelEducativo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Personas", new { id = nivelEducativo.IdPersona });
            }
            return View(nivelEducativo);
        }

        // GET: NivelesEducativos/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nivelEducativo = await _context.NivelesEducativos.FindAsync(id);
            if (nivelEducativo == null)
            {
                return NotFound();
            }
            return View(nivelEducativo);
        }

        // POST: NivelesEducativos/Edit/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNivelEducativo,IdPersona,GradoAcademico,Institucion,TituloObtenido,FechaInicio,FechaFin,Comentarios")] NivelEducativo nivelEducativo)
        {
            if (id != nivelEducativo.IdNivelEducativo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (nivelEducativo.FechaInicio.HasValue)
                    {
                        nivelEducativo.FechaInicio = DateTime.SpecifyKind(nivelEducativo.FechaInicio.Value, DateTimeKind.Utc);
                    }
                    if (nivelEducativo.FechaFin.HasValue)
                    {
                        nivelEducativo.FechaFin = DateTime.SpecifyKind(nivelEducativo.FechaFin.Value, DateTimeKind.Utc);
                    }
                    _context.Update(nivelEducativo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NivelEducativoExists(nivelEducativo.IdNivelEducativo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Personas", new { id = nivelEducativo.IdPersona });
            }
            return View(nivelEducativo);
        }

        // GET: NivelesEducativos/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nivelEducativo = await _context.NivelesEducativos
                .Include(n => n.Persona)
                .FirstOrDefaultAsync(m => m.IdNivelEducativo == id);
            if (nivelEducativo == null)
            {
                return NotFound();
            }

            return View(nivelEducativo);
        }

        // POST: NivelesEducativos/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nivelEducativo = await _context.NivelesEducativos.FindAsync(id);
            if (nivelEducativo != null)
            {
                _context.NivelesEducativos.Remove(nivelEducativo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Personas", new { id = nivelEducativo.IdPersona });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NivelEducativoExists(int id)
        {
            return _context.NivelesEducativos.Any(e => e.IdNivelEducativo == id);
        }
    }
}
