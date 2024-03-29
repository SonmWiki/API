﻿using System.Security.Claims;
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
using MediatR;
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
                var idClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
                if (idClaim == null) return;
                if (principal?.Identity?.Name == null) return;
                if (principal.Identity.IsAuthenticated == false) return;

                var mediator = context.HttpContext.RequestServices.GetService<IMediator>();
                var createCommand = new CreateAuthorCommand(idClaim.Value, principal.Identity.Name);

                var result = await mediator.Send(createCommand);
                if (result.IsError)
                {
                    var updateCommand = new EditAuthorCommand(idClaim.Value, principal.Identity.Name);
                    await mediator.Send(updateCommand);
                }
            };
        });
        services.AddAuthorization();
        services.AddKeycloakAuthorization(configuration);

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}