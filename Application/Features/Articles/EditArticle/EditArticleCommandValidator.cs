using FluentValidation;

namespace Application.Features.Articles.EditArticle;

public class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
{
    public EditArticleCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty();
        RuleFor(v => v.Content)
            .NotEmpty();
    }
}