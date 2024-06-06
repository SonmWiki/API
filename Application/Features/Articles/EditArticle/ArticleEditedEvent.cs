using MediatR;

namespace Application.Features.Articles.EditArticle;

public class ArticleEditedEvent : INotification
{
    public required string Id { get; init; }
    public required string Content { get; init; }
    public required string AuthorsNote { get; init; }
    public required List<string> CategoryIds { get; init; }
}