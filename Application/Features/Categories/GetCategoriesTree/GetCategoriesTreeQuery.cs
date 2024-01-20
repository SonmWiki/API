using ErrorOr;
using MediatR;

namespace Application.Features.Categories.GetCategoriesTree;

public record GetCategoriesTreeQuery : IRequest<ErrorOr<GetCategoriesTreeResponse>>;