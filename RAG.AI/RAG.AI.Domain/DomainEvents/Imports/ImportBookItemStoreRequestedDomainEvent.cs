using RAG.AI.Domain.Aggregates.ImportAggregate;
using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Domain.DomainEvents.Imports;
//itemId,bookItemId,CoreId
public record ImportBookItemStoreRequestedDomainEvent(int VendorId,int JobId, ICollection<Tuple<long, long, int>> StoreBookItems) : IMessageDomainEvent
{
}
