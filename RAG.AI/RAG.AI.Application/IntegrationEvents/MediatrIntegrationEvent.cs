using System;

namespace RAG.AI.Application.IntegrationEvents;

public record MediatrIntegrationEvent(Guid Id, DateTime CreateDate) : INotification
{
    public MediatrIntegrationEvent() : this(Guid.NewGuid(), DateTime.UtcNow) { }
}



