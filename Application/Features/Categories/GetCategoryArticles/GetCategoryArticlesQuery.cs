using Application.Common.Messaging;

namespace Application.Features.Categories.GetCategoryArticles;

public record GetCategoryArticlesQuery(string Id) : IRequest<ErrorOr<GetCategoryArticlesResponse>>;
public record GetCategoryArticlesQuery(string Id) : IQuery<GetCategoryArticlesResponse>;