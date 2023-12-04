using Application;
using Application.Data;
using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.SetupDatabase();

app.MapGet("/", () => "Hello World!");

app.Run();