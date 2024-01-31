namespace Application.Features.Categories.CreateCategory;

public class CategoryCreatedEvent
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? ParentId { get; init; }
}