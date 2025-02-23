using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniBook.Application.Common;
using UniBook.Application.Common.Helpers;
using UniBook.Core.Enum;
using UniBook.Core.Errors;
using UniBook.Core.Localization;
using UniBook.UI.Areas.Admin.ViewModels.PaymentViewModels;
using static UniBook.Application.Features.PaymentFeature;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[PermissionChecker(new[] { UserType.Admin })]
public class PaymentController : Controller
{
    private readonly IMediator _mediator;
    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(PaymentFilterInput filterInput, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetPaymentListQuery { PaymentFilterInput = filterInput }, cancellationToken);
        return View(new PaymentViewModel { Filter = filterInput, PagingHandler = items });
    }

    public IActionResult Create()
    {
        return PartialView();
    }

    [HttpPost]
    public async Task<ActionResult<ReturnedDto>> Create(CreateOrEditPaymentDto paymentDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Messages.InvalidState();
        }
        try
        {
            await _mediator.Send(new CreatePaymentCommand { CreateOrEditPaymentDto = paymentDto }, cancellationToken);
            return Messages.SuccessState();
        }
        catch (WebAppException e)
        {
            return Messages.FailExceptionState(e);
        }
    }


    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var payment = await _mediator.Send(new GetPaymentDetailForEditQuery { PaymentId = id }, cancellationToken);
        return PartialView(payment);
    }

    [HttpPut]
    public async Task<ActionResult<ReturnedDto>> Edit(CreateOrEditPaymentDto paymentDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Messages.InvalidState();
        }
        try
        {
            await _mediator.Send(new EditPaymentCommand { CreateOrEditPaymentDto = paymentDto }, cancellationToken);
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
            await _mediator.Send(new DeletePaymentCommand { PaymentId = id }, cancellationToken);
            return Messages.SuccessState();
        }
        catch (WebAppException e)
        {
            return Messages.FailExceptionState(e);
        }
    }

}


