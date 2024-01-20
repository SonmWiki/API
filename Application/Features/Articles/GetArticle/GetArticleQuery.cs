using ErrorOr;
using MediatR;

namespace Application.Features.Articles.GetArticle;

public record GetArticleQuery(string Id, Guid? RevisionId = null) : IRequest<ErrorOr<GetArticleResponse>>;