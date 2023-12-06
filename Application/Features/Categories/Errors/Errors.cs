using ErrorOr;

namespace Application.Features.Categories.Errors;

public static class Category
{
    public static Error NotFound = Error.NotFound(
        code: "Category.NotFound",
        description: "Category with this name was not found."
    );

    public static Error ParentNotFound = Error.NotFound(
        code: "Category.ParentNotFound",
        description: "Category with this name was not found."
    );

    public static Error DuplicateName = Error.Conflict(
        code: "Category.DuplicateName",
        description: "Category with this name already exists."
    );
}