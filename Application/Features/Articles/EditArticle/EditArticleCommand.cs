using ErrorOr;
using MediatR;

namespace Application.Features.Articles.EditArticle;

public record EditArticleCommand(
    string Id,
    string Content,
    List<string> CategoryIds
) : IRequest<ErrorOr<EditArticleResponse>>;