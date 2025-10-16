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
    public class TrayectoriaEmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrayectoriaEmpleadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrayectoriaEmpleados/Index/{idEmpleado}
        // Muestra el historial de trayectoria para un empleado específico
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index(int? idEmpleado)
        {
            if (idEmpleado == null)
            {
                TempData["ErrorMessage"] = "Debe especificar un empleado para ver su historial de trayectoria.";
                return RedirectToAction("Index", "Empleados");
            }

            var empleado = await _context.Empleados
                                    .Include(e => e.Persona)
                                    .Include(e => e.HistorialTrayectoria.OrderByDescending(t => t.IdHistorialLaboral)) //muestra el orden por id de historial
                                    .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);

            if (empleado == null)
            {
                TempData["ErrorMessage"] = "Empleado no encontrado.";
                return RedirectToAction("Index", "Empleados");
            }

            ViewData["EmpleadoNombreCompleto"] = $"{empleado.Persona?.Nombre} {empleado.Persona?.Apellidos}";
            ViewData["EmpleadoCodigo"] = empleado.CodigoEmpleado;
            ViewData["IdEmpleadoActual"] = idEmpleado.Value;

            return View(empleado.HistorialTrayectoria); // Pasamos la colección de historial laboral a la vista
        }

        // GET: TrayectoriaEmpleados/Details/5
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trayectoriaEmpleado = await _context.TrayectoriaEmpleado
                .Include(t => t.Empleado) // Incluimos el empleado relacionado
                .ThenInclude(e => e!.Persona) // Y la persona del empleado
                .FirstOrDefaultAsync(m => m.IdHistorialLaboral == id);

            if (trayectoriaEmpleado == null)
            {
                return NotFound();
            }

            return View(trayectoriaEmpleado);
        }

        // GET: TrayectoriaEmpleados/Create/{idEmpleado}
        // Este método preparará el formulario para registrar un nuevo cambio
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Create(int? idEmpleado)
        {
            if (idEmpleado == null)
            {
                TempData["ErrorMessage"] = "Debe especificar un empleado para registrar la trayectoria.";
                return RedirectToAction("Index", "Empleados");
            }

            // Cargamos el empleado y su persona asociada para mostrar la información en la vista
            var empleado = await _context.Empleados
                                        .Include(e => e.Persona)
                                        .FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);

            if (empleado == null)
            {
                TempData["ErrorMessage"] = "Empleado no encontrado.";
                return RedirectToAction("Index", "Empleados");

            }

            if (empleado.Estado != EstadoEmpleado.Activo)
            {
                TempData["ErrorMessage"] = $"No se pueden registrar cambios de puesto/salario para el empleado {empleado.Persona?.NombreCompleto} porque su estado es '{empleado.Estado}'. Solo se permiten cambios para empleados 'Activos'.";
                return RedirectToAction("Details", "Empleados", new { id = idEmpleado }); // Redirige de vuelta con un mensaje de error
            }

            // Pre-llenamos el modelo TrayectoriaEmpleado con los datos actuales del empleado
            // Estos serán los "valores anteriores" en el nuevo registro de trayectoria.
            var trayectoria = new TrayectoriaEmpleado
            {
                IdEmpleado = idEmpleado.Value,
                FechaCambio = DateTime.Today, // Por defecto, la fecha actual
                PuestoAnterior = empleado.Puesto,
                SalarioAnterior = empleado.Salario,
                DepartamentoAnterior = empleado.Departamento
            };

            // Pasamos datos adicionales a la vista usando ViewData/ViewBag
            ViewData["EmpleadoNombreCompleto"] = $"{empleado.Persona?.Nombre} {empleado.Persona?.Apellidos}";
            ViewData["EmpleadoCodigo"] = empleado.CodigoEmpleado;

            // Para el dropdown de IdEmpleado (aunque lo ocultemos, scaffolding lo requiere a veces)
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "CodigoEmpleado", idEmpleado);

            return View(trayectoria);
        }

        // POST: TrayectoriaEmpleados/Create
        // Este método guarda el nuevo registro de trayectoria y actualiza el empleado principal
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHistorialLaboral,IdEmpleado,FechaCambio,TipoCambio,PuestoAnterior,PuestoNuevo,SalarioAnterior,SalarioNuevo,DepartamentoAnterior,DepartamentoNuevo,MotivoCambio")] TrayectoriaEmpleado trayectoriaEmpleado)
        {
            // Removemos la validación de los campos que no se envían desde el formulario
            ModelState.Remove(nameof(trayectoriaEmpleado.PuestoAnterior));
            ModelState.Remove(nameof(trayectoriaEmpleado.SalarioAnterior));
            ModelState.Remove(nameof(trayectoriaEmpleado.DepartamentoAnterior));

            ModelState.Remove(nameof(trayectoriaEmpleado.Empleado)); // Ignorar la validación de la propiedad de navegación 'Empleado'
            if (ModelState.IsValid)
            {
                // Paso 1: Obtener el empleado actual desde la base de datos
                var empleado = await _context.Empleados.FindAsync(trayectoriaEmpleado.IdEmpleado);
                if (empleado == null)
                {
                    ModelState.AddModelError("", "Empleado no encontrado. No se pudo registrar el cambio.");
                    // Si el empleado no existe, volvemos a la vista con el error.
                    // Recargar ViewData necesario para la vista.
                    ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "CodigoEmpleado", trayectoriaEmpleado.IdEmpleado);
                    return View(trayectoriaEmpleado);
                }

                // Verificar el estado del empleado antes de registrar cambios
                // Si el empleado no está activo, no se permiten cambios de trayectoria  
                if (empleado.Estado != EstadoEmpleado.Activo)
                {
                    TempData["ErrorMessage"] = $"No se pueden registrar cambios de puesto/salario para el empleado {empleado.Persona?.NombreCompleto} porque su estado es '{empleado.Estado}'. Solo se permiten cambios para empleados 'Activos'.";
                    return RedirectToAction("Details", "Empleados", new { id = trayectoriaEmpleado.IdEmpleado });
                }


                // Paso 2: Convertir fechas a UTC antes de guardar (para PostgreSQL)
                trayectoriaEmpleado.FechaCambio = DateTime.SpecifyKind(trayectoriaEmpleado.FechaCambio, DateTimeKind.Utc);
                trayectoriaEmpleado.FechaRegistro = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc); // Fecha en que se registra el evento

                // Paso 3: Asignar el usuario que registra (ejemplo: usuario autenticado)
                trayectoriaEmpleado.RegistradoPor = User.Identity?.Name ?? "Sistema";

                // Paso 4: Actualizar las propiedades actuales del empleado con los "nuevos" valores
                empleado.Puesto = trayectoriaEmpleado.PuestoNuevo ?? empleado.Puesto;
                empleado.Salario = trayectoriaEmpleado.SalarioNuevo ?? empleado.Salario;
                empleado.Departamento = trayectoriaEmpleado.DepartamentoNuevo ?? empleado.Departamento;

                // Paso 5: Guardar ambos cambios en una sola transacción
                _context.Add(trayectoriaEmpleado); // Agrega el nuevo registro de trayectoria
                _context.Update(empleado);        // Actualiza el empleado principal
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

                TempData["SuccessMessage"] = "Cambio de trayectoria registrado exitosamente.";
                // Redirige a los detalles del empleado para ver el historial actualizado
                return RedirectToAction("Details", "Empleados", new { id = trayectoriaEmpleado.IdEmpleado });
            }

            // Si el ModelState no es válido, volvemos a la vista con los errores.
            // Es importante recargar los ViewData para que la vista se renderice correctamente.
            var currentEmpleado = await _context.Empleados
                                                .Include(e => e.Persona)
                                                .FirstOrDefaultAsync(e => e.IdEmpleado == trayectoriaEmpleado.IdEmpleado);
            if (currentEmpleado != null)
            {
                ViewData["EmpleadoNombreCompleto"] = $"{currentEmpleado.Persona?.Nombre} {currentEmpleado.Persona?.Apellidos}";
                ViewData["EmpleadoCodigo"] = currentEmpleado.CodigoEmpleado;
            }
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "CodigoEmpleado", trayectoriaEmpleado.IdEmpleado);
            return View(trayectoriaEmpleado);
        }

        // GET: TrayectoriaEmpleados/Edit/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trayectoriaEmpleado = await _context.TrayectoriaEmpleado.FindAsync(id);
            if (trayectoriaEmpleado == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoNombreCompleto"] = (await _context.Empleados.Include(e => e.Persona).FirstOrDefaultAsync(e => e.IdEmpleado == trayectoriaEmpleado.IdEmpleado))?.Persona?.Nombre + " " + (await _context.Empleados.Include(e => e.Persona).FirstOrDefaultAsync(e => e.IdEmpleado == trayectoriaEmpleado.IdEmpleado))?.Persona?.Apellidos;
            return View(trayectoriaEmpleado);
        }

        // POST: TrayectoriaEmpleados/Edit/5
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHistorialLaboral,IdEmpleado,FechaCambio,TipoCambio,PuestoAnterior,PuestoNuevo,SalarioAnterior,SalarioNuevo,DepartamentoAnterior,DepartamentoNuevo,MotivoCambio,RegistradoPor,FechaRegistro")] TrayectoriaEmpleado trayectoriaEmpleado)
        {
            if (id != trayectoriaEmpleado.IdHistorialLaboral)
            {
                return NotFound();
            }
            ModelState.Remove(nameof(trayectoriaEmpleado.PuestoAnterior));
            ModelState.Remove(nameof(trayectoriaEmpleado.SalarioAnterior));
            ModelState.Remove(nameof(trayectoriaEmpleado.DepartamentoAnterior));
            if (ModelState.IsValid)
            {
                try
                {
                    trayectoriaEmpleado.FechaCambio = DateTime.SpecifyKind(trayectoriaEmpleado.FechaCambio, DateTimeKind.Utc);
                    trayectoriaEmpleado.FechaRegistro = DateTime.SpecifyKind(trayectoriaEmpleado.FechaRegistro, DateTimeKind.Utc); // Asegura que la fecha de registro también sea UTC

                    _context.Update(trayectoriaEmpleado);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Registro de trayectoria actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrayectoriaEmpleadoExists(trayectoriaEmpleado.IdHistorialLaboral))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Empleados", new { id = trayectoriaEmpleado.IdEmpleado }); // Redirige a los detalles del empleado
            }
            // Si hay errores, recargar ViewData si es necesario para la vista de edición.
            ViewData["EmpleadoNombreCompleto"] = (await _context.Empleados.Include(e => e.Persona).FirstOrDefaultAsync(e => e.IdEmpleado == trayectoriaEmpleado.IdEmpleado))?.Persona?.Nombre + " " + (await _context.Empleados.Include(e => e.Persona).FirstOrDefaultAsync(e => e.IdEmpleado == trayectoriaEmpleado.IdEmpleado))?.Persona?.Apellidos;
            return View(trayectoriaEmpleado);
        }

        // GET: TrayectoriaEmpleados/Delete/5
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trayectoriaEmpleado = await _context.TrayectoriaEmpleado
                .Include(t => t.Empleado)
                .ThenInclude(e => e!.Persona)
                .FirstOrDefaultAsync(m => m.IdHistorialLaboral == id);

            if (trayectoriaEmpleado == null)
            {
                return NotFound();
            }

            return View(trayectoriaEmpleado);
        }

        // POST: TrayectoriaEmpleados/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trayectoriaEmpleado = await _context.TrayectoriaEmpleado.FindAsync(id);
            if (trayectoriaEmpleado != null)
            {
                _context.TrayectoriaEmpleado.Remove(trayectoriaEmpleado);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registro de trayectoria eliminado exitosamente.";
                // Redirige de nuevo a los detalles del empleado, o al Index de trayectoria.
                return RedirectToAction("Details", "Empleados", new { id = trayectoriaEmpleado.IdEmpleado });
            }
            return NotFound();
        }

        private bool TrayectoriaEmpleadoExists(int id)
        {
            return _context.TrayectoriaEmpleado.Any(e => e.IdHistorialLaboral == id);
        }
    }
}