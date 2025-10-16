using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AcaciRRHH.Web.Models;
using Microsoft.AspNetCore.Http;


namespace AcaciRRHH.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // DbSets para cada una de tus entidades
        public DbSet<Persona> Personas { get; set; } = default!;
        public DbSet<Empleado> Empleados { get; set; } = default!;
        public DbSet<Directivo> Directivos { get; set; }
        public DbSet<HistorialPuesto> HistorialPuestos { get; set; } = default!; 
        public DbSet<CapacitacionEmpleado> CapacitacionesEmpleados { get; set; } = default!;
        public DbSet<EvaluacionDesempeno> EvaluacionesDesempeno { get; set; } = default!;
        public DbSet<HistorialDisciplinario> HistorialDisciplinario { get; set; } = default!;
        public DbSet<HistorialDirectivo> HistorialDirectivo { get; set; }
        public DbSet<TrayectoriaEmpleado> TrayectoriasEmpleado { get; set; }

        public DbSet<NivelEducativo> NivelesEducativos { get; set; }
        public DbSet<Atestado> Atestados { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public DbSet<Incapacidad> Incapacidades { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Vacacion> Vacaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones de relaciones 1-1 y claves foráneas
            // Relación 1-1 entre Persona y Empleado (IdEmpleado es FK y PK)
            modelBuilder.Entity<Empleado>()
                .HasOne(e => e.Persona)
                .WithOne(p => p.Empleado)
                .HasForeignKey<Empleado>(e => e.IdPersona);

            // Relación 1-a-muchos entre Persona y Directivo
            modelBuilder.Entity<Persona>()
                .HasMany(p => p.Directivos)
                .WithOne(d => d.Persona)
                .HasForeignKey(d => d.IdPersona);

            // Para asegurarte de que `DUI` y `NIT` sean únicos en `Persona`
            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.DUI)
                .IsUnique();
            modelBuilder.Entity<Persona>()
                .HasIndex(p => p.NIT)
                .IsUnique();

            // Configura la relación entre Empleado y TrayectoriaEmpleado
            modelBuilder.Entity<Empleado>()
                .HasMany(e => e.HistorialTrayectoria)
                .WithOne(t => t.Empleado)
                .HasForeignKey(t => t.IdEmpleado)
                .OnDelete(DeleteBehavior.Cascade);

            // Borrado en cascada para Directivo y su historial
            modelBuilder.Entity<Directivo>()
                .HasMany(d => d.HistorialDirectivo)
                .WithOne(hd => hd.Directivo)
                .HasForeignKey(hd => hd.IdDirectivo)
                .OnDelete(DeleteBehavior.Cascade);

            // Para CargoDirectivoEnum
            modelBuilder.Entity<Directivo>()
                .Property(d => d.CargoDirectivo)
                .HasConversion<string>();

            // Para TipoMiembroOrganoEnum
            modelBuilder.Entity<Directivo>()
                .Property(d => d.TipoMiembro)
                .HasConversion<string>();

            modelBuilder.Entity<HistorialDirectivo>()
                .Property(hd => hd.Cargo)
                .HasDefaultValue("");

            modelBuilder.Entity<HistorialDirectivo>()
                .Property(hd => hd.TipoMandato)
                .HasDefaultValue("");

            modelBuilder.Entity<Persona>()
                .HasMany(p => p.NivelesEducativos)
                .WithOne(ne => ne.Persona)
                .HasForeignKey(ne => ne.IdPersona)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NivelEducativo>()
                .Property(ne => ne.GradoAcademico)
                .HasConversion<string>();

            // --- NUEVAS CONFIGURACIONES DE RELACIÓN: Persona con Atestado ---
            modelBuilder.Entity<Persona>()
                .HasMany(p => p.Atestados)
                .WithOne(a => a.Persona)
                .HasForeignKey(a => a.IdPersona)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Empleado>()
                .HasMany(e => e.HistorialDisciplinario)
                .WithOne(hd => hd.Empleado)
                .HasForeignKey(hd => hd.IdEmpleado)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Empleado>()
                .HasMany(e => e.EvaluacionesDesempeno)
                .WithOne(ed => ed.Empleado)
                .HasForeignKey(ed => ed.IdEmpleado)
                .OnDelete(DeleteBehavior.Cascade);

            // Remove the old Capacitaciones relationships
            // modelBuilder.Entity<Empleado>()
            //     .HasMany(e => e.Capacitaciones)
            //     .WithOne(c => c.Empleado)
            //     .HasForeignKey(c => c.IdEmpleado)
            //     .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<Directivo>()
            //     .HasMany(d => d.Capacitaciones)
            //     .WithOne(c => c.Directivo)
            //     .HasForeignKey(c => c.IdDirectivo)
            //     .OnDelete(DeleteBehavior.Cascade);

            // Add the new Capacitaciones relationship to Persona
            modelBuilder.Entity<Persona>()
                .HasMany(p => p.Capacitaciones) // Assuming Persona now has a Capacitaciones collection
                .WithOne(c => c.Persona)
                .HasForeignKey(c => c.IdPersona)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Empleado>()
                .HasMany(e => e.Incapacidades)
                .WithOne(i => i.Empleado)
                .HasForeignKey(i => i.IdEmpleado)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Empleado>()
                .HasMany(e => e.Permisos)
                .WithOne(p => p.Empleado)
                .HasForeignKey(p => p.IdEmpleado)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Empleado>()
                .HasMany(e => e.Vacaciones)
                .WithOne(v => v.Empleado)
                .HasForeignKey(v => v.IdEmpleado)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<AcaciRRHH.Web.Models.TrayectoriaEmpleado> TrayectoriaEmpleado { get; set; } = default!;

        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is IAuditable && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
            var currentTime = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                var entity = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = currentUsername;
                    entity.CreatedAt = currentTime;
                    entity.LastModifiedBy = currentUsername;
                    entity.LastModifiedAt = currentTime;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.LastModifiedBy = currentUsername;
                    entity.LastModifiedAt = currentTime;
                }
            }
        }
    }
}