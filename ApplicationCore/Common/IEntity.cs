namespace Application.Common;

public interface IEntity<T>
{ 
    T Id { get; set; }
}