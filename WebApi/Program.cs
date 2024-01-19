using Application.Extensions;
using Infrastructure.Extensions;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;
using WebApi.Features.Articles;
using WebApi.Features.Categories;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureKeycloakConfigurationSource();

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.CustomSchemaIds(x => x.FullName.Replace("+", ".").Replace(x.Namespace+".", ""));
        options.SupportNonNullableReferenceTypes();
        KeycloakAuthenticationOptions kcoptions = new();

        builder.Configuration
            .GetSection(KeycloakAuthenticationOptions.Section)
            .Bind(kcoptions, opt => opt.BindNonPublicProperties = true);
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
                    AuthorizationUrl = new Uri($"{kcoptions.KeycloakUrlRealm}/protocol/openid-connect/auth"),
                    TokenUrl = new Uri($"{kcoptions.KeycloakUrlRealm}/protocol/openid-connect/token"),
                    Scopes = new Dictionary<string, string>()
                },
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri($"{kcoptions.KeycloakUrlRealm}/protocol/openid-connect/auth"),
                    TokenUrl = new Uri($"{kcoptions.KeycloakUrlRealm}/protocol/openid-connect/token"),
                    Scopes = new Dictionary<string, string>()
                }
            }
        };
        options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {securityScheme, Array.Empty<string>()}
        });
    }
);

var app = builder.Build();

app.SetupDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.AddArticlesEndpoints();
app.AddCategoriesEndpoints();

if (app.Environment.IsDevelopment())
{
    KeycloakAuthenticationOptions kcoptions = new();

    app.Configuration
        .GetSection(KeycloakAuthenticationOptions.Section)
        .Bind(kcoptions, opt => opt.BindNonPublicProperties = true);
    
    app.UseSwagger();
    app.UseSwaggerUI(options => options.OAuthClientId(kcoptions.Resource));
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();