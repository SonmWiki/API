using ErrorOr;
using MediatR;

namespace Application.Features.Categories.DeleteCategory;

public record DeleteCategoryCommand(string Id) : IRequest<ErrorOr<DeleteCategoryResponse>>;