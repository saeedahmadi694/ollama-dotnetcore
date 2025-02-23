using RAG.AI.Application.Commands.WithdrawalRequests.ChangeCashBackRequest;
using RAG.AI.Application.Commands.WithdrawalRequests.ChangeInPersonRequest;
using RAG.AI.Application.Commands.WithdrawalRequests.SetWithdrawalRequestDesription;
using RAG.AI.Application.Queries.WithdrawalRequests.ExportAllCashBacksForAdmin;
using RAG.AI.Application.Queries.WithdrawalRequests.ExportAllInPersonsForAdmin;
using RAG.AI.Application.Queries.WithdrawalRequests.GetAllWithdrawalRequestsForAdmin;
using RAG.AI.Application.Queries.WithdrawalRequests.GetWithdrawalRequestForAdmin;
using RAG.AI.Domain.Aggregates.Common;
using RAG.AI.Domain.Aggregates.WithdrawalRequestAggregate;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.WithdrawalRequestViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class WithdrawalRequestController : Controller
{
    private readonly IMediator _mediator;
    public WithdrawalRequestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> InPerson(GetAllWithdrawalRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = ExchangeType.Gold.Id };
        var items = await _mediator.Send(query, cancellationToken);
        return View(new WithdrawalRequestViewModel(query, items));
    }
    public async Task<IActionResult> CashBack(GetAllWithdrawalRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = ExchangeType.Cash.Id };
        var items = await _mediator.Send(query, cancellationToken);
        return View(new WithdrawalRequestViewModel(query, items));
    }

    public async Task<IActionResult> Detail(GetWithdrawalRequestForAdminQuery query, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(query, cancellationToken);
        return PartialView(result);
    }

    public async Task<IActionResult> SetDescription(GetWithdrawalRequestForAdminQuery query, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(query, cancellationToken);
        return PartialView(new SetWithdrawalRequestDesriptionCommand(result.Id, result.Description ?? ""));
    }

    [HttpPatch]
    public async Task<IActionResult> SetDescription(SetWithdrawalRequestDesriptionCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportCachBack([FromQuery] ExportAllCashBacksForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = ExchangeType.Cash.Id };
        var result = await _mediator.Send(query, cancellationToken);
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"CashBacks-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }
    [Produces("text/csv")]
    public async Task<IActionResult> ExportInPerson([FromQuery] ExportAllInPersonsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = ExchangeType.Gold.Id };
        var result = await _mediator.Send(query, cancellationToken);
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"InPersons-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }

    public IActionResult SetInPersonRequestToAccepted(int id)
    {
        return PartialView(new ChangeInPersonRequestStatusCommand(id, WithdrawalRequestStatus.Paid.Id, null, null, null, null));
    }

    [HttpPatch]
    public async Task<IActionResult> SetInPersonRequestToAccepted(ChangeInPersonRequestStatusCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    public IActionResult SetInPersonRequestToDelivered(int id)
    {
        return PartialView(new ChangeInPersonRequestStatusCommand(id, WithdrawalRequestStatus.ReadyForDelivery.Id, null, null, null, null));
    }

    [HttpPatch]
    public async Task<IActionResult> SetInPersonRequestToDelivered(ChangeInPersonRequestStatusCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> SetInPersonRequestToRejected(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(new ChangeInPersonRequestStatusCommand(id, WithdrawalRequestStatus.Cancelled.Id, null, null, null, null), cancellationToken);
        return Ok(result);
    }



    [HttpPatch]
    public async Task<IActionResult> SetCashBackRequestToAccepted(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(new ChangeCashBackRequestStatusCommand(id, WithdrawalRequestStatus.Paid.Id, DateTime.Now, null), cancellationToken);
        return Ok(result);
    }
    [HttpPatch]
    public async Task<IActionResult> SetCashBackRequestToRejected(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(new ChangeCashBackRequestStatusCommand(id, WithdrawalRequestStatus.Cancelled.Id, null, null), cancellationToken);
        return Ok(result);
    }
}


