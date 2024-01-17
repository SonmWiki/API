using Domain.Contracts;

namespace Domain.Entities;

public class Revision : BaseEntity<Guid>
{
    public required string ArticleId { get; set; }
    public virtual required Article Article { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public required string AuthorId { get; set; }
    public virtual required Author Author { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid? LatestReviewId { get; set; }
    public virtual Review? LatestReview { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
