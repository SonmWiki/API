using FluentValidation;

namespace Application.Features.Articles.SetRedirect;

public class SetRedirectValidator : AbstractValidator<SetRedirectCommand>
{
    public SetRedirectValidator()
    {
        RuleFor(v => v.ArticleId)
            .NotEmpty();
        RuleFor(v => v.RedirectId)
            .NotEmpty()
            .NotEqual(v=>v.ArticleId);
    }
}