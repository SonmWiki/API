using ErrorOr;
using MediatR;

namespace Application.Features.Categories.GetCategoryArticles;

public record GetCategoryArticlesQuery(string Id) : IRequest<ErrorOr<GetCategoryArticlesResponse>>;