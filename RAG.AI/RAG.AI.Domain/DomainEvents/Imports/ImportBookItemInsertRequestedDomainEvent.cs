using RAG.AI.Domain.SeedWork;
namespace RAG.AI.Domain.DomainEvents.Imports;
public record ImportBookItemInsertRequestedDomainEvent(string Isbn):IMessageDomainEvent
{
}
