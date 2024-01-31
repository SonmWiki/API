using Application.Common.Messaging;

namespace Application.Features.Articles.GetArticle;

public record GetArticleQuery(string Id, Guid? RevisionId = null) : IQuery<GetArticleResponse>;