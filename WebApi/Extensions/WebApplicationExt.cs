using Application.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class WebApplicationExt
{
    public static WebApplication SetupDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        try
        {
            dbContext.Database.EnsureCreated();
            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            logger.LogError(e, "There was an error while creating or migrating database");

            Console.WriteLine(e);
            throw;
        }

        return app;
    }
}