using Application.Common.Constants;
using Application.Common.Messaging;

namespace Application.Features.Categories.GetCategories;

public record GetCategoriesQuery : ICachedQuery<GetCategoriesResponse>
{
    public string Key => CachingKeys.Categories.CategoriesAll;
    public TimeSpan? Expiration => null;
}