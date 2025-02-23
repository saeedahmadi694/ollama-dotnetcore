namespace RAG.AI.Application.IntegrationEvents;
public interface IAIMediatrIntegrationEventService
{
    Task PublishEventsThroughEventAsync(Guid transactionId);
    void AddAndSaveEventAsync(MediatrIntegrationEvent evt);
}


