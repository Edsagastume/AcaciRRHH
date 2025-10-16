using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using AcaciRRHH.Web.Data;
using System.IO; 
using System.Reflection; // Necesario para Assembly
using Microsoft.AspNetCore.Http; // Necesario para IHttpContextAccessor

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Obtener el directorio de donde se está ejecutando el ensamblado de la fábrica (tu proyecto web)
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (string.IsNullOrEmpty(basePath))
        {
            // Fallback: usar el directorio actual si el Location no es claro (menos fiable)
            basePath = Directory.GetCurrentDirectory();
        }

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath) // Usar el basePath determinado
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // optional: false para que falle si no lo encuentra
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("No se encontró la cadena de conexión 'DefaultConnection' en appsettings.json.");
        }

        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseNpgsql(connectionString);

        // Pass null for IHttpContextAccessor in design-time context
        return new ApplicationDbContext(builder.Options, null!); 
    }
}