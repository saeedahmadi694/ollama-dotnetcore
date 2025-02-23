using RAG.AI.Application.IntegrationEvents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RAG.AI.Presenter.Web.Controllers;


public class DevController : Controller
{
    private readonly IAIMediatrIntegrationEventService _shopIntegrationEventService;
    private readonly IMediator _mediator;

    public DevController(IAIMediatrIntegrationEventService shopIntegrationEventService, IMediator mediator)
    {
        _shopIntegrationEventService = shopIntegrationEventService;
        _mediator = mediator;
    }

    public IActionResult Test()
    {
        //var setStatusCommand = new SetPaymentStatusCommand(1, true);
        //await _mediator.Send(setStatusCommand);
        //await _mediator.Publish(
        //     new PaymentStatusChangedIntegrationEvent(Guid.Parse("F7D30DFA-B4F3-405B-8DC3-1CF2FBE340FD"), true, 2425000));

        return Ok("TEST SUCCESS");
    }
}



