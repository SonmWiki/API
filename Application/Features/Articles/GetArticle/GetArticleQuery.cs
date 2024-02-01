using Application.Common.Constants;
using Application.Common.Messaging;

namespace Application.Features.Articles.GetArticle;

public record GetArticleQuery(string Id, Guid? RevisionId = null) : ICachedQuery<GetArticleResponse>
{
    public string Key => CachingKeys.Articles.ArticleById(Id);
    public TimeSpan? Expiration => null;
}