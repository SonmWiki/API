namespace Domain.Contracts;

public abstract class BaseEntity<T> : IEntity<T>
{
    public virtual T Id { get; set; }
}