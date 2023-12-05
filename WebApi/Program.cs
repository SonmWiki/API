using Application.Extensions;
using Application.Features.Articles.Extensions;
using Application.Features.Categories.Extensions;
using Infrastructure.Extensions;
using WebApi.Extensions;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
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

app.MapArticles();
app.MapCategories();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();