using Application.Common.Messaging;

namespace Application.Features.Articles.CreateArticle;

public record CreateArticleCommand(
    string Title,
    string Content,
    string AuthorsNote,
    List<string> CategoryIds
) : ICommand<CreateArticleResponse>;