using ErrorOr;

namespace Application.Features.Users.Errors;

public static class User
{
    public static readonly Error NotFound = Error.NotFound(
        "User.NotFound",
        "User with this ID was not found."
    );

    public static readonly Error DuplicateExternalId = Error.Conflict(
        "User.DuplicateId",
        "User with this external ID already exists."
    );
}