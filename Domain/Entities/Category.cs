using Domain.Common;

namespace Domain.Entities;

public sealed class Category: BaseEntity<string>
{
    public string Title { get; set; }
    public int Weight { get; set; }
}