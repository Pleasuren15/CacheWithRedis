using System.Data;
using CacheWithRedis.Api.Models;
using CacheWithRedis.Api.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace CacheWithRedis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriberController(
    IDbConnection _connection,
    ICacheService _cacheService,
    ILogger<SubscriberController> _logger) : ControllerBase
{
    [HttpGet("GetAllSubscribers")]
    public async Task<ActionResult<IEnumerable<Subscriber>>> GetAllSubscribers()
    {
        _logger.LogInformation("GetAllSubscribers endpoint called");
        
        try
        {
            var cacheKey = "subscriberscachekey";
            
            _logger.LogDebug("Attempting to retrieve subscribers from cache with key: {CacheKey}", cacheKey);
            var subscribers = await _cacheService.GetRecordAsync<IEnumerable<Subscriber>>(cacheKey);
            
            if (subscribers is not null) 
            { 
                _logger.LogInformation("Successfully retrieved {Count} subscribers from cache", subscribers.Count());
                return Ok(subscribers); 
            }

            _logger.LogInformation("Cache miss - retrieving subscribers from database using stored procedure");
            subscribers = await _connection.QueryAsync<Subscriber>("GetAllSubscribers", commandType: CommandType.StoredProcedure);
            
            _logger.LogInformation("Successfully retrieved {Count} subscribers from database", subscribers.Count());
            
            _logger.LogDebug("Caching subscribers with key: {CacheKey}", cacheKey);
            await _cacheService.SetRecordAsync(cacheKey, subscribers);
            _logger.LogDebug("Successfully cached subscribers");

            return Ok(subscribers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all subscribers");
            return StatusCode(500, "An error occurred while retrieving subscribers");
        }
    }
}