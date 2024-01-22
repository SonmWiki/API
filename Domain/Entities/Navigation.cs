using Domain.Contracts;

namespace Domain.Entities;

public class Navigation : BaseEntity<int>
{
    public required string Name { get; set; }
    public int Weight { get; set; }
    public string? Uri { get; set; }
    public int? ParentId { get; set; }
    public string? Icon { get; set; }
    public virtual ICollection<Navigation> Children { get; set; } = new List<Navigation>();
}