﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.UploadDocument;

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
}


