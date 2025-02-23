using RAG.AI.Application.Commands.Operations.CreateCashDepositOperation;
using RAG.AI.Application.Commands.Operations.CreateCashWithdrawalOperation;
using RAG.AI.Application.Commands.Operations.CreateGoldDepositOperation;
using RAG.AI.Application.Commands.Operations.CreateGoldWithdrawalOperation;
using RAG.AI.Application.Queries.ContactMessages.GetContactMessageDetail;
using RAG.AI.Application.Queries.Operations.GetAllOperationsForAdmin;
using RAG.AI.Application.Queries.Operations.ExportAllOperationsForAdmin;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.OperationViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Queries.Operations.GetOperationDetail;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OperationController : Controller
{
    private readonly IMediator _mediator;

    public OperationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(GetAllOperationsForAdminQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new OperationViewModel(query, items));
    }


    [Produces("text/csv")]
    public async Task<IActionResult> ExportOperations([FromQuery] ExportAllOperationsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"operations-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }

    public async Task<IActionResult> Detail(int id, CancellationToken cancellationToken)
    {
        var operation = await _mediator.Send(new GetOperationDetailQuery(id), cancellationToken);
        return PartialView(operation);
    }

    public IActionResult CreateCashDepositOperation(int userId, int walletId)
    {
        return PartialView(new CreateCashDepositOperationCommand("", null, 0, walletId, userId, null));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCashDepositOperation(CreateCashDepositOperationCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    public IActionResult CreateCashWithdrawalOperation(int userId, int walletId)
    {
        return PartialView(new CreateCashWithdrawalOperationCommand("", null, 0, walletId, userId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCashWithdrawalOperation(CreateCashWithdrawalOperationCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }



    public IActionResult CreateGoldDepositOperation(int userId, int walletId)
    {
        return PartialView(new CreateGoldDepositOperationCommand("", null, 0, walletId, userId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoldDepositOperation(CreateGoldDepositOperationCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }



    public IActionResult CreateGoldWithdrawalOperation(int userId, int walletId)
    {
        return PartialView(new CreateGoldWithdrawalOperationCommand("", null, 0, walletId, userId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateGoldWithdrawalOperation(CreateGoldWithdrawalOperationCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}


