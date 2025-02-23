﻿namespace RAG.AI.Infrastructure.Extentions;

static class PublisheEndpointExtentions
{
    public static async Task DispatchDomainEventsAsync(this IPublishEndpoint publisher, AIContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any(e => e is IMessageDomainEvent))
            .ToList();

        if (!domainEntities.Any())
            return;

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await publisher.Publish((object)domainEvent);
    }
}



