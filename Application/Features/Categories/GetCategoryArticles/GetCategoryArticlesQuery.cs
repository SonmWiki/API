using Application.Common.Constants;
using Application.Common.Messaging;

namespace Application.Features.Categories.GetCategoryArticles;

public record GetCategoryArticlesQuery(string Id) : ICachedQuery<GetCategoryArticlesResponse>
{
    public string Key => CachingKeys.Categories.CategoryArticlesById(Id);
    public TimeSpan? Expiration => TimeSpan.FromHours(12);
    public bool IgnoreCaching => false;
};