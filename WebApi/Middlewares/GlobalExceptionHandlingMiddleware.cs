using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares;

public class GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);

            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var problem = new ProblemDetails
            {
                Status = (int) HttpStatusCode.InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error has occured"
            };

            var json = JsonSerializer.Serialize(problem);
            
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsync(json);
        }
    }
}