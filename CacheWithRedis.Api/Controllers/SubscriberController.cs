using System.Text.Json;
using CacheWithRedis.Api.Models;
using CacheWithRedis.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CacheWithRedis.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriberController(
    ISubscriberService subscriberService,
    ILogger<SubscriberController> logger,
    IValidator<Subscriber> validator) : ControllerBase
{
    private readonly ISubscriberService _subscriberService = subscriberService;
    private readonly ILogger<SubscriberController> _logger = logger;
    private readonly IValidator<Subscriber> _validator = validator;

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

    [HttpPost("AddSubscriber")]
    public async Task<ActionResult<Subscriber>> AddSubscriber([FromBody] Subscriber subscriber, CancellationToken cancellationToken)
    {
        var validatioinResults = _validator.Validate(subscriber);
        if (validatioinResults.Errors.Count > 0)
        {
            throw new Exception(message: JsonSerializer.Serialize(validatioinResults.ToDictionary(), new JsonSerializerOptions { WriteIndented = true }));
        }
        cancellationToken.ThrowIfCancellationRequested();
        _logger.LogInformation("AddSubscriber endpoint called for email: {Email}", subscriber.Email);

        try
        {
            var newSubscriber = await _subscriberService.AddSubscriberAsync(subscriber, cancellationToken);
            _logger.LogInformation("Successfully processed AddSubscriber request for ID: {SubscriberId}", newSubscriber.SubscriberId);
            return CreatedAtAction(nameof(AddSubscriber), new { id = newSubscriber.SubscriberId }, newSubscriber);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("AddSubscriber request was cancelled for email: {Email}", subscriber.Email);
            return StatusCode(499, "Request was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in AddSubscriber endpoint for email: {Email}", subscriber.Email);
            return StatusCode(500, "An error occurred while adding the subscriber");
        }
    }
}