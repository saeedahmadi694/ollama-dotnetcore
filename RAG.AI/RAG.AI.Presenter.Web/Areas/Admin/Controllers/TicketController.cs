using RAG.AI.Application.Commands.Tickets.CloseTicket;
using RAG.AI.Application.Commands.Tickets.CreateTicketByAdmin;
using RAG.AI.Application.Commands.Tickets.TicketDiscussions.CreateAdminTicketDiscussion;
using RAG.AI.Application.Queries.TicketCategories.GetAllTicketCategory;
using RAG.AI.Application.Queries.Tickets.ExportAllTicketsForAdmin;
using RAG.AI.Application.Queries.Tickets.GetAllTicketsForAdmin;
using RAG.AI.Application.Queries.Tickets.GetTicketForAdmin;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.TicketViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class TicketController : Controller
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }


    public async Task<IActionResult> Index(GetAllTicketsForAdminQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new TicketViewModel(query, items));
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(new GetTicketForAdminQuery(id), cancellationToken);
        return View(item);
    }
    public async Task<IActionResult> CreateTicket(int id, CancellationToken cancellationToken)
    {
        var paged = await _mediator.Send(new GetAllTicketCategoriesQuery(1, 100), cancellationToken);
        return PartialView(new CreateTicketViewModel(new CreateTicketByAdminCommand(id, "", "", 0, null), paged.Items));
    }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(CreateTicketViewModel vm, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(vm.Command, cancellationToken);
        return Ok(result);
    }
    public IActionResult Create(int id)
    {
        return PartialView(new CreateAdminTicketDiscussionCommand(User.GetUserId(), id, "", null));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAdminTicketDiscussionCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> CloseTicket(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CloseTicketCommand(id, User.GetUserId(), true), cancellationToken);
        return Ok(result);
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportTickets([FromQuery] ExportAllTicketsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"tickets-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }
}


