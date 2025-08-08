namespace Application.Common.Messaging;

public interface ICachedQuery<TResponse> : IQuery<TResponse>, ICachedQuery;

public interface ICachedQuery
{
    string Key { get; }

    TimeSpan? Expiration { get; }

    bool IgnoreCaching { get; }
}