using System.Text.Json.Serialization;
using Application.Authorization.Abstractions;
using Application.Extensions;
using Domain.Rbac;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WebApi.Extensions;
using WebApi.Features.Articles;
using WebApi.Features.Categories;
using WebApi.Features.Navigations;
using WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Auth;
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
if (builder.Environment.IsDevelopment())
{
    builder.Services.RegisterSwagger(builder.Configuration);
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetRequiredSection(JwtSettings.SectionName).Get<JwtSettings>()
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
builder.Services.AddAuthorization(options =>
{
    foreach (var permission in Permissions.All)
    {
        options.AddPolicy($"Permission_{permission.Id:D}",
            policy => policy.Requirements.Add(new PermissionRequirement(permission)));
    }
});
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Services.AddScoped<IUserContext, UserContext>();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();