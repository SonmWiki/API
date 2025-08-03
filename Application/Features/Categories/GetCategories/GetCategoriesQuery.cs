using Application.Common.Constants;
using Application.Common.Messaging;
using ErrorOr;

namespace Application.Features.Categories.GetCategories;

public record GetCategoriesQuery : ICachedQuery<ErrorOr<GetCategoriesResponse>>
{
    public string Key => CachingKeys.Categories.CategoriesAll;
    public TimeSpan? Expiration => null;
    public bool IgnoreCaching => false;
}