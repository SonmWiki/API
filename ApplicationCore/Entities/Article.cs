using Application.Common;

namespace Application.Entities;

public class Article : BaseEntity<string>
{
    public bool IsVisible { get; set; }
    public string? RedirectArticleId { get; set; }
    public virtual Article? RedirectArticle { get; set; }
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
    public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();
}