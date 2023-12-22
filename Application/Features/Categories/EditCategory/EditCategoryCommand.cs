using ErrorOr;
using MediatR;

namespace Application.Features.Categories.EditCategory;

public record EditCategoryCommand(string Id, string Name, string? ParentId) : IRequest<ErrorOr<EditCategoryResponse>>;