using RAG.AI.Application.Commands.GoldTradeRequests.SetGoldTradeRequestDesription;
using RAG.AI.Application.Queries.GoldTradeRequests.ExportAllBuyRequestsForAdmin;
using RAG.AI.Application.Queries.GoldTradeRequests.ExportAllSellRequestsForAdmin;
using RAG.AI.Application.Queries.GoldTradeRequests.GetAllGoldTradeRequestsForAdmin;
using RAG.AI.Application.Queries.GoldTradeRequests.GetGoldTradeRequestForAdmin;
using RAG.AI.Domain.Aggregates.Common;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.GoldTradeRequestViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class GoldTradeRequestController : Controller
{
    private readonly IMediator _mediator;
    public GoldTradeRequestController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> SellRequest(GetAllGoldTradeRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = TransactionType.Sell.Id };
        var items = await _mediator.Send(query, cancellationToken);
        return View(new GoldTradeRequestViewModel(query, items));
    }
    public async Task<IActionResult> BuyRequest(GetAllGoldTradeRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = TransactionType.Buy.Id };
        var items = await _mediator.Send(query, cancellationToken);
        return View(new GoldTradeRequestViewModel(query, items));
    }

    public async Task<IActionResult> SetDescription(GetGoldTradeRequestForAdminQuery query, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(query, cancellationToken);
        return PartialView(new SetGoldTradeRequestDesriptionCommand(result.Id, result.Description));
    }

    [HttpPatch]
    public async Task<IActionResult> SetDescription(SetGoldTradeRequestDesriptionCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    [Produces("text/csv")]
    public async Task<IActionResult> ExportSellRequests([FromQuery] ExportAllSellRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = TransactionType.Sell.Id };
        var result = await _mediator.Send(query, cancellationToken);
        return result != null && result.Length > 0
            ? File(result, "text/csv", $"goldTradeRequests-{DateTime.Now.ToShamsi()}.csv")
            : (IActionResult)BadRequest();
    }


    [Produces("text/csv")]
    public async Task<IActionResult> ExportBuyRequests([FromQuery] ExportAllBuyRequestsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = TransactionType.Buy.Id };
        var result = await _mediator.Send(query, cancellationToken);
        return result != null && result.Length > 0
            ? File(result, "text/csv", $"goldTradeRequests-{DateTime.Now.ToShamsi()}.csv")
            : (IActionResult)BadRequest();
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


