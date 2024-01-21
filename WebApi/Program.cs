using Application.Extensions;
using Infrastructure.Extensions;
using Keycloak.AuthServices.Authentication;
using WebApi.Extensions;
using WebApi.Features.Articles;
using WebApi.Features.Categories;
using WebApi.Features.Navigations;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureKeycloakConfigurationSource();

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwagger(builder.Configuration);

var app = builder.Build();

app.SetupDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.AddArticlesEndpoints();
app.AddCategoriesEndpoints();
app.AddNavigationsEndpoints();

if (app.Environment.IsDevelopment())
{
    var keycloakOptions = new KeycloakAuthenticationOptions();
    app.Configuration.GetSection(KeycloakAuthenticationOptions.Section)
        .Bind(keycloakOptions, opt => opt.BindNonPublicProperties = true);

    app.UseSwagger();
    app.UseSwaggerUI(options => options.OAuthClientId(keycloakOptions.Resource));
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();