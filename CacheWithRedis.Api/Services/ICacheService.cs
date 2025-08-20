namespace CacheWithRedis.Api.Services;

public interface ICacheService
{
    Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null, CancellationToken cancellationToken = default);
    Task<T> GetRecordAsync<T>(string recordId, CancellationToken cancellationToken = default);
}