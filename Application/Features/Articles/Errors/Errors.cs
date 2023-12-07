using ErrorOr;

namespace Application.Features.Articles.Errors;

public static class Article
{
    public static readonly Error NotFound = Error.NotFound(
        code: "Article.NotFound",
        description: "Article with this ID was not found."
    );

    public static readonly Error DuplicateId = Error.Conflict(
        code: "Article.DuplicateId",
        description: "Article with similar title that results in same ID already exists."
    );

    public static readonly Error EmptyId = Error.Validation(
        code: "Article.EmptyId",
        description: "Generated ID for this article is empty."
    );
}