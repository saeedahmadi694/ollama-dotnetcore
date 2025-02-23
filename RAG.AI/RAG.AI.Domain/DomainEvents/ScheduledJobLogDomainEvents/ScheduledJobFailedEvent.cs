using RAG.AI.Domain.Aggregates.ScheduledJobLogAggregate;
using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.DomainEvents.ScheduledJobLogDomainEvents;

public record ScheduledJobFailedEvent(ScheduledJobLog ScheduledJob) : IMessageDomainEvent { }

