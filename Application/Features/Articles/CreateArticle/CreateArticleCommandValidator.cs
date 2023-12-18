using FluentValidation;

namespace Application.Features.Articles.CreateArticle;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(128)
            .NotEmpty();
        RuleFor(v => v.Content)
            .NotEmpty();
    }
}