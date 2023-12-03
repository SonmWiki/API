using Application;
using Application.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        dbContext.Database.EnsureCreated();
        await dbContext.Database.MigrateAsync();
    }
    catch (Exception e)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogError(e, "There was an error while creating or migrating database");
        
        Console.WriteLine(e);
        throw;
    }
}

app.MapGet("/", () => "Hello World!");

app.Run();