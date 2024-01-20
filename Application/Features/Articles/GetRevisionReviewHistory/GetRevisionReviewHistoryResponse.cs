using Domain.Entities;

namespace Application.Features.Articles.GetRevisionReviewHistory;

public record GetRevisionReviewHistoryResponse(List<GetRevisionReviewHistoryResponse.Element> Data)
{
    public record Element(
        Guid Id,
        Reviewer Reviewer,
        ReviewStatus Status,
        string Message,
        DateTime Timestamp
    );

    public record Reviewer(string Id, string Name);
}