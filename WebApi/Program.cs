using System.Text.Json.Serialization;
using Application.Extensions;
using Infrastructure.Extensions;
using Keycloak.AuthServices.Authentication;
using WebApi.Extensions;
using WebApi.Features.Articles;
using WebApi.Features.Categories;
using WebApi.Features.Navigations;
using WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.IsDevelopment());
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
if (builder.Environment.IsDevelopment())
{
    builder.Services.RegisterSwagger(builder.Configuration);
}

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors(policyBuilder => policyBuilder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins(app.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
);

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

app.Run();