using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CacheWithRedis.Api.Services;

public class CacheService(IDistributedCache cache) : ICacheService
{
    private readonly IDistributedCache _cache = cache;

    public async Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime ?? TimeSpan.FromSeconds(60)
        };

        var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await _cache.SetStringAsync(recordId, jsonData, options);
    }

    public async Task<T> GetRecordAsync<T>(string recordId)
    {
        var jsonData = await _cache.GetStringAsync(recordId);
        if (jsonData is null)
        {
            return default!;
        }

        var data = JsonSerializer.Deserialize<T>(jsonData);
        return data!;
    }
}