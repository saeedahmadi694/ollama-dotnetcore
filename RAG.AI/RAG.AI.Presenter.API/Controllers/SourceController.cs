using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.UploadDocument;
using RAG.AI.Application.Queries.DocumentStatus;

namespace RAG.AI.Presenter.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SourceController : ControllerBase
{
    private readonly IMediator _mediator;
    public SourceController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("upload-document", Name = "UploadDocument")]
    public async Task<ActionResult<Unit>> UploadDocument([FromForm] UploadDocumentCommand command, CancellationToken clt)
    {
        var result = await _mediator.Send(command, clt);
        return Ok(result);
    }
    [HttpPost("document-status", Name = "DocumentStatus")]
    public async Task<ActionResult<Unit>> DocumentStatus([FromForm] DocumentStatusQuery command, CancellationToken clt)
    {
        var result = await _mediator.Send(command, clt);
        return Ok(result);
    }
}


