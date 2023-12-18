using ErrorOr;
using MediatR;

namespace Application.Features.Categories.UpdateCategory;

public record UpdateCategoryCommand
    (string Id, string Name, string? ParentId) : IRequest<ErrorOr<UpdateCategoryResponse>>;