using MediatR;

namespace Application.Features.Articles.DeleteArticle;

public class ArticleDeletedEvent : INotification
{
    public required string Id { get; init; }
}