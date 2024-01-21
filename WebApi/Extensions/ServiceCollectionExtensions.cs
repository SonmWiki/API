using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    internal static void RegisterSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(x => x.FullName?.Replace("+", ".").Replace(x.Namespace + ".", ""));
            options.SupportNonNullableReferenceTypes();
            var keycloakOptions = new KeycloakAuthenticationOptions();

            configuration.GetSection(KeycloakAuthenticationOptions.Section).Bind(keycloakOptions, opt => opt.BindNonPublicProperties = true);
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
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{keycloakOptions.KeycloakUrlRealm}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{keycloakOptions.KeycloakUrlRealm}/protocol/openid-connect/token"),
                        Scopes = new Dictionary<string, string>()
                    },
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{keycloakOptions.KeycloakUrlRealm}/protocol/openid-connect/auth"),
                        TokenUrl = new Uri($"{keycloakOptions.KeycloakUrlRealm}/protocol/openid-connect/token"),
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
}