namespace Application.Features.Categories.EditCategory;

public class CategoryEditedEvent
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? ParentId { get; init; }
}