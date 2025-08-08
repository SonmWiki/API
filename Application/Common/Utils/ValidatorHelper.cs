using ErrorOr;
using FluentValidation;

namespace Application.Common.Utils;

public static class ValidatorHelper
{
    public static ErrorOr<Success> Validate<TRequest>(IValidator<TRequest> validator, TRequest request)
    {
        var validationResult = validator.Validate(request);

        return validationResult.IsValid
            ? Result.Success
            : validationResult.Errors.ConvertAll(e => Error.Validation(e.PropertyName, e.ErrorMessage));
    }
}