using Domain.Entities;

namespace Application.Features.Articles.GetRevisionHistory;

public record GetRevisionHistoryResponse(List<GetRevisionHistoryResponse.Element> Data)
{
    public record Element(
        Guid Id,
        Author Contributor,
        string AuthorsNote,
        DateTime Timestamp,
        Review? LatestReview
    );

    public record Author(Guid Id, string Name);

    public record Review(
        Guid Id,
        Author Reviewer,
        ReviewStatus Status,
        string Message,
        DateTime ReviewTimestamp
    );
}