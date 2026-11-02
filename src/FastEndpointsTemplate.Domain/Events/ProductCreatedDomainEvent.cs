using FastEndpointsTemplate.Domain.Common;

namespace FastEndpointsTemplate.Domain.Events;

public sealed record ProductCreatedDomainEvent(Guid ProductId, string Sku, decimal Price) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
