namespace RAG.AI.Application.IntegrationEvents;

public class AIMediatrIntegrationEventService : IAIMediatrIntegrationEventService
{
    private readonly MediatrIntegrationEventStore _integrationEventStore;
    private readonly AIContext _dbContext;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;
    public AIMediatrIntegrationEventService(
        MediatrIntegrationEventStore integrationEventStore, AIContext dbContext, IMediator mediator, ILogger logger)
    {
        _integrationEventStore = integrationEventStore;
        _dbContext = dbContext;
        _mediator = mediator;
        _logger = logger;
    }

    public void AddAndSaveEventAsync(MediatrIntegrationEvent evt)
    {
        _logger.Information("Adding event to store {@eventName}", evt.GetGenericTypeName());
        _integrationEventStore.AddEvent(evt, _dbContext.GetCurrentTransaction());
    }

    public async Task PublishEventsThroughEventAsync(Guid transactionId)
    {
        var pendingEvents = _integrationEventStore.IntegrationEvents.Where(e => e.TransactionId == transactionId).ToList();

        foreach (var pendingEvent in pendingEvents)
        {
            _logger.Information("----- Publishing inernal integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", pendingEvent.EventId, GetType().Name, pendingEvent.IntegrationEvent);

            try
            {
                await _mediator.Publish((object)pendingEvent.IntegrationEvent);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ERROR publishing inernal integration event: {IntegrationEventId} from {AppName}", pendingEvent.EventId, GetType().Name);

            }
        }
        _integrationEventStore.ClearTransactionEvents(transactionId);

    }
}


