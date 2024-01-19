using FluentValidation;

namespace Application.Features.Articles.ReviewRevision;

public class ReviewRevisionValidator : AbstractValidator<ReviewRevisionCommand>
{
    public ReviewRevisionValidator()
    {
        RuleFor(v => v.RevisionId)
            .NotEmpty();
        RuleFor(v => v.Review).NotEmpty();
    }
}