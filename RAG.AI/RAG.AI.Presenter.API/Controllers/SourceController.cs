using DocumentFormat.OpenXml.Packaging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.UploadDocument;
using RAG.AI.Application.Queries.DocumentStatus;
using System.Text;

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

    [HttpPost("extract-text")]
    public async Task<IActionResult> ExtractTextFromDocx(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please upload a valid DOCX file.");
        }

        // Save the uploaded file to a temporary location
        var filePath = Path.GetTempFileName();
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        StringBuilder text = new StringBuilder();

        using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
        {
            var body = doc.MainDocumentPart.Document.Body;
            text.Append(body.InnerText);
        }

        return Ok(new { text = text.ToString() });
    }
}


