using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using WebApi.Auth;
using WebApi.SchemaFilters;
using WebApi.Settings;

namespace WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    internal static void RegisterSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(x => x.FullName?.Replace("+", ".").Replace(x.Namespace + ".", ""));
            options.SupportNonNullableReferenceTypes();
            options.SchemaFilter<SwaggerRequiredSchemaFilter>();
            var jwtSettings = configuration.GetRequiredSection(JwtSettings.SectionName).Get<JwtSettings>()
                              ?? throw new InvalidOperationException($"Missing {JwtSettings.SectionName} section");

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Auth",
                Type = SecuritySchemeType.OAuth2,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows
                {
                    //TODO awful hack .Replace("host.docker.internal", "localhost") for ease of testing
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{jwtSettings.Authority.Replace("host.docker.internal", "localhost")}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{jwtSettings.Authority.Replace("host.docker.internal", "localhost")}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>()
                    },
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{jwtSettings.Authority.Replace("host.docker.internal", "localhost")}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{jwtSettings.Authority.Replace("host.docker.internal", "localhost")}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>()
                    }
                }
            };
            options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            });
        });
    }

    internal static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
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
}