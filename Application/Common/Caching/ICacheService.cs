namespace Application.Common.Caching;

public interface ICacheService
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken token = default
    );

    Task RemoveAsync(string key, CancellationToken token = default);
}