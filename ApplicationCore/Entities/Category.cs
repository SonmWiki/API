using Application.Common;

namespace Application.Entities;

public class Category: BaseEntity<string>
{
    public string? ParentId { get; set; }
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
}