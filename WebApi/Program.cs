using Application.Extensions;
using Application.Features.Articles.Extensions;
using Application.Features.Categories.Extensions;
using Infrastructure.Extensions;
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

app.MapArticles();
app.MapCategories();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();