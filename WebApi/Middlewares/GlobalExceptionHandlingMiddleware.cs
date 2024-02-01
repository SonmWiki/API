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
            
            ProblemDetails problem;
            
            if (e is BadHttpRequestException)
            {
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                
                problem = new ProblemDetails
                {
                    Status = (int) HttpStatusCode.BadRequest,
                    Type = "Bad request",
                    Title = "Bad request",
                    Detail = "Unexpected request content"
                };
            }
            else
            {
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                
                problem = new ProblemDetails
                {
                    Status = (int) HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Server error",
                    Detail = "An internal server error has occured"
                };
            }
            
            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(problem);
            await context.Response.WriteAsync(json);
        }
    }
}