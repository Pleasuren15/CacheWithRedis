using CacheWithRedis.Api.Models;
using FluentValidation;

namespace CacheWithRedis.Api.Validators;

public class SubscriberValidator : AbstractValidator<Subscriber>
{
    public SubscriberValidator()
    {
        RuleFor(s => s.FullName)
            .Length(7, 20)
            .NotEmpty()
            .WithMessage("FullName value validation failed");

        RuleFor(s => s.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email value validation failed");

        RuleFor(s => s.SubscriptionDate)
            .NotNull()
            .GreaterThan(DateTime.Now)
            .WithMessage("SubscriptionDate value validation failed");
    }
}
