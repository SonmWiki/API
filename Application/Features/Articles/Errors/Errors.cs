using ErrorOr;

namespace Application.Features.Articles.Errors;

public static class Article
{
    public static readonly Error NotFound = Error.NotFound(
        "Article.NotFound",
        "Article with this ID was not found."
    );

    public static readonly Error DuplicateId = Error.Conflict(
        "Article.DuplicateId",
        "Article with similar title that results in same ID already exists."
    );

    public static readonly Error EmptyId = Error.Validation(
        "Article.EmptyId",
        "Generated ID for this article is empty."
    );

    public static readonly Error RedirectExists = Error.Conflict(
        "Article.RedirectAlreadySet",
        "Redirect for this article has already been set."
    );
    
    public static readonly Error UnfitRedirectTarget = Error.Conflict(
        "Article.UnfitRedirectTarget",
        "Article with a redirect set cannot be set as a redirect target."
    );
}

public static class Revision
{
    public static readonly Error NotFound = Error.NotFound(
        "Revision.NotFound",
        "Revision with this ID was not found."
    );
}