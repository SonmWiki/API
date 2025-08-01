using System.Security.Claims;
using Application.Authorization.Abstractions;
using Application.Common.Caching;
using Application.Data;
using Application.Features.Authors.CreateAuthor;
using Application.Features.Authors.EditAuthor;
using Infrastructure.Authorization;
using Infrastructure.Caching;
using Infrastructure.Data;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Application.Features.Authors.Errors.Author;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExt
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    )
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddKeycloakWebApiAuthentication(configuration, options =>
        {
            if (isDevelopment) options.RequireHttpsMetadata = false;
            options.Events ??= new JwtBearerEvents();
            options.Events.OnTokenValidated += CreateOrUpdateAuthorOnTokenValidated;
        });
        services.AddAuthorization();
        services.AddKeycloakAuthorization(configuration);

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }

    private static async Task CreateOrUpdateAuthorOnTokenValidated(TokenValidatedContext context)
    {
        var principal = context.Principal;
        var idClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
        if (idClaim == null) return;
        if (principal?.Identity?.Name == null) return;
        if (principal.Identity.IsAuthenticated == false) return;

        //TODO: Reimplement without MediatR

        // var mediator = context.HttpContext.RequestServices.GetService<IMediator>();
        // if (mediator == null) return;
        //
        // var createAuthorCommand = new CreateAuthorCommand(idClaim.Value, principal.Identity.Name);
        //
        // var createAuthorResult = await mediator.Send(createAuthorCommand);
        // if (createAuthorResult.IsError && createAuthorResult.FirstError == DuplicateId)
        // {
        //     var editAuthorCommand = new EditAuthorCommand(idClaim.Value, principal.Identity.Name);
        //     await mediator.Send(editAuthorCommand);
        // }
    }
}