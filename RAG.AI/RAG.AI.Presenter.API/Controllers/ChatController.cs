using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.ChatDocument;

namespace RAG.AI.Presenter.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    public ChatController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("chat-document", Name = "ChatDocument")]
    public async Task<ActionResult<Unit>> ChatDocument([FromBody] ChatDocumentCommand command, CancellationToken clt)
    {
        var result = await _mediator.Send(command, clt);
        return Ok(result);
    }
}


