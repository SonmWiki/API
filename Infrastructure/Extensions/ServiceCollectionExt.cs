using System.Security.Claims;
using Application.Authorization.Abstractions;
using Application.Data;
using Domain.Entities;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExt
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddKeycloakAuthentication(configuration, options =>
        {
            options.Events ??= new JwtBearerEvents();

            options.Events.OnTokenValidated += async context =>
            {
                var principal = context.Principal;
                var id = principal?.FindFirst(ClaimTypes.NameIdentifier);
                if (id == null) return;
                if (principal?.Identity?.Name == null) return;
                if (principal.Identity.IsAuthenticated == false) return;

                var dbContext = context.HttpContext.RequestServices.GetService<IApplicationDbContext>();

                var author = new Author {Id = id.Value, Name = principal.Identity.Name};
                var exists = dbContext.Authors.Any(e => e.Id == id.Value);
                if (exists)
                    dbContext.Authors.Update(author);
                else
                    dbContext.Authors.Add(author);

                await dbContext.SaveChangesAsync();
            };
        });
        services.AddAuthorization();
        services.AddKeycloakAuthorization(configuration);

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}