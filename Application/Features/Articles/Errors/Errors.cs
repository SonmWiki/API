using ErrorOr;

namespace Application.Features.Articles.Errors;

public static class Article
{
    public static readonly Error NotFound = Error.NotFound(
        code: "Article.NotFound",
        description: "Article with this title was not found."
    );

    public static readonly Error DuplicateTitle = Error.Conflict(
        code: "Article.DuplicateTitle",
        description: "Article with this title already exists."
    );
}