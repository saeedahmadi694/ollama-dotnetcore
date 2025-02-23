using RAG.AI.Application.Commands.ContactMessages.DeleteContactMessage;
using RAG.AI.Application.Commands.WalletChargeRequests.SetWalletChargeRequestsDesription;
using RAG.AI.Application.Queries.ContactMessages.ExportAllContactMessagesForAdmin;
using RAG.AI.Application.Queries.ContactMessages.GetAllContactMessages;
using RAG.AI.Application.Queries.ContactMessages.GetContactMessageDetail;
using RAG.AI.Application.Queries.Sliders.GetSliderForAdminEdit;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.ContactMessageViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ContactMessageController : Controller
{
    private readonly IMediator _mediator;
    public ContactMessageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(GetAllContactMessagesQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new ContactMessageViewModel(query, items));
    }


    public async Task<IActionResult> Detail(GetContactMessageDetailQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return PartialView(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Unit result = await _mediator.Send(new DeleteContactMessageCommand(id), cancellationToken);
        return Ok(result);
    }
    [Produces("text/csv")]
    public async Task<IActionResult> ExportMessages([FromQuery] ExportAllContactMessagesForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        return result != null && result.Length > 0 ? File(result, "text/csv", $"messages-{DateTime.Now.ToShamsi()}.csv") : BadRequest();
    }
}


