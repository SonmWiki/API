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
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Children).ForEach(v=>v.SetValidator(this));
    }
}