using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Articles.CreateArticle;

public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(v => v.Title)
            .MaximumLength(ApplicationConstants.MaxTitleLenght)
            .NotEmpty();
        RuleFor(v => v.Content)
            .NotEmpty();
    }
}