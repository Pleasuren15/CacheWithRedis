using System.Text.Json;
using CacheWithRedis.Api.Controllers;
using CacheWithRedis.Api.Models;
using CacheWithRedis.Api.Unit.Tests.Substitutes;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CacheWithRedis.Api.Unit.Tests.Controllers;

[TestFixture]
public class SubscriberControllerTests
{
    private SubscriberController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        SubscriberControllerSubstitutes.SubscriberService.ClearReceivedCalls();
        SubscriberControllerSubstitutes.Logger.ClearReceivedCalls();
        SubscriberControllerSubstitutes.Validator.ClearReceivedCalls();
        
        _controller = new SubscriberController(
            SubscriberControllerSubstitutes.SubscriberService,
            SubscriberControllerSubstitutes.Logger,
            SubscriberControllerSubstitutes.Validator);
    }

    #region GetAllSubscribers Tests

    [Test]
    public async Task Given_ServiceThrowsException_When_GetAllSubscribers_Then_ReturnsStatus500()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        
        SubscriberControllerSubstitutes.SubscriberService
            .GetAllSubscribersAsync(cancellationToken)
            .Throws(new InvalidOperationException("Database error"));

        // Act
        var result = await _controller.GetAllSubscribers(cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = (ObjectResult)result.Result!;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value, Is.EqualTo("An error occurred while retrieving subscribers"));
    }

    [Test]
    public async Task Given_EmptySubscriberList_When_GetAllSubscribers_Then_ReturnsOkWithEmptyList()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var expectedSubscribers = new List<Subscriber>();
        
        SubscriberControllerSubstitutes.SubscriberService
            .GetAllSubscribersAsync(cancellationToken)
            .Returns(expectedSubscribers);

        // Act
        var result = await _controller.GetAllSubscribers(cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result.Result!;
        Assert.That(okResult.Value, Is.EqualTo(expectedSubscribers));
        Assert.That(((IEnumerable<Subscriber>)okResult.Value!).Count(), Is.EqualTo(0));
    }

    #endregion

    #region AddSubscriber Tests

    [Test]
    public async Task Given_ValidSubscriber_When_AddSubscriber_Then_ReturnsCreatedResult()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var inputSubscriber = new Subscriber 
        { 
            FullName = "John Doe", 
            Email = "john@example.com", 
            SubscriptionDate = DateTime.UtcNow 
        };
        
        var expectedSubscriber = new Subscriber 
        { 
            SubscriberId = 1, 
            FullName = "John Doe", 
            Email = "john@example.com", 
            SubscriptionDate = inputSubscriber.SubscriptionDate 
        };

        var validationResult = new ValidationResult();
        
        SubscriberControllerSubstitutes.Validator
            .Validate(inputSubscriber)
            .Returns(validationResult);
            
        SubscriberControllerSubstitutes.SubscriberService
            .AddSubscriberAsync(inputSubscriber, cancellationToken)
            .Returns(expectedSubscriber);

        // Act
        var result = await _controller.AddSubscriber(inputSubscriber, cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = (CreatedAtActionResult)result.Result!;
        Assert.That(createdResult.Value, Is.EqualTo(expectedSubscriber));
        Assert.That(createdResult.ActionName, Is.EqualTo(nameof(_controller.AddSubscriber)));
        
        await SubscriberControllerSubstitutes.SubscriberService.Received(1)
            .AddSubscriberAsync(inputSubscriber, cancellationToken);
    }

    [Test]
    public void Given_InvalidSubscriber_When_AddSubscriber_Then_ThrowsValidationException()
    {
        // Arrange
        var subscriber = new Subscriber 
        { 
            FullName = "", 
            Email = "invalid-email", 
            SubscriptionDate = DateTime.UtcNow 
        };

        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("FullName", "Full name is required"));
        validationResult.Errors.Add(new ValidationFailure("Email", "Email is invalid"));
        
        SubscriberControllerSubstitutes.Validator
            .Validate(subscriber)
            .Returns(validationResult);

        // Act & Assert
        var exception = Assert.ThrowsAsync<Exception>(async () => 
            await _controller.AddSubscriber(subscriber, CancellationToken.None));
        
        Assert.That(exception!.Message, Contains.Substring("Full name is required"));
        Assert.That(exception.Message, Contains.Substring("Email is invalid"));
    }

    [Test]
    public async Task Given_ServiceThrowsOperationCanceledException_When_AddSubscriber_Then_ReturnsStatus499()
    {
        // Arrange
        var subscriber = new Subscriber 
        { 
            FullName = "John Doe", 
            Email = "john@example.com", 
            SubscriptionDate = DateTime.UtcNow 
        };
        
        var cancellationToken = CancellationToken.None;
        var validationResult = new ValidationResult();
        
        SubscriberControllerSubstitutes.Validator
            .Validate(subscriber)
            .Returns(validationResult);
            
        SubscriberControllerSubstitutes.SubscriberService
            .AddSubscriberAsync(subscriber, cancellationToken)
            .Throws<OperationCanceledException>();

        // Act
        var result = await _controller.AddSubscriber(subscriber, cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = (ObjectResult)result.Result!;
        Assert.That(objectResult.StatusCode, Is.EqualTo(499));
        Assert.That(objectResult.Value, Is.EqualTo("Request was cancelled"));
    }

    [Test]
    public async Task Given_ServiceThrowsException_When_AddSubscriber_Then_ReturnsStatus500()
    {
        // Arrange
        var subscriber = new Subscriber 
        { 
            FullName = "John Doe", 
            Email = "john@example.com", 
            SubscriptionDate = DateTime.UtcNow 
        };
        
        var cancellationToken = CancellationToken.None;
        var validationResult = new ValidationResult();
        
        SubscriberControllerSubstitutes.Validator
            .Validate(subscriber)
            .Returns(validationResult);
            
        SubscriberControllerSubstitutes.SubscriberService
            .AddSubscriberAsync(subscriber, cancellationToken)
            .Throws(new InvalidOperationException("Database error"));

        // Act
        var result = await _controller.AddSubscriber(subscriber, cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var objectResult = (ObjectResult)result.Result!;
        Assert.That(objectResult.StatusCode, Is.EqualTo(500));
        Assert.That(objectResult.Value, Is.EqualTo("An error occurred while adding the subscriber"));
    }

    [Test]
    public async Task Given_SubscriberWithFutureDate_When_AddSubscriber_Then_ReturnsCreatedResult()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var futureDate = DateTime.UtcNow.AddDays(30);
        var inputSubscriber = new Subscriber 
        { 
            FullName = "Future Subscriber", 
            Email = "future@example.com", 
            SubscriptionDate = futureDate 
        };
        
        var expectedSubscriber = new Subscriber 
        { 
            SubscriberId = 99, 
            FullName = "Future Subscriber", 
            Email = "future@example.com", 
            SubscriptionDate = futureDate 
        };

        var validationResult = new ValidationResult();
        
        SubscriberControllerSubstitutes.Validator
            .Validate(inputSubscriber)
            .Returns(validationResult);
            
        SubscriberControllerSubstitutes.SubscriberService
            .AddSubscriberAsync(inputSubscriber, cancellationToken)
            .Returns(expectedSubscriber);

        // Act
        var result = await _controller.AddSubscriber(inputSubscriber, cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = (CreatedAtActionResult)result.Result!;
        Assert.That(createdResult.Value, Is.EqualTo(expectedSubscriber));
        Assert.That(((Subscriber)createdResult.Value!).SubscriptionDate, Is.EqualTo(futureDate));
    }

    [Test]
    public async Task Given_SubscriberWithLongName_When_AddSubscriber_Then_ReturnsCreatedResult()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var longName = new string('A', 1000);
        var inputSubscriber = new Subscriber 
        { 
            FullName = longName, 
            Email = "long@example.com", 
            SubscriptionDate = DateTime.UtcNow 
        };
        
        var expectedSubscriber = new Subscriber 
        { 
            SubscriberId = 999, 
            FullName = longName, 
            Email = "long@example.com", 
            SubscriptionDate = inputSubscriber.SubscriptionDate 
        };

        var validationResult = new ValidationResult();
        
        SubscriberControllerSubstitutes.Validator
            .Validate(inputSubscriber)
            .Returns(validationResult);
            
        SubscriberControllerSubstitutes.SubscriberService
            .AddSubscriberAsync(inputSubscriber, cancellationToken)
            .Returns(expectedSubscriber);

        // Act
        var result = await _controller.AddSubscriber(inputSubscriber, cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = (CreatedAtActionResult)result.Result!;
        Assert.That(createdResult.Value, Is.EqualTo(expectedSubscriber));
        Assert.That(((Subscriber)createdResult.Value!).FullName.Length, Is.EqualTo(1000));
    }

    #endregion

    #region Edge Cases and Error Scenarios

    [Test]
    public async Task Given_ValidSubscriberWithMinimumValidDate_When_AddSubscriber_Then_ReturnsCreatedResult()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var minDate = new DateTime(1900, 1, 1);
        var inputSubscriber = new Subscriber 
        { 
            FullName = "Historical Subscriber", 
            Email = "historical@example.com", 
            SubscriptionDate = minDate 
        };
        
        var expectedSubscriber = new Subscriber 
        { 
            SubscriberId = 1900, 
            FullName = "Historical Subscriber", 
            Email = "historical@example.com", 
            SubscriptionDate = minDate 
        };

        var validationResult = new ValidationResult();
        
        SubscriberControllerSubstitutes.Validator
            .Validate(inputSubscriber)
            .Returns(validationResult);
            
        SubscriberControllerSubstitutes.SubscriberService
            .AddSubscriberAsync(inputSubscriber, cancellationToken)
            .Returns(expectedSubscriber);

        // Act
        var result = await _controller.AddSubscriber(inputSubscriber, cancellationToken);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        var createdResult = (CreatedAtActionResult)result.Result!;
        Assert.That(createdResult.Value, Is.EqualTo(expectedSubscriber));
    }

    [Test]
    public async Task Given_MultipleValidationErrors_When_AddSubscriber_Then_ThrowsExceptionWithAllErrors()
    {
        // Arrange
        var subscriber = new Subscriber 
        { 
            FullName = "", 
            Email = "", 
            SubscriptionDate = DateTime.MinValue 
        };

        var validationResult = new ValidationResult();
        validationResult.Errors.Add(new ValidationFailure("FullName", "Full name is required"));
        validationResult.Errors.Add(new ValidationFailure("Email", "Email is required"));
        validationResult.Errors.Add(new ValidationFailure("SubscriptionDate", "Subscription date is invalid"));
        
        SubscriberControllerSubstitutes.Validator
            .Validate(subscriber)
            .Returns(validationResult);

        // Act & Assert
        var exception = Assert.ThrowsAsync<Exception>(async () => 
            await _controller.AddSubscriber(subscriber, CancellationToken.None));
        
        Assert.That(exception!.Message, Contains.Substring("Full name is required"));
        Assert.That(exception.Message, Contains.Substring("Email is required"));
        Assert.That(exception.Message, Contains.Substring("Subscription date is invalid"));
    }

    #endregion
}