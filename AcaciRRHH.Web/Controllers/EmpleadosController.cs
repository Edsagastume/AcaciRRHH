using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList
using System; // Necesario para DateTime
using AcaciRRHH.Web.ViewModels; // Importa el ViewModel de terminación de empleado
using Microsoft.AspNetCore.Authorization; 


namespace AcaciRRHH.Web.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Inyección de dependencias del contexto de la base de datos

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
            
        }

        // GET: Empleados
        // Muestra una lista de todos los empleados
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Index(string searchString)
        {
            var empleados = from e in _context.Empleados
                            select e;

            // Incluir la entidad relacionada Persona para mostrar nombres en la vista
            empleados = empleados.Include(e => e.Persona);

            if (!string.IsNullOrEmpty(searchString))
            {
                empleados = empleados.Where(e => e.Persona!.Nombre.Contains(searchString) ||
                                                    e.Persona.Apellidos.Contains(searchString) ||
                                                    e.CodigoEmpleado.Contains(searchString) ||
                                                    (e.Puesto != null && e.Puesto.Contains(searchString)) ||
                                                    (e.Departamento != null && e.Departamento.Contains(searchString)));
            }

            empleados = empleados.OrderBy(e => e.Persona != null ? e.Persona.Nombre : string.Empty)
                                .ThenBy(e => e.Persona != null ? e.Persona.Apellidos : string.Empty);

            ViewData["CurrentFilter"] = searchString;

            return View(await empleados.ToListAsync());
        }

        // GET: Empleados/Details/5
        // Muestra los detalles de un empleado específico
        [Authorize(Policy = "Visualizador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.HistorialTrayectoria)
                .Include(e => e.Persona)
                .Include(e => e.Incapacidades)
                .Include(e => e.Permisos)
                .Include(e => e.Vacaciones)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);

            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        // Muestra el formulario para crear un nuevo empleado
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Create()
        {
            await PopulatePersonasNoEmpleadas(); // Usamos el método auxiliar
            return View();
        }

        // POST: Empleados/Create
        // Procesa el formulario para crear un nuevo empleado
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,CodigoEmpleado,FechaContratacion,Puesto,Salario,Departamento,Agencia")] Empleado empleado)
        {
            // 1. Validar el estado del modelo según las DataAnnotations
            if (!ModelState.IsValid)
            {
                await PopulatePersonasNoEmpleadas(empleado.IdPersona); // Recarga el SelectList manteniendo la selección
                return View(empleado); // Retorna la vista con errores de validación
            }

            // 2. Verificar si la persona seleccionada ya es un empleado
            if (await _context.Empleados.AnyAsync(e => e.IdPersona == empleado.IdPersona))
            {
                ModelState.AddModelError("IdPersona", "Esta persona ya está registrada como empleado.");
                await PopulatePersonasNoEmpleadas(empleado.IdPersona); // Recarga el SelectList manteniendo la selección
                return View(empleado); // Retorna la vista con el error de duplicado
            }

            // 3. Si todo está bien, procede con la creación
            empleado.FechaContratacion = DateTime.SpecifyKind(empleado.FechaContratacion, DateTimeKind.Utc);
            empleado.Estado = EstadoEmpleado.Activo; // Asegura de que el nuevo empleado esté activo

            _context.Add(empleado);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Empleado creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Empleados/Edit/5
        // Muestra el formulario para editar un empleado existente
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                                        .Include(e => e.Persona) // Incluimos la Persona para mostrar su nombre
                                        .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // Procesa el formulario para actualizar un empleado existente
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("IdEmpleado,IdPersona,CodigoEmpleado,FechaContratacion,Puesto,Salario,Departamento,Estado,FechaFinContrato,MotivoFinalizacion,TipoFinalizacion")] Empleado empleado) // Añadidos los campos del estado
        {
            if (id != empleado.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el empleado actual de la BD para preservar campos que no se bindean
                    var empleadoToUpdate = await _context.Empleados.AsNoTracking().FirstOrDefaultAsync(e => e.IdEmpleado == id);
                    if (empleadoToUpdate == null)
                    {
                        return NotFound();
                    }

                    // Mapear los valores bindeados al objeto que se va a actualizar
                    empleadoToUpdate.CodigoEmpleado = empleado.CodigoEmpleado;
                    empleadoToUpdate.FechaContratacion = DateTime.SpecifyKind(empleado.FechaContratacion, DateTimeKind.Utc);
                    empleadoToUpdate.Puesto = empleado.Puesto;
                    empleadoToUpdate.Salario = empleado.Salario;
                    empleadoToUpdate.Departamento = empleado.Departamento;
                    empleadoToUpdate.Estado = empleado.Estado;
                    if (empleado.FechaFinContrato.HasValue)
                    {
                        empleadoToUpdate.FechaFinContrato = DateTime.SpecifyKind(empleado.FechaFinContrato.Value, DateTimeKind.Utc);
                    }
                    else
                    {
                        empleadoToUpdate.FechaFinContrato = null; // Permitir limpiar la fecha de fin de contrato
                    }
                    empleadoToUpdate.MotivoFinalizacion = empleado.MotivoFinalizacion;
                    empleadoToUpdate.TipoFinalizacion = empleado.TipoFinalizacion;

                    _context.Update(empleadoToUpdate);
                    await _context.SaveChangesAsync();

                    // Eliminar toda la lógica de guardado de documentos de aquí si la tenías:
                    // if (nuevoDocumentoAtestado != null && nuevoDocumentoAtestado.Length > 0) { ... }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.IdEmpleado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Empleado actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            // Para asegurar que la persona esté disponible en caso de error de validación en el POST de Edit:
            empleado.Persona = (await _context.Personas.FindAsync(empleado.IdPersona))!;
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        // Muestra la confirmación para eliminar un empleado
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Persona)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Empleado eliminado exitosamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Empleados/Terminate/{id}
        // Muestra el formulario para finalizar la relación laboral de un empleado
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Terminate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Persona)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);

            if (empleado == null)
            {
                return NotFound();
            }

            if (empleado.Estado != EstadoEmpleado.Activo)
            {
                TempData["WarningMessage"] = $"La relación laboral de {empleado.Persona?.NombreCompleto} ya está {empleado.Estado.ToString().ToLower()}.";
                return RedirectToAction(nameof(Details), new { id = empleado.IdEmpleado });
            }

            var model = new EmpleadoTerminationViewModel
            {
                IdEmpleado = empleado.IdEmpleado,
                FechaFinContrato = DateTime.Today,
                NombreCompletoEmpleado = empleado.Persona?.NombreCompleto,
                FechaContratacion = empleado.FechaContratacion
            };
            return View(model);
        }

        // POST: Empleados/Terminate/{id}
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Terminate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TerminateConfirmed(int id, EmpleadoTerminationViewModel terminationForm)
        {
            if (id != terminationForm.IdEmpleado)
            {
                return NotFound();
            }

            var empleadoExistente = await _context.Empleados
                .Include(e => e.Persona)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);

            if (empleadoExistente == null)
            {
                return NotFound();
            }

            // --- Manual validation using ViewModel properties ---
            if (terminationForm.FechaFinContrato == null)
            {
                ModelState.AddModelError("FechaFinContrato", "La fecha de fin de relación laboral es obligatoria.");
            }
            else if (terminationForm.FechaFinContrato.Value.Date > DateTime.UtcNow.Date.AddDays(1))
            {
                ModelState.AddModelError("FechaFinContrato", "La fecha de finalización no puede ser una fecha futura significativa.");
            }
            else if (terminationForm.FechaFinContrato.Value.Date < empleadoExistente.FechaContratacion.Date)
            {
                ModelState.AddModelError("FechaFinContrato", "La fecha de finalización no puede ser anterior a la fecha de contratación.");
            }

            if (terminationForm.TipoFinalizacion == null)
            {
                ModelState.AddModelError("TipoFinalizacion", "Debe seleccionar un tipo de finalización.");
            }

            if (!ModelState.IsValid)
            {
                var modelToReturn = new EmpleadoTerminationViewModel
                {
                    IdEmpleado = empleadoExistente.IdEmpleado,
                    FechaFinContrato = terminationForm.FechaFinContrato,
                    MotivoFinalizacion = terminationForm.MotivoFinalizacion,
                    TipoFinalizacion = terminationForm.TipoFinalizacion,
                    NombreCompletoEmpleado = empleadoExistente.Persona?.NombreCompleto,
                    FechaContratacion = empleadoExistente.FechaContratacion
                };
                return View("Terminate", modelToReturn);
            }

            // Guardar el estado actual del empleado antes de modificarlo para el historial
            var puestoActual = empleadoExistente.Puesto;
            var salarioActual = empleadoExistente.Salario;
            var departamentoActual = empleadoExistente.Departamento;


            // Aplicar cambios al empleado existente
            empleadoExistente.Estado = EstadoEmpleado.Inactivo;
            empleadoExistente.FechaFinContrato = terminationForm.FechaFinContrato.HasValue
                ? DateTime.SpecifyKind(terminationForm.FechaFinContrato.Value, DateTimeKind.Utc)
                : (DateTime?)null;
            empleadoExistente.MotivoFinalizacion = terminationForm.MotivoFinalizacion;
            empleadoExistente.TipoFinalizacion = terminationForm.TipoFinalizacion;

            // --- REGISTRAR EN TRAYECTORIAEMPLEADO (FINALIZACIÓN) ---
            var trayectoriaInactivacion = new TrayectoriaEmpleado
            {
                IdEmpleado = empleadoExistente.IdEmpleado,
                FechaCambio = empleadoExistente.FechaFinContrato.GetValueOrDefault(DateTime.UtcNow), // Fecha de finalización
                TipoCambio = "Inactivación de Empleado",
                PuestoAnterior = puestoActual, // Puesto que tenía al momento de la inactivación
                PuestoNuevo = puestoActual, // No hay cambio de puesto, solo de estado
                SalarioAnterior = salarioActual, // Salario que tenía al momento de la inactivación
                SalarioNuevo = salarioActual, // No hay cambio de salario, solo de estado
                DepartamentoAnterior = departamentoActual, // Departamento que tenía al momento de la inactivación
                DepartamentoNuevo = departamentoActual, // No hay cambio de departamento, solo de estado
                MotivoCambio = $"Finalización: {empleadoExistente.MotivoFinalizacion} ({empleadoExistente.TipoFinalizacion})",
                RegistradoPor = User.Identity?.Name ?? "Sistema", // Quien realiza la acción
                FechaRegistro = DateTime.UtcNow
            };
            _context.TrayectoriasEmpleado.Add(trayectoriaInactivacion);
            // --- FIN REGISTRO ---

            try
            {
                _context.Update(empleadoExistente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Relación laboral de {empleadoExistente.Persona?.NombreCompleto} finalizada exitosamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(empleadoExistente.IdEmpleado))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Details), new { id = empleadoExistente.IdEmpleado });
        }

        // GET: Empleados/Reactivate/{id}
        // Muestra el formulario de confirmación para reactivar la relación laboral de un empleado
        [Authorize(Policy = "Administrador")]
        public async Task<IActionResult> Reactivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Persona)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);

            if (empleado == null)
            {
                return NotFound();
            }

            if (empleado.Estado == EstadoEmpleado.Activo)
            {
                TempData["WarningMessage"] = $"La relación laboral de {empleado.Persona?.NombreCompleto} ya está activa.";
                return RedirectToAction(nameof(Details), new { id = empleado.IdEmpleado });
            }

            var model = new EmpleadoReactivateViewModel
            {
                IdEmpleado = empleado.IdEmpleado,
                NombreCompletoEmpleado = empleado.Persona?.NombreCompleto,
                FechaContratacion = empleado.FechaContratacion
            };
            return View(model);
        }

        // POST: Empleados/Reactivate/{id}
        [Authorize(Policy = "Administrador")]
        [HttpPost, ActionName("Reactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReactivateConfirmed(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var empleadoExistente = await _context.Empleados
                .Include(e => e.Persona)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);

            if (empleadoExistente == null)
            {
                return NotFound();
            }

            if (empleadoExistente.Estado == EstadoEmpleado.Activo)
            {
                TempData["WarningMessage"] = $"La relación laboral de {empleadoExistente.Persona?.NombreCompleto} ya está activa.";
                return RedirectToAction(nameof(Details), new { id = empleadoExistente.IdEmpleado });
            }

            // Guardar el puesto, salario y departamento actual antes de limpiar los campos de finalización
            var puestoActual = empleadoExistente.Puesto;
            var salarioActual = empleadoExistente.Salario;
            var departamentoActual = empleadoExistente.Departamento;

            // Cambiar el estado a Activo y limpiar campos de finalización
            empleadoExistente.Estado = EstadoEmpleado.Activo;
            empleadoExistente.FechaFinContrato = null;
            empleadoExistente.MotivoFinalizacion = null;
            empleadoExistente.TipoFinalizacion = null;

            // --- REGISTRAR EN TRAYECTORIAEMPLEADO (REACTIVACIÓN) ---
            var trayectoriaReactivacion = new TrayectoriaEmpleado
            {
                IdEmpleado = empleadoExistente.IdEmpleado,
                FechaCambio = DateTime.UtcNow, // La fecha y hora actual de reactivación
                TipoCambio = "Reactivación de Empleado",
                PuestoAnterior = puestoActual, // Puesto que tenía al momento de la inactivación
                PuestoNuevo = puestoActual, // No hay cambio de puesto, solo de estado
                SalarioAnterior = salarioActual, // Salario que tenía al momento de la inactivación
                SalarioNuevo = salarioActual, // No hay cambio de salario, solo de estado
                DepartamentoAnterior = departamentoActual, // Departamento que tenía al momento de la inactivación
                DepartamentoNuevo = departamentoActual, // No hay cambio de departamento, solo de estado
                MotivoCambio = "Empleado reactivado", // Motivo genérico para la reactivación
                RegistradoPor = User.Identity?.Name ?? "Sistema", // Quien realiza la acción
                FechaRegistro = DateTime.UtcNow
            };
            _context.TrayectoriasEmpleado.Add(trayectoriaReactivacion);
            // --- FIN REGISTRO ---

            try
            {
                _context.Update(empleadoExistente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Relación laboral de {empleadoExistente.Persona?.NombreCompleto} reactivada exitosamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(empleadoExistente.IdEmpleado))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Details), new { id = empleadoExistente.IdEmpleado });
        }

        // Método auxiliar para poblar la SelectList de personas no empleadas
        private async Task PopulatePersonasNoEmpleadas(int? selectedId = null)
        {
            var personasNoEmpleadas = await _context.Personas
                                                    .Where(p => p.Empleado == null || (selectedId.HasValue && p.IdPersona == selectedId.Value))
                                                    .OrderBy(p => p.Nombre)
                                                    .ThenBy(p => p.Apellidos)
                                                    .Select(p => new
                                                    {
                                                        IdPersona = p.IdPersona,
                                                        NombreCompleto = p.Nombre + " " + p.Apellidos + " (DUI: " + p.DUI + ")"
                                                    })
                                                    .ToListAsync();
            ViewData["IdPersona"] = new SelectList(personasNoEmpleadas, "IdPersona", "NombreCompleto", selectedId);

            if (!personasNoEmpleadas.Any() && !selectedId.HasValue)
            {
                ViewData["NoPersonsAvailable"] = "No hay personas disponibles para ser registradas como empleados.";
            }
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.IdEmpleado == id);
        }
    }
}