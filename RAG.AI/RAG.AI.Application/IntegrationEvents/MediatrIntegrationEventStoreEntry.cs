using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RAG.AI.Application.IntegrationEvents;

public class MediatrIntegrationEventStoreEntry
{
    private MediatrIntegrationEventStoreEntry() { }
    public MediatrIntegrationEventStoreEntry(MediatrIntegrationEvent @event, Guid transactionId)
    {
        EventId = @event.Id;
        CreationTime = @event.CreateDate;
        EventTypeName = @event.GetType().FullName;
        Content = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
        {
            WriteIndented = true
        });
        //State = EventStateEnum.NotPublished;
        TimesSent = 0;
        TransactionId = transactionId;
        IntegrationEvent = @event;
    }
    public Guid EventId { get; private set; }
    public string EventTypeName { get; private set; }
    [NotMapped]
    public string? EventTypeShortName => EventTypeName.Split('.')?.Last();
    [NotMapped]
    public MediatrIntegrationEvent IntegrationEvent { get; private set; }
    public int TimesSent { get; set; }
    public DateTime CreationTime { get; private set; }
    public string Content { get; private set; }
    public Guid TransactionId { get; private set; }

    public MediatrIntegrationEventStoreEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent? newEvent = JsonSerializer.Deserialize(Content, type, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) as IntegrationEvent;
        IntegrationEvent = IntegrationEvent with { CreateDate = newEvent.CreationDate, Id = newEvent.Id };
        return this;
    }
}


