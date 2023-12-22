using FluentValidation;

namespace Application.Features.Categories.EditCategory;

public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
{
    public EditCategoryCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(128)
            .NotEmpty();
    }
}