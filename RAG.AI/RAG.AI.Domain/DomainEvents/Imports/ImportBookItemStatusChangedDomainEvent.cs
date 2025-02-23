using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.DomainEvents.Imports;
public record ImportBookItemStatusChangedDomainEvent(string Isbn) : IMessageDomainEvent
{
}