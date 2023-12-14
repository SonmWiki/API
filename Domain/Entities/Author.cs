using Domain.Contracts;

namespace Domain.Entities;

public class Author : BaseEntity<string>
{
    public required string Name { get; set; }
}