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
    List<string> Categories
)
{
    public record Author(string Id, string Name);
}