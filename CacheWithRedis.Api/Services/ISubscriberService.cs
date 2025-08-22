using CacheWithRedis.Api.Models;

namespace CacheWithRedis.Api.Services;

public interface ISubscriberService
{
    Task<IEnumerable<Subscriber>> GetAllSubscribersAsync(CancellationToken cancellationToken = default);
    Task<Subscriber> AddSubscriberAsync(Subscriber subscriber, CancellationToken cancellationToken = default);
}