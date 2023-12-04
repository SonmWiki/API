using Application;
using Application.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SupportNonNullableReferenceTypes();
        options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
    }
);

//TODO Error pipeline behaviour i.e. NotFoundException, ConflictException

var app = builder.Build();

app.SetupDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.Run();