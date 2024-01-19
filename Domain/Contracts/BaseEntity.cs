namespace Domain.Contracts;

public abstract class BaseEntity<T> : IEntity<T>
{
    public virtual required T Id { get; set; }
}