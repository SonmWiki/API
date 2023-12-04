using Application;
using Application.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.SetupDatabase();

app.MapGet("/", () => "Hello World!");

app.Run();