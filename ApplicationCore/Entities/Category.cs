using Application.Common;

namespace Application.Entities;

public class Category: BaseEntity<string>
{
    public required string Title { get; set; }
    public int Weight { get; set; }
    public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
}