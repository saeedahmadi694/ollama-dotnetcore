using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.Comments.CreateComment;
using RAG.AI.Application.Commands.Comments.DeleteComment;
using RAG.AI.Application.Commands.Comments.EditComment;
using RAG.AI.Application.Queries.Comments.GetCommentForEdit;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.CommentViewModels;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Application.Queries.Comments.GetAllComments;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Application.Queries.Comments.ExportAllCommentsForAdmin;
using RAG.AI.Application.Queries.ContactMessages.GetContactMessageDetail;
using RAG.AI.Application.Queries.Comments.GetCommentDetail;
using RAG.AI.Application.Commands.Comments.ChangeCommentDisplayStatus;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CommentController : Controller
{
    private readonly IMediator _mediator;

    public CommentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(GetAllCommentsQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new CommentViewModel(query, items));
    }


    public async Task<IActionResult> Detail(GetCommentDetailQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return PartialView(result);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentCommand command, CancellationToken cancellationToken)
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
        var item = await _mediator.Send(new GetCommentForEditQuery(id), cancellationToken);
        return View(item);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditCommentCommand command, CancellationToken cancellationToken)
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
        Unit result = await _mediator.Send(new DeleteCommentCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeDisplayStatus(ChangeCommentDisplayStatusCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        bool result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportComments([FromQuery] ExportAllCommentsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        if (result != null && result.Length > 0)
        {
            return File(result, "text/csv", $"Comments-{DateTime.Now.ToShamsi()}.csv");
        }

        return BadRequest();
    }
}


