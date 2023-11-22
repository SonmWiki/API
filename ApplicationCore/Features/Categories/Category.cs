using Application.Common;
using Application.Features.Articles;

namespace Application.Features.Categories;

public class Category: BaseEntity<string>
{
    public required string Title { get; set; }
    public int Weight { get; set; }
    public virtual ICollection<Article> Articles { get; set; } = new HashSet<Article>();
}