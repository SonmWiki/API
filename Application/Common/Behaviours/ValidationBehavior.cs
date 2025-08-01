// using System.Reflection;
// using ErrorOr;
// using FluentValidation;
// using FluentValidation.Results;
// using MediatR;
// using ValidationException = Application.Common.Exceptions.ValidationException;
//
// namespace Application.Common.Behaviours;
//
// public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
//     where TRequest : IRequest<TResponse>
//     where TResponse : IErrorOr
// {
//     private readonly IValidator<TRequest>? _validator;
//
//     public ValidationBehavior(IValidator<TRequest>? validator = null)
//     {
//         _validator = validator;
//     }
//
//     public async Task<TResponse> Handle(
//         TRequest request,
//         RequestHandlerDelegate<TResponse> next,
//         CancellationToken token)
//     {
//         if (_validator == null) return await next();
//
//         var validationResult = await _validator.ValidateAsync(request, token);
//
//         if (validationResult.IsValid) return await next();
//
//         return TryCreateResponseFromErrors(validationResult.Errors, out var response)
//             ? response
//             : throw new ValidationException(validationResult.Errors);
//     }
//
//     private static bool TryCreateResponseFromErrors(List<ValidationFailure> validationFailures, out TResponse response)
//     {
//         var errors = validationFailures.ConvertAll(x => Error.Validation(
//             x.PropertyName,
//             x.ErrorMessage));
//
//         response = (TResponse?) typeof(TResponse)
//             .GetMethod(
//                 nameof(ErrorOr<object>.From),
//                 BindingFlags.Static | BindingFlags.Public,
//                 new[] {typeof(List<Error>)})?
//             .Invoke(null, new[] {errors})!;
//
//         return response is not null;
//     }
// }