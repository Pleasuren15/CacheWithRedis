using CacheWithRedis.Api.Models;
using CacheWithRedis.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CacheWithRedis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriberController(
    ISubscriberService subscriberService,
    ILogger<SubscriberController> logger) : ControllerBase
{
    private readonly ISubscriberService _subscriberService = subscriberService;
    private readonly ILogger<SubscriberController> _logger = logger;

    [HttpGet("GetAllSubscribers")]
    public async Task<ActionResult<IEnumerable<Subscriber>>> GetAllSubscribers(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogInformation("GetAllSubscribers endpoint called");
        
        try
        {
            var subscribers = await _subscriberService.GetAllSubscribersAsync(cancellationToken);
            _logger.LogInformation("Successfully processed GetAllSubscribers request");
            return Ok(subscribers);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("GetAllSubscribers request was cancelled");
            return StatusCode(499, "Request was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetAllSubscribers endpoint");
            return StatusCode(500, "An error occurred while retrieving subscribers");
        }
    }
}