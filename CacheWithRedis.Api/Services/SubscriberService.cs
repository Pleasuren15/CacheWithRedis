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
        cancellationToken.ThrowIfCancellationRequested();

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

    public async Task<Subscriber> AddSubscriberAsync(Subscriber subscriber, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Starting AddSubscriberAsync operation for subscriber with email: {Email}", subscriber.Email);

        try
        {
            var parameters = new
            {
                FullName = subscriber.FullName,
                Email = subscriber.Email,
                SubscriptionDate = subscriber.SubscriptionDate
            };

            _logger.LogDebug("Adding subscriber to database using stored procedure 'AddSubscriber'");
            var commandDefinition = new CommandDefinition(
                "AddSubscriber", 
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken);
            
            var newSubscriberId = await _connection.QuerySingleAsync<int>(commandDefinition);
            subscriber.SubscriberId = newSubscriberId;

            _logger.LogInformation("Successfully added subscriber with ID: {SubscriberId}", newSubscriberId);

            // Invalidate cache since we added a new subscriber
            _logger.LogDebug("Invalidating subscribers cache due to new subscriber addition");
            await _cacheService.RemoveRecordAsync(CacheKey, cancellationToken);
            
            return subscriber;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding subscriber with email: {Email}", subscriber.Email);
            throw;
        }
    }
}