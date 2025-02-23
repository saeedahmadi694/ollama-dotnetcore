using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniBook.Application.Common;
using UniBook.Application.Common.Helpers;
using UniBook.Core.Enum;
using UniBook.Core.Errors;
using UniBook.Core.Localization;
using UniBook.UI.Areas.Admin.ViewModels.CommunicationModels;
using static UniBook.Application.Features.CommunicationFeature;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[PermissionChecker(new[] { UserType.Admin })]

public class CommunicationController : Controller
{
    private readonly IMediator _mediator;
    public CommunicationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> SendEmail(CancellationToken cancellationToken)
    {
        return View(new SendEmailViewModel
        {
            //Vendors = await _mediator.Send(new GetAllVendorsSelectListQuery() { }, cancellationToken),
        });
    }

    [HttpPost]
    public async Task<ActionResult<ReturnedDto>> SendEmail(SendEmailViewModel vm, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(new SendEmailCommand() { SendEmailInput = vm.SendEmailInput }, cancellationToken);
            return Messages.SuccessState();
        }
        catch (WebAppException e)
        {
            return Messages.FailExceptionState(e);
        }
    }
}


