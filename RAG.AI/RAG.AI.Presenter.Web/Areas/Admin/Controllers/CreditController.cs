using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniBook.Application.Common;
using UniBook.Application.Common.Helpers;
using UniBook.Application.Features.Credits.ImportCouponsFromExcel;
using UniBook.Core.Enum;
using UniBook.Core.Errors;
using UniBook.Core.Localization;
using UniBook.UI.Areas.Admin.ViewModels.VoucherViewModels;
using static UniBook.Application.Features.CreditFeature;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[PermissionChecker(new[] { UserType.Admin })]
public class CreditController : Controller
{
    private readonly IMediator _mediator;
    public CreditController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(CreditFilterInput filterInput, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetCreditListQuery { CreditFilterInput = filterInput }, cancellationToken);
        return View(new CreditViewModel(filterInput, items));
    }

    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditCreditDto voucherDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Messages.InvalidState();
        }
        try
        {
            await _mediator.Send(new CreateCreditCommand { CreateOrEditCreditDto = voucherDto }, cancellationToken);
            return Messages.SuccessState();
        }
        catch (WebAppException e)
        {
            return Messages.FailExceptionState(e);
        }
    }


    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var voucher = await _mediator.Send(new GetCreditDetailForEditQuery { CreditId = id }, cancellationToken);
        return PartialView(voucher);
    }

    [HttpPut]
    public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditCreditDto voucherDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Messages.InvalidState();
        }
        try
        {
            await _mediator.Send(new EditCreditCommand { CreateOrEditCreditDto = voucherDto }, cancellationToken);
            return Messages.SuccessState();
        }
        catch (WebAppException e)
        {
            return Messages.FailExceptionState(e);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<ReturnedDto>> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new DeleteCreditCommand { Id = id }, cancellationToken);
            return Messages.SuccessState();
        }
        catch (WebAppException e)
        {
            return Messages.FailExceptionState(e);
        }
    }

    public IActionResult ImportCoupon()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<ActionResult<ReturnedDto>> ImportCoupon(ImportCouponsFromExcelCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(command, cancellationToken);
            return Messages.SuccessState();
        }
        catch (WebAppException e)
        {
            return Messages.FailExceptionState(e);
        }
    }


}


