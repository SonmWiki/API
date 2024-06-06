using ErrorOr;
using MediatR;

namespace Application.Features.Articles.EditArticle;

public record EditArticleCommand(
    string Id,
    string Content,
    string AuthorsNote,
    List<string> CategoryIds
) : IRequest<ErrorOr<EditArticleResponse>>;