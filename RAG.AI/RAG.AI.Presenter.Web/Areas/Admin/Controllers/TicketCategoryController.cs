using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Queries.TicketCategories.GetAllTicketCategory;
using RAG.AI.Infrastructure.Dtos.TicketCategories;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Application.Commands.Tickets.TicketCategories.CreateTicketCategory;
using RAG.AI.Application.Commands.Tickets.TicketCategories.EditTicketCategory;
using RAG.AI.Application.Commands.Tickets.TicketCategories.DeleteTicketCategory;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TicketCategoryController : Controller
{
    private readonly IMediator _mediator;
    public TicketCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }


    public async Task<ActionResult<PagedDto<TicketCategoryDto>>> Index([FromQuery] GetAllTicketCategoriesQuery query)
    {
        PagedDto<TicketCategoryDto> response = await _mediator.Send(query, new CancellationToken());
        return Ok(response);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    //public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    //{
    //    EditTicketCategoryCommand slider = await _mediator.Send(new GetTicketCategoryForAdminEditQuery(id), cancellationToken);
    //    return View(slider);
    //}

    [HttpPut]
    public async Task<IActionResult> Edit(EditTicketCategoryCommand command, CancellationToken cancellationToken)
    {

        Unit result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteTicketCategoryCommand(id), cancellationToken);
        return Ok(result);
    }

    //[HttpPatch]
    //public async Task<IActionResult> ChangeDisplayStatus(ChangeDisplayStatusCommand command, CancellationToken cancellationToken)
    //{
    //    bool result = await _mediator.Send(command, cancellationToken);
    //    return Ok(result);
    //}
}


