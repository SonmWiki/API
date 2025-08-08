using Domain.Contracts;

namespace Domain.Entities;

public class User : BaseEntity<Guid>
{
    public required string ExternalId { get; set; }
    public required string Name { get; set; }
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}