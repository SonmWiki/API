using Application.Common;
using Application.Features.Articles;

namespace Application.Features.Revisions;

public class Revision : BaseEntity<string>
{
    public virtual Article? Article { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Author { get; set; }
    public DateTime Timestamp { get; set; }
    public RevisionStatus Status { get; set; }
    public required string Reviewer { get; set; }
    public string? Review { get; set; }
    public DateTime ReviewTimestamp { get; set; }
}

public enum RevisionStatus
{
    Draft,
    Created,
    Accepted,
    Rejected,
    Active
}