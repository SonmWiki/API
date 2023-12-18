using ErrorOr;
using MediatR;

namespace Application.Features.Articles.GetArticle;

public record GetArticleQuery(string Id) : IRequest<ErrorOr<GetArticleResponse>>;