using Application.Common.Caching;
using Application.Common.Constants;
using Application.Common.Messaging;
using Application.Common.Utils;
using Application.Data;
using Domain.Entities;
using ErrorOr;
using FluentValidation;

namespace Application.Features.Navigations.UpdateNavigationsTree;

public class UpdateNavigationsTreeCommandHandler(
    IApplicationDbContext dbContext,
    ICacheService cacheService,
    IValidator<UpdateNavigationsTreeCommand> validator
) : ICommandHandler<UpdateNavigationsTreeCommand, UpdateNavigationsTreeResponse>
{
    public async Task<ErrorOr<UpdateNavigationsTreeResponse>> HandleAsync(UpdateNavigationsTreeCommand request,
        CancellationToken token)
    {
        var validationResult = ValidatorHelper.Validate(validator, request);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        foreach (var item in dbContext.Navigations)
            dbContext.Navigations.Remove(item);

        var navigationsToAdd = new List<Navigation>();

        var stack = new Stack<Tuple<Navigation?, int, UpdateNavigationsTreeCommand.Element>>();
        for (var i = 0; i < request.Data.Count; i++)
            stack.Push(new Tuple<Navigation?, int, UpdateNavigationsTreeCommand.Element>(null, request.Data.Count - i,
                request.Data[i]));

        while (stack.Count > 0)
        {
            var (parent, weight, requestElement) = stack.Pop();

            var navigation = new Navigation
            {
                Id = default!,
                Name = requestElement.Name,
                Weight = weight,
                Uri = requestElement.Uri,
                Parent = parent,
                Icon = requestElement.Icon
            };

            navigationsToAdd.Add(navigation);

            for (var i = 0; i < requestElement.Children.Count; i++)
                stack.Push(new Tuple<Navigation?, int, UpdateNavigationsTreeCommand.Element>
                    (navigation, requestElement.Children.Count - i, requestElement.Children[i]));
        }

        await dbContext.Navigations.AddRangeAsync(navigationsToAdd, token);
        await dbContext.SaveChangesAsync(token);

        await cacheService.RemoveAsync(CachingKeys.Navigation.NavigationsTree, token);

        return new UpdateNavigationsTreeResponse();
    }
}