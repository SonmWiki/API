using ErrorOr;

namespace Application.Features.Authors.Errors;

public static class Author
{
    public static readonly Error NotFound = Error.NotFound(
        code: "Author.NotFound",
        description: "Author with this ID was not found."
    );

    public static readonly Error DuplicateId = Error.Conflict(
        code: "Author.DuplicateId",
        description: "Author with this ID already exists."
    );
}