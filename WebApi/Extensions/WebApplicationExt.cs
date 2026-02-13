using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class WebApplicationExt
{
    extension(WebApplication app)
    {
        public async Task<WebApplication> SetupDatabase()
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception e)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "There was an error while creating or migrating database");
                throw;
            }

            return app;
        }
    }
}