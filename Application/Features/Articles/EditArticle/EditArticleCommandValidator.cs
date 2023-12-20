using FluentValidation;

namespace Application.Features.Articles.EditArticle;

public class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
{
    public EditArticleCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
        RuleFor(v => v.Title)
            .MaximumLength(128)
            .NotEmpty();
        RuleFor(v => v.Content)
            .NotEmpty();
    }
}