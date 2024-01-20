using ErrorOr;

namespace Application.Features.Categories.Errors;

public static class Category
{
    public static readonly Error NotFound = Error.NotFound(
        "Category.NotFound",
        "Category with this ID was not found."
    );

    public static readonly Error ParentNotFound = Error.NotFound(
        "Category.ParentNotFound",
        "Category with this parent Id was not found."
    );

    public static readonly Error DuplicateId = Error.Conflict(
        "Category.DuplicateId",
        "Category with similar name that results in same ID already exists."
    );

    public static readonly Error EmptyId = Error.Validation(
        "Category.EmptyId",
        "Generated ID for this category is empty."
    );
}