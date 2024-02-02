using Application.Common.Constants;
using Application.Common.Messaging;

namespace Application.Features.Categories.GetCategoriesTree;

public record GetCategoriesTreeQuery : ICachedQuery<GetCategoriesTreeResponse>
{
    public string Key => CachingKeys.Categories.CategoriesTree;
    public TimeSpan? Expiration => null;
    public bool IgnoreCaching => false;
}