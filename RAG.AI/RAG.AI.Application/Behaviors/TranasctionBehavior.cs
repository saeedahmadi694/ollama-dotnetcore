using Microsoft.EntityFrameworkCore;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Extentions;

namespace RAG.AI.Application.Behaviors;

public class TranasactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly ILogger _logger;
    private readonly IUnitOfWork _uow;
    private readonly IAIIntegrationEventService _integrationEventService;
    private readonly IAIMediatrIntegrationEventService _mediatrIntegrationEventService;
    private readonly IMediator _mediator;
    private readonly CommandHelper _commandHelper;
    private readonly AIContext _context;

    public TranasactionBehavior(ILogger logger, IUnitOfWork uow,
        IAIIntegrationEventService orderingIntegrationEventService, IMediator mediator, CommandHelper commandHelper, AIContext context, IAIMediatrIntegrationEventService mediatrIntegrationEventService)
    {
        _logger = logger;
        _uow = uow;
        _integrationEventService = orderingIntegrationEventService;
        _mediator = mediator;
        _commandHelper = commandHelper;
        _context = context;
        _mediatrIntegrationEventService = mediatrIntegrationEventService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            if (_uow.HasActiveTransaction)
            {
                _commandHelper.NeedCommitting = true;
                return await next();
            }

            var strategy = _uow.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using var transaction = await _uow.BeginTransactionAsync();
                using (Serilog.Context.LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                {
                    _logger.Information("----- Begin transaction {TransactionId} for {CommandName} ({@Command})",
                        transaction.TransactionId, typeName, request);

                    response = await next();

                    _logger.Information("----- Commit transaction {TransactionId} for {CommandName}",
                        transaction.TransactionId, typeName);
                    while (_commandHelper.NeedCommitting)
                    {
                        _commandHelper.NeedCommitting = false;
                        await _mediator.DispatchDomainEventsAsync(_context);
                    }

                    await _uow.CommitTransactionAsync(transaction);

                    transactionId = transaction.TransactionId;
                }
                await _integrationEventService.PublishEventsThroughEventBusAsync(transactionId);
                await _mediatrIntegrationEventService.PublishEventsThroughEventAsync(transactionId);
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}


