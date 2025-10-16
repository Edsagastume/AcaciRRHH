using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AcaciRRHH.Web.Controllers
{
    public class AtestadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AtestadosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Atestados/Create
        [Authorize(Policy = "Visualizador")]
        public IActionResult Create(int idPersona)
        {
            var atestado = new Atestado { IdPersona = idPersona };
            return View(atestado);
        }

        // POST: Atestados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Create(Atestado atestado, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                if (archivo != null && archivo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "atestados");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + archivo.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivo.CopyToAsync(fileStream);
                    }

                    atestado.RutaArchivo = "/" + Path.Combine("uploads", "atestados", uniqueFileName).Replace('\\', '/');
                    atestado.NombreOriginalArchivo = archivo.FileName;
                    atestado.ExtensionArchivo = Path.GetExtension(archivo.FileName);
                    atestado.FechaSubida = DateTime.UtcNow;

                    _context.Add(atestado);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Personas", new { id = atestado.IdPersona });
                }
                else
                {
                    ModelState.AddModelError("RutaArchivo", "Por favor, seleccione un archivo.");
                }
            }
            return View(atestado);
        }

        // GET: Atestados/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atestado = await _context.Atestados
                .Include(a => a.Persona)
                .FirstOrDefaultAsync(m => m.IdAtestado == id);
            if (atestado == null)
            {
                return NotFound();
            }

            return View(atestado);
        }

        // GET: Atestados/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atestado = await _context.Atestados.FindAsync(id);
            if (atestado == null)
            {
                return NotFound();
            }
            return View(atestado);
        }

        // POST: Atestados/Edit/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAtestado,IdPersona,TipoAtestado,NombreOriginalArchivo,ExtensionArchivo,FechaSubida,RutaArchivo,Comentarios,Activo,SubidoPor")] Atestado atestado, IFormFile? archivo)
        {
            if (id != atestado.IdAtestado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var atestadoToUpdate = await _context.Atestados.FindAsync(id);
                    if (atestadoToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Manejar la carga del nuevo archivo si se proporciona uno
                    if (archivo != null && archivo.Length > 0)
                    {
                        // Eliminar el archivo antiguo si existe
                        if (!string.IsNullOrEmpty(atestadoToUpdate.RutaArchivo))
                        {
                            var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, atestadoToUpdate.RutaArchivo.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "atestados");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + archivo.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await archivo.CopyToAsync(fileStream);
                        }

                        atestadoToUpdate.RutaArchivo = "/" + Path.Combine("uploads", "atestados", uniqueFileName).Replace('\\', '/');
                        atestadoToUpdate.NombreOriginalArchivo = archivo.FileName;
                        atestadoToUpdate.ExtensionArchivo = Path.GetExtension(archivo.FileName);
                        atestadoToUpdate.FechaSubida = DateTime.UtcNow; // Actualizar la fecha de subida
                    }

                    // Actualizar otros campos si es necesario
                    atestadoToUpdate.TipoAtestado = atestado.TipoAtestado;
                    atestadoToUpdate.Comentarios = atestado.Comentarios;
                    atestadoToUpdate.Activo = atestado.Activo;
                    atestadoToUpdate.SubidoPor = atestado.SubidoPor; // Actualizar quien subió el archivo

                    _context.Update(atestadoToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AtestadoExists(atestado.IdAtestado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Personas", new { id = atestado.IdPersona });
            }
            return View(atestado);
        }

        // GET: Atestados/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atestado = await _context.Atestados
                .Include(a => a.Persona)
                .FirstOrDefaultAsync(m => m.IdAtestado == id);
            if (atestado == null)
            {
                return NotFound();
            }

            return View(atestado);
        }

        // POST: Atestados/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atestado = await _context.Atestados.FindAsync(id);
            if (atestado != null)
            {
                // Eliminar el archivo físico
                if (!string.IsNullOrEmpty(atestado.RutaArchivo))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, atestado.RutaArchivo.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Atestados.Remove(atestado);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Personas", new { id = atestado.IdPersona });
            }
            return NotFound();
        }

        private bool AtestadoExists(int id)
        {
            return _context.Atestados.Any(e => e.IdAtestado == id);
        }
    }
}