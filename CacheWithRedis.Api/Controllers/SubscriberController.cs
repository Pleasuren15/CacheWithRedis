using CacheWithRedis.Api.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CacheWithRedis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriberController(IDbConnection connection, ILogger<SubscriberController> logger) : ControllerBase
{
    private readonly IDbConnection _connection = connection;
    private readonly ILogger<SubscriberController> _logger = logger;

    [HttpGet("GetAllSubscribers")]
    public async Task<ActionResult<IEnumerable<Subscriber>>> GetAllSubscribers()
    {
        try
        {
            var subscribers = await _connection.QueryAsync<Subscriber>("GetAllSubscribers", commandType: CommandType.StoredProcedure);
            return Ok(subscribers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all subscribers");
            return StatusCode(500, "An error occurred while retrieving subscribers");
        }
    }
}