using Domain.Entities;

namespace WebApi.Features.Articles.Requests;

public record ReviewArticleRevisionRequest(ReviewStatus Status, string Review);