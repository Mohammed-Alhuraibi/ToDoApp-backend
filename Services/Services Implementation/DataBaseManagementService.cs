using Data; // Corrected namespace
using Microsoft.EntityFrameworkCore;

namespace ToDoBackend.Services; // Corrected namespace
public class DataManagementService // Corrected class name typo
{
    public static void MigrationInitialization(IApplicationBuilder app) // Corrected method name typo
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>(); // Corrected DbContext usage
            if (context != null) // Added null check
            {
                context.Database.Migrate();
            }
            else
            {
                throw new Exception("Unable to get AppDbContext from service provider.");
            }
        }
    }
}
