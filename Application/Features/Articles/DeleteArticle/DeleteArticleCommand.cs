using ErrorOr;
using MediatR;

namespace Application.Features.Articles.DeleteArticle;

public record DeleteArticleCommand(string Id) : IRequest<ErrorOr<DeleteArticleResponse>>;