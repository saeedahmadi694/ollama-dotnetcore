using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Queries.Wallets.GetAllWalletsForAdmin;
using RAG.AI.Infrastructure.Extentions.Attributes;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.SliderViewModels;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Application.Commands.Wallets.ChangeWalletStatus;
using RAG.AI.Application.Queries.Wallets.ExportAllWalletsForAdmin;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.WalletViewModels;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class WalletController : Controller
{
    private readonly IMediator _mediator;

    public WalletController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(GetAllWalletsForAdminQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send( query, cancellationToken);
        return View(new WalletViewModel(query, items));
    }


    [HttpPatch]
    public async Task<IActionResult> ChangeWalletStatus(ChangeWalletStatusCommand command, CancellationToken cancellationToken)
    {
        bool result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    [Produces("text/csv")]
    public async Task<IActionResult> ExportWallets([FromQuery] ExportAllWalletsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"wallets-{DateTime.Now.ToShamsi()}.csv");
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


