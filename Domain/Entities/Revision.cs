using Domain.Common;

namespace Domain.Entities;

public class Revision : BaseEntity<string>
{
    public virtual Article Article { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public DateTime Timestamp { get; set; }
    public RevisionStatus Status { get; set; }
    public string Reviewer { get; set; }
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