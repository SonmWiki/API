namespace Domain.Common;

public abstract class BaseEntity<T> : IEntity<T>
{
    public virtual T id { get; set; }
}