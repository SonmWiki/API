using Application.Common.Constants;
using Application.Common.Messaging;
using ErrorOr;

namespace Application.Features.Categories.GetCategoryArticles;

public record GetCategoryArticlesQuery(string Id) : ICachedQuery<ErrorOr<GetCategoryArticlesResponse>>
{
    public string Key => CachingKeys.Categories.CategoryArticlesById(Id);
    public TimeSpan? Expiration => TimeSpan.FromHours(12);
    public bool IgnoreCaching => false;
};