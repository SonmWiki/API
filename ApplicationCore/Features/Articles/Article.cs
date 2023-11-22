using Application.Common;
using Application.Features.Categories;
using Application.Features.Revisions;

namespace Application.Features.Articles;

public class Article : BaseEntity<int>
{
    public required string Path { get; set; } 
    public bool IsVisible { get; set; }
    public int Weight { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<Revision> Revisions { get; set; } = new HashSet<Revision>();
}