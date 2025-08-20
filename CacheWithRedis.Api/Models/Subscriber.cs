namespace CacheWithRedis.Api.Models;

public class Subscriber
{
    public int SubscriberId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime SubscriptionDate { get; set; }
}