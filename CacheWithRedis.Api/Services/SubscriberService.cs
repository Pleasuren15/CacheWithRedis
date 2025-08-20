using System.Data;
using CacheWithRedis.Api.Models;
using Dapper;

namespace CacheWithRedis.Api.Services;

public class SubscriberService(
    IDbConnection connection,
    ICacheService cacheService,
    ILogger<SubscriberService> logger) : ISubscriberService
{
    private readonly IDbConnection _connection = connection;
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<SubscriberService> _logger = logger;
    private const string CacheKey = "subscribers_cache_key";

    public async Task<IEnumerable<Subscriber>> GetAllSubscribersAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting GetAllSubscribersAsync operation");

        try
        {
            // Try to get from cache first
            _logger.LogDebug("Attempting to retrieve subscribers from cache");
            var cachedSubscribers = await _cacheService.GetRecordAsync<IEnumerable<Subscriber>>(CacheKey, cancellationToken);
            
            if (cachedSubscribers != null)
            {
                _logger.LogInformation("Successfully retrieved {Count} subscribers from cache", cachedSubscribers.Count());
                return cachedSubscribers;
            }

            // Cache miss - get from database
            _logger.LogInformation("Cache miss - retrieving subscribers from database using stored procedure 'GetAllSubscribers'");
            var commandDefinition = new CommandDefinition(
                "GetAllSubscribers", 
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);
            var subscribers = await _connection.QueryAsync<Subscriber>(commandDefinition);

            var subscribersList = subscribers.ToList();
            _logger.LogInformation("Successfully retrieved {Count} subscribers from database", subscribersList.Count);

            // Cache the results
            _logger.LogDebug("Caching subscribers for future requests");
            await _cacheService.SetRecordAsync(CacheKey, subscribersList, TimeSpan.FromMinutes(15), null, cancellationToken);
            _logger.LogDebug("Successfully cached subscribers with 15-minute expiration");

            return subscribersList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all subscribers");
            throw;
        }
    }
}