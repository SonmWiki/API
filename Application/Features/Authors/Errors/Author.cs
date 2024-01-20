using ErrorOr;

namespace Application.Features.Authors.Errors;

public static class Author
{
    public static readonly Error NotFound = Error.NotFound(
        "Author.NotFound",
        "Author with this ID was not found."
    );

    public static readonly Error DuplicateId = Error.Conflict(
        "Author.DuplicateId",
        "Author with this ID already exists."
    );
}