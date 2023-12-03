using Application.Common;

namespace Application.Entities;

public class Revision : BaseEntity<Guid>
{
    public required string ArticleId { get; set; }
    public virtual required Article Article { get; set; }
    public Guid? PreviousRevisionId { get; set; }
    public virtual Revision? PreviousRevision { get; set; }
    public required string Content { get; set; }
    public required string Author { get; set; }
    public DateTime Timestamp { get; set; }
    public RevisionStatus Status { get; set; }
    public string? Reviewer { get; set; }
    public string? Review { get; set; }
    public DateTime? ReviewTimestamp { get; set; }
}

public enum RevisionStatus
{
    Draft,
    Submitted,
    Rejected,
    Accepted,
    Active
}