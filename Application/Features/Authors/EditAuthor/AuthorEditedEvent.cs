using MediatR;

namespace Application.Features.Authors.EditAuthor;

public class AuthorEditedEvent : INotification
{
    public required string Id { get; init; }
    public required string Name { get; init; }
}