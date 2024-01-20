using Domain.Entities;

namespace Application.Features.Articles.GetArticle;

public record GetArticleResponse(
    string Id,
    string Title,
    string? Content,
    List<GetArticleResponse.Author> Contributors,
    Guid? RevisionId,
    ReviewStatus? ReviewStatus,
    DateTime? SubmittedTimestamp,
    DateTime? ReviewTimestamp,
    List<GetArticleResponse.Category> Categories
)
{
    public record Author(string Id, string Name);

    public record Category(string Id, string Name);
}