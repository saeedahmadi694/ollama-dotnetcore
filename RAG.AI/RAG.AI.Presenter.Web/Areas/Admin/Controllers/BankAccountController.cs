using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Queries.UserBanks.GetAllUserBanksForAdmin;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.UserBankViewModels;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Application.Queries.UserBanks.ExportAllUserBanksForAdmin;
using RAG.AI.Application.Commands.Userbanks.ChangeBankAccountStatus;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BankAccountController : Controller
{
    private readonly IMediator _mediator;

    public BankAccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(GetAllUserBanksForAdminQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new UserBankViewModel(query, items));
    }


    [HttpPatch]
    public async Task<IActionResult> ChangeBankAccountStatus(ChangeBankAccountStatusCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        bool result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportBankAccounts([FromQuery] ExportAllUserBanksForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"bankAccounts-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }

    //public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    //{
    //    var item = await _mediator.Send(new GetWalletDetailForEditQuery { WalletId = id }, cancellationToken);
    //    return PartialView(item);
    //}

    //[HttpPost]
    //public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditWalletDto command, CancellationToken cancellationToken)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return Messages.InvalidState();
    //    }
    //    try
    //    {
    //        await _mediator.Send(new EditWalletCommand
    //        {
    //            CreateOrEditWalletDto = command
    //        }, cancellationToken);
    //        return Messages.SuccessState();
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }
    //}
}


