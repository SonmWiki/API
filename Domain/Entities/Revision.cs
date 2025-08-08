using Domain.Contracts;

namespace Domain.Entities;

public class Revision : BaseEntity<Guid>
{
    public required string ArticleId { get; set; }
    public virtual required Article Article { get; set; }
    public required string Content { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public required Guid AuthorId { get; set; }
    public virtual required User Author { get; set; }
    public required string AuthorsNote { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid? LatestReviewId { get; set; }
    public virtual Review? LatestReview { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}