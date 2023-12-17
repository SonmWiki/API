using Application.Extensions;
using Application.Features.Articles.Extensions;
using Application.Features.Categories.Extensions;
using Infrastructure.Extensions;
using Keycloak.AuthServices.Authentication;
using WebApi.Extensions;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureKeycloakConfigurationSource("keycloak.json");

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SupportNonNullableReferenceTypes();
        options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    }
);

var app = builder.Build();

app.SetupDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.MapArticles();
app.MapCategories();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();