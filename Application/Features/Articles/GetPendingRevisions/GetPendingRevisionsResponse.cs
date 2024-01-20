namespace Application.Features.Articles.GetPendingRevisions;

public record GetPendingRevisionsResponse(List<GetPendingRevisionsResponse.Element> Data)
{
    public record Element(
        string ArticleId,
        string ArticleIdTitle,
        Guid RevisionId,
        Author Author,
        DateTime Timestamp
    );

    public record Author(string Id, string Name);
}