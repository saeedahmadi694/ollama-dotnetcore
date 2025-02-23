using Microsoft.EntityFrameworkCore.Storage;

namespace RAG.AI.Application.IntegrationEvents;

public class MediatrIntegrationEventStore
{
    private List<MediatrIntegrationEventStoreEntry>? _events;

    public IReadOnlyCollection<MediatrIntegrationEventStoreEntry> IntegrationEvents => _events ?? new List<MediatrIntegrationEventStoreEntry>();

    public void AddEvent(MediatrIntegrationEvent @event, IDbContextTransaction transaction)
    {
        _events ??= new List<MediatrIntegrationEventStoreEntry>();

        var eventStoreEntry = new MediatrIntegrationEventStoreEntry(@event, transaction.TransactionId);
        _events.Add(eventStoreEntry);
    }

    public void ClearTransactionEvents(Guid transactionId)
    {
        if (_events != null)
        {
            _events = _events.Where(e => e.TransactionId == transactionId).ToList();
        }
    }


}


