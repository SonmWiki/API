namespace Application.Common.Messaging;

//TODO: Redo without MediatR
public interface ICachedQuery<TResponse> : IQuery, ICachedQuery;

public interface ICachedQuery
{
    string Key { get; }

    TimeSpan? Expiration { get; }

    bool IgnoreCaching { get; }
}