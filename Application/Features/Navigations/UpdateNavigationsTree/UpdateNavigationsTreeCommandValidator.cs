using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Navigations.UpdateNavigationsTree;

public class UpdateNavigationsTreeCommandValidator : AbstractValidator<UpdateNavigationsTreeCommand>
{
    public UpdateNavigationsTreeCommandValidator()
    {
        RuleForEach(v => v.Data).SetValidator(new UpdateNavigationsTreeCommandElementValidator());
    }
}

public class UpdateNavigationsTreeCommandElementValidator : AbstractValidator<UpdateNavigationsTreeCommand.Element>
{
    public UpdateNavigationsTreeCommandElementValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .MaximumLength(ApplicationConstants.MaxTitleLenght);
        RuleFor(v => v.Icon)
            .MaximumLength(ApplicationConstants.MaxTitleLenght);
        RuleFor(v => v.Uri)
            .MaximumLength(ApplicationConstants.MaxUriLength);
        RuleFor(v => v.Children)
            .ForEach(v => v.SetValidator(this));
    }
}