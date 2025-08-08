using Domain.Contracts;

namespace Domain.Entities;

public class Role : BaseEntity<Guid>
{
    public required string Name { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}