using ErrorOr;
using MediatR;

namespace Application.Features.Categories.GetCategories;

public record GetCategoriesQuery : IRequest<ErrorOr<GetCategoriesResponse>>;