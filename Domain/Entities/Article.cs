using Domain.Common;

namespace Domain.Entities;

public class Article : BaseEntity<int>
{
    public string Path { get; set; }
    public bool IsVisible { get; set; }
    public int Weight { get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<Revision> Revisions { get; set; }
}