using ErrorOr;
using MediatR;

namespace Application.Features.Articles.CreateArticle;

public record CreateArticleCommand(
    string Title,
    string Content,
    List<string> CategoryIds
) : IRequest<ErrorOr<CreateArticleResponse>>;