namespace WebApi.Features.Categories.Requests;

public record UpdateCategoryRequest(string Name, string? ParentId);