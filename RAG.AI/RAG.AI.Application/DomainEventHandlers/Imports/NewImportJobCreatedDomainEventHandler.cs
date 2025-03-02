using BookHouse.Core.Bases;
using BookHouse.Core.Localization;
using RAG.AI.Application.Commands.Imports.ProcessExcelImportJob;
using RAG.AI.Domain.DomainEvents.Imports;
using RAG.AI.Domain.SeedWork;

namespace RAG.AI.Application.DomainEventHandlers.Imports;
public class NewImportJobCreatedDomainEventHandler : INotificationHandler<NewImportJobCreatedDomainEvent>
{
    private readonly IMediator _mediator;

    public NewImportJobCreatedDomainEventHandler( IMediator mediator)
    {
        _mediator = mediator;
    }


    public async Task Handle(NewImportJobCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _mediator.Send(new ProcessExcelImportJobCommand(notification.JobId));
    }
}
