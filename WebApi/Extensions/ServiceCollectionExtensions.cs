using Application.Authorization.Abstractions;
using Domain.Rbac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using WebApi.Auth;
using WebApi.Settings;

namespace WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        internal void ConfigureCors(IConfiguration configuration)
        {
            var corsSettings = configuration.GetRequiredSection(CorsSettings.SectionName).Get<CorsSettings>()
                               ?? throw new InvalidOperationException($"Missing {CorsSettings.SectionName} section");

            if (corsSettings.Enabled)
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy
                            .WithOrigins(corsSettings.AllowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                });
            }
        }

        internal void ConfigureAuth(IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration.GetRequiredSection(JwtSettings.SectionName).Get<JwtSettings>()
                                      ?? throw new InvalidOperationException($"Missing {JwtSettings.SectionName} section");

                    options.Authority = jwtSettings.Authority;
                    options.Audience = jwtSettings.Audience;
                    options.RequireHttpsMetadata = jwtSettings.RequireHttpsMetadata;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = jwtSettings.ValidateIssuer,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = jwtSettings.ValidateAudience,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = jwtSettings.ValidateLifetime,
                        ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                        ClockSkew = jwtSettings.ClockSkew,
                    };

                    options.Events = options.ConfigureJwtBearerEvents();
                });

            services.AddAuthorization(options =>
            {
                foreach (var permission in Permissions.All)
                {
                    options.AddPolicy($"Permission_{permission.Id:D}",
                        policy => policy.Requirements.Add(new PermissionRequirement(permission)));
                }
            });

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped<IUserContext, UserContext>();
        }
    }
}