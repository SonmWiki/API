using System.Text.Json.Serialization;
using Application.Extensions;
using Infrastructure.Extensions;
using WebApi.Extensions;
using WebApi.Features.Articles;
using WebApi.Features.Categories;
using WebApi.Features.Navigations;
using WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using WebApi.Features.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.ConfigureCors(builder.Configuration);
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

builder.Services.AddOpenApi(config =>
{
    config.CustomSchemaIds(x => x.FullName?.Replace("+", ".").Replace(x.Namespace + ".", ""));
    config.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new OpenApiInfo
        {
            Title = "SomniumWiki API | v1",
            Version = "v1",
        };
        return Task.CompletedTask;
    });
});

builder.Services.ConfigureAuth(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors();

await app.SetupDatabase();

app.UseAuthentication();
app.UseAuthorization();

app.AddArticlesEndpoints();
app.AddCategoriesEndpoints();
app.AddNavigationsEndpoints();
app.AddUsersEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference();
}

app.Run();