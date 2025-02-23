namespace RAG.AI.Application.IntegrationEvents;
public interface IAIIntegrationEventService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    void AddAndSaveEventAsync(IntegrationEvent evt);
}



