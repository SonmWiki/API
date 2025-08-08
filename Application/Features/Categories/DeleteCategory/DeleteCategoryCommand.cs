using Application.Common.Messaging;

namespace Application.Features.Categories.DeleteCategory;

public record DeleteCategoryCommand(string Id) : ICommand<DeleteCategoryResponse>;