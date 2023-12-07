using ErrorOr;

namespace Application.Features.Categories.Errors;

public static class Category
{
    public static Error NotFound = Error.NotFound(
        code: "Category.NotFound",
        description: "Category with this ID was not found."
    );

    public static Error ParentNotFound = Error.NotFound(
        code: "Category.ParentNotFound",
        description: "Category with this parent Id was not found."
    );

    public static Error DuplicateId = Error.Conflict(
        code: "Category.DuplicateId",
        description: "Category with similar name that results in same ID already exists."
    );
    
    public static readonly Error EmptyId = Error.Validation(
        code: "Category.EmptyId",
        description: "Generated ID for this category is empty."
    );
}