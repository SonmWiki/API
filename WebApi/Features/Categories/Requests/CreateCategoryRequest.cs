namespace WebApi.Features.Categories.Requests;

public record CreateCategoryRequest(string Name, string? ParentId);