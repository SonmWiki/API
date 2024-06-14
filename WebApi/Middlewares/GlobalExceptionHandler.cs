using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        IResult result;
        if (exception is BadHttpRequestException)
        {
            result = Results.Problem(
                "Unexpected request content",
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad request"
            );
        }
        else
        {
            logger.LogError(exception, exception.Message);
            
            result = Results.Problem(
                "An internal server error has occured",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Server error"
            );
        }
        await result.ExecuteAsync(httpContext);

        return true;
    }
}