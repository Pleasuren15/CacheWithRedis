using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CacheWithRedis.Api.Services;

public class CacheService(IDistributedCache cache, ILogger<CacheService> logger) : ICacheService
{
    private readonly IDistributedCache _cache = cache;
    private readonly ILogger<CacheService> _logger = logger;

    public async Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        _logger.LogDebug("Setting cache record with key: {RecordId}, Type: {DataType}", recordId, typeof(T).Name);
        
        try
        {
            var absoluteExpiry = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            var slidingExpiry = unusedExpireTime ?? TimeSpan.FromSeconds(60);
            
            _logger.LogDebug("Cache expiration settings - Absolute: {AbsoluteExpiry}, Sliding: {SlidingExpiry}", 
                absoluteExpiry, slidingExpiry);
            
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiry,
                SlidingExpiration = slidingExpiry
            };

            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            _logger.LogDebug("Serialized data size: {DataSize} characters for key: {RecordId}", 
                jsonData.Length, recordId);

            await _cache.SetStringAsync(recordId, jsonData, options);
            
            _logger.LogInformation("Successfully cached data for key: {RecordId}", recordId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to set cache record for key: {RecordId}", recordId);
            throw;
        }
    }

    public async Task<T> GetRecordAsync<T>(string recordId)
    {
        _logger.LogDebug("Retrieving cache record with key: {RecordId}, Expected type: {DataType}", 
            recordId, typeof(T).Name);
        
        try
        {
            var jsonData = await _cache.GetStringAsync(recordId);
            if (jsonData is null)
            {
                _logger.LogDebug("Cache miss for key: {RecordId}", recordId);
                return default!;
            }

            _logger.LogDebug("Cache hit for key: {RecordId}, Data size: {DataSize} characters", 
                recordId, jsonData.Length);

            var data = JsonSerializer.Deserialize<T>(jsonData);
            
            _logger.LogInformation("Successfully retrieved and deserialized data from cache for key: {RecordId}", recordId);
            return data!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get cache record for key: {RecordId}", recordId);
            throw;
        }
    }
}