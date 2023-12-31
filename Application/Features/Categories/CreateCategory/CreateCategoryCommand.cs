using ErrorOr;
using MediatR;

namespace Application.Features.Categories.CreateCategory;

public record CreateCategoryCommand(string Name, string? ParentId) : IRequest<ErrorOr<CreateCategoryResponse>>;