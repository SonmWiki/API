using Application.Common.Messaging;

namespace Application.Features.Articles.EditArticle;

public record EditArticleCommand(
    string Id,
    string Content,
    string AuthorsNote,
    List<string> CategoryIds
): ICommand<EditArticleResponse>;