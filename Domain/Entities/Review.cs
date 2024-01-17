using Domain.Contracts;

namespace Domain.Entities;

public class Review : BaseEntity<Guid>
{
    public required string ReviewerId { get; set; }
    public required Author Reviewer { get; set; }
    public ReviewStatus Status { get; set; }
    public required string Message { get; set; }
    public DateTime ReviewTimestamp { get; set; }
    public required Guid RevisionId { get; set; }
    public virtual required Revision Revision { get; set; }
}

public enum ReviewStatus
{
    Removed,
    Rejected,
    Accepted
}