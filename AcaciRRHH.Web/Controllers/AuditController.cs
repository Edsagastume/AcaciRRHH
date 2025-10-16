using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using AcaciRRHH.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace AcaciRRHH.Web.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class AuditController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var auditLogs = new List<AuditLogEntryViewModel>();

            //obtienemos todas las entidades que implementan IAuditable
            var auditableEntityTypes = _context.Model.GetEntityTypes()
                .Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType));

            foreach (var entityType in auditableEntityTypes)
            {
                //Usamos reflection para obtener el DbSet correspondiente a la entidad
                var entities = await GetAuditableEntities(entityType.ClrType);

                foreach (var entity in entities)
                {
                    //Creamos una entrada de log para la creación de la entidad
                    auditLogs.Add(new AuditLogEntryViewModel
                    {
                        EntityType = entityType.DisplayName(),
                        EntityId = entityType.FindPrimaryKey()?.Properties.Select(p => p.PropertyInfo?.GetValue(entity)?.ToString()).FirstOrDefault() ?? "N/A",
                        Action = "Created",
                        UserName = entity.CreatedBy ?? "N/A",
                        Timestamp = entity.CreatedAt
                    });

                    //Modificación de la entidad (si aplica)
                    if (entity.LastModifiedAt != entity.CreatedAt)
                    {
                        auditLogs.Add(new AuditLogEntryViewModel
                        {
                            EntityType = entityType.DisplayName(),
                            EntityId = entityType.FindPrimaryKey()?.Properties.Select(p => p.PropertyInfo?.GetValue(entity)?.ToString()).FirstOrDefault() ?? "N/A",
                            Action = "Modified",
                            UserName = entity.LastModifiedBy ?? "N/A",
                            Timestamp = entity.LastModifiedAt
                        });
                    }
                }
            }

            //Ordenamos los logs por fecha descendente
            auditLogs = auditLogs.OrderByDescending(log => log.Timestamp).ToList();

            return View(auditLogs);
        }

        private async Task<List<IAuditable>> GetAuditableEntities(Type entityType)
        {
            // Usamos reflection para llamar a _context.Set<entityType>()
            var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes)?.MakeGenericMethod(entityType);

            if (setMethod == null)
            {
                // El manejador del caso donde no se encuentra el método (debería suceder raramente)
                return new List<IAuditable>();
            }

            var dbSet = setMethod.Invoke(_context, null);

            if (dbSet == null)
            {
                // El manejador del caso donde no se encuentra el DbSet (debería suceder raramente)
                return new List<IAuditable>();
            }

            // Convertimos el DbSet a IQueryable<IAuditable> y obtenemos la lista de entidades auditable
            return await ((IQueryable<IAuditable>)dbSet).ToListAsync();
        }
    }
}