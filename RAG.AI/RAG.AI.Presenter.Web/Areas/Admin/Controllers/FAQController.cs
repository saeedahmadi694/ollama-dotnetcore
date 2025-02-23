using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.FAQs.ChangeDisplayStatus;
using RAG.AI.Application.Commands.FAQs.CreateFAQ;
using RAG.AI.Application.Commands.FAQs.DeleteFAQ;
using RAG.AI.Application.Commands.FAQs.EditFAQ;
using RAG.AI.Application.Queries.FAQs.GetFAQForEdit;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.FAQViewModels;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Application.Queries.FAQs.GetAllFAQs;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Application.Queries.FAQs.ExportAllFAQsForAdmin;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class FAQController : Controller
{
    private readonly IMediator _mediator;

    public FAQController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(GetAllFAQsQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new FAQViewModel(query, items));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFAQCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(new GetFAQForEditQuery(id), cancellationToken);
        return View(item);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditFAQCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Unit result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Unit result = await _mediator.Send(new DeleteFAQCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeDisplayStatus(ChangeFAQDisplayStatusCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        bool result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportFAQs([FromQuery] ExportAllFAQsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"FAQs-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }
}


