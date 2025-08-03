using Application.Common.Constants;
using Application.Common.Messaging;
using ErrorOr;

namespace Application.Features.Categories.GetCategoriesTree;

public record GetCategoriesTreeQuery : ICachedQuery<ErrorOr<GetCategoriesTreeResponse>>
{
    public string Key => CachingKeys.Categories.CategoriesTree;
    public TimeSpan? Expiration => null;
    public bool IgnoreCaching => false;
}