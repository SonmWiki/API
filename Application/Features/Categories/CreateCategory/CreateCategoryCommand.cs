using Application.Common.Messaging;

namespace Application.Features.Categories.CreateCategory;

public record CreateCategoryCommand(string Name, string? ParentId) : ICommand<CreateCategoryResponse>;