using ErrorOr;
using Microsoft.AspNetCore.Http;

namespace Application.Extensions;

public static class ErrorExt
{
    public static IResult ToIResult(this Error error)
    {
        return error.Type switch
        {
            ErrorType.Validation => Results.Problem(error.Description, statusCode: StatusCodes.Status400BadRequest, title: error.Code),
            ErrorType.Conflict => Results.Problem(error.Description, statusCode: StatusCodes.Status409Conflict, title: error.Code),
            ErrorType.NotFound => Results.Problem(error.Description, statusCode: StatusCodes.Status404NotFound, title: error.Code),
            ErrorType.Unauthorized => Results.Problem(error.Description, statusCode: StatusCodes.Status401Unauthorized, title: error.Code),
            _ => Results.Problem(error.Description, title: error.Code)
        };
    }
}