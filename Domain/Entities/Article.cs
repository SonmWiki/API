using Domain.Contracts;

namespace Domain.Entities;

public class Article : BaseEntity<string>
{
    public required string Title { get; set; }
    public ArticleStatus ArticleStatus { get; set; }
    public string? RedirectArticleId { get; set; }
    public virtual Article? RedirectArticle { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
    public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();
}

public enum ArticleStatus
{
    Submitted,
    Rejected,
    Redirect,
    Active
}