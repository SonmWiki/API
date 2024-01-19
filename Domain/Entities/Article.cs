using Domain.Contracts;

namespace Domain.Entities;

public class Article : BaseEntity<string>
{
    public required string Title { get; set; }
    public string? RedirectArticleId { get; set; }
    public virtual Article? RedirectArticle { get; set; }
    public Guid? CurrentRevisionId { get; set; }
    public virtual Revision? CurrentRevision { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();
}