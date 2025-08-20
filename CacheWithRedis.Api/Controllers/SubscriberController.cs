using CacheWithRedis.Api.Models;
using CacheWithRedis.Api.Services;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CacheWithRedis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriberController(
    IDbConnection connection,
    ICacheService cacheService,
    ILogger<SubscriberController> logger) : ControllerBase
{
    private readonly IDbConnection _connection = connection;
    private readonly ICacheService _cacheService = cacheService;
    private readonly ILogger<SubscriberController> _logger = logger;

    [HttpGet("GetAllSubscribers")]
    public async Task<ActionResult<IEnumerable<Subscriber>>> GetAllSubscribers()
    {
        try
        {
            var cacheKey = "subscriberscachekey";
            var subscribers = await _cacheService.GetRecordAsync<IEnumerable<Subscriber>>(cacheKey);
            if (subscribers is not null) { return Ok(subscribers); }

            subscribers = await _connection.QueryAsync<Subscriber>("GetAllSubscribers", commandType: CommandType.StoredProcedure);
            await _cacheService.SetRecordAsync(cacheKey, subscribers);

            return Ok(subscribers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all subscribers");
            return StatusCode(500, "An error occurred while retrieving subscribers");
        }
    }
}