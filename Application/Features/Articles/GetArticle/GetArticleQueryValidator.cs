using FluentValidation;

namespace Application.Features.Articles.GetArticle;

public class GetArticleQueryValidator: AbstractValidator<GetArticleQuery>
{
    public GetArticleQueryValidator()
    {
        When(v=> v.RevisionId.HasValue, () => {
            RuleFor(v => v.Id).Empty();
        }).Otherwise(() => {
            RuleFor(v => v.Id).NotEmpty();
        });
    }
}