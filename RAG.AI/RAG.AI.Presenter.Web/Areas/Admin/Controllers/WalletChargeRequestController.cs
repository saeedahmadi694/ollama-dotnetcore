using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Queries.WalletChargeRequests.GetAllWalletChargeRequestsForAdmin;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.WalletChargeRequestViewModels;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Domain.Aggregates.WithdrawalRequestAggregate;
using RAG.AI.Application.Commands.WalletChargeRequests.SetWalletChargeRequestsDesription;
using RAG.AI.Application.Queries.WalletChargeRequests.ExportAllWalletChargeRequestsForAdmin;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Application.Queries.WalletChargeRequests.WalletChargeRequestsForAdmin;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]

public class WalletChargeRequestController : Controller
{
    private readonly IMediator _mediator;
    public WalletChargeRequestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(GetAllWalletChargeRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new WalletChargeRequestViewModel(query, items));
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportWalletChargeRequests([FromQuery] ExportAllWalletChargeRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"walletChargeRequests-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }

    public async Task<IActionResult> SetDescription(WalletChargeRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return PartialView(new SetWalletChargeRequestsDesriptionCommand(result.Id, result.Description ?? ""));
    }

    [HttpPatch]
    public async Task<IActionResult> SetDescription(SetWalletChargeRequestsDesriptionCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    //[HttpDelete]
    //public async Task<ActionResult<ReturnedDto>> Delete(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        await _mediator.Send(new DeleteCashBackCommand { CashBackId = id }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }
    //}

    //public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    //{
    //    var cashBack = await _mediator.Send(new GetCashBackDetailForEditQuery { CashBackId = id }, cancellationToken);
    //    return PartialView(cashBack);
    //}

    //[HttpPut]
    //public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditCashBackDto cashBackDto, CancellationToken cancellationToken)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return Messages.InvalidState();
    //    }
    //    try
    //    {
    //        await _mediator.Send(new EditCashBackCommand { CreateOrEditCashBackDto = cashBackDto }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }
    //}

    //[HttpPut]
    //public async Task<IActionResult> SetToAccepted(int id, CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(new ChangeCashBackRequestStatusCommand(id, WithdrawalRequestStatus.Paid.Id, DateTime.Now, null), cancellationToken);
    //    return Ok(result);
    //}
    //[HttpPut]
    //public async Task<IActionResult> SetToRejected(int id, CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(new ChangeCashBackRequestStatusCommand(id, WithdrawalRequestStatus.Cancelled.Id, DateTime.Now, null), cancellationToken);
    //    return Ok(result);
    //}
}


