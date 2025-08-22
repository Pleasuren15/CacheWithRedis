using System.Data;
using CacheWithRedis.Api.Controllers;
using CacheWithRedis.Api.Models;
using CacheWithRedis.Api.Services;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace CacheWithRedis.Api.Unit.Tests.Substitutes;

public static class SubscriberControllerSubstitutes
{
    public static ISubscriberService SubscriberService = Substitute.For<ISubscriberService>();
    public static ILogger<SubscriberController> Logger = Substitute.For<ILogger<SubscriberController>>();
    public static IValidator<Subscriber> Validator = Substitute.For<IValidator<Subscriber>>();
    
    // Dependencies for SubscriberService
    public static IDbConnection DbConnection = Substitute.For<IDbConnection>();
    public static ICacheService CacheService = Substitute.For<ICacheService>();
    public static ILogger<SubscriberService> SubscriberServiceLogger = Substitute.For<ILogger<SubscriberService>>();
    
    // Dependencies for CacheService
    public static IDistributedCache DistributedCache = Substitute.For<IDistributedCache>();
    public static ILogger<CacheService> CacheServiceLogger = Substitute.For<ILogger<CacheService>>();
}