using Domain.Contracts;

namespace Domain.Entities;

public class Permission : BaseEntity<Guid>
{
    public required string Name { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}