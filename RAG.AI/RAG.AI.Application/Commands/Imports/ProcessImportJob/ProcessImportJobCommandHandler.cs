using IronOcr;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Extensions.Options;
using RAG.AI.Application.Commands.StoreDocument;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.Exceptions.BaseExceptions;
using RAG.AI.Infrastructure.ExternalServices;
using RAG.AI.Infrastructure.Persistent.QueryServices;
using RAG.AI.Infrastructure.Persistent.QueryServices.QueryServiceInterfaces;

namespace RAG.AI.Application.Commands.Imports.ProcessImportJob;
public class ProcessImportJobCommandHandler : IRequestHandler<ProcessImportJobCommand, Unit>
{
    private readonly IFileSaverService _fileSaverService;
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly IImportJobQueryService _importJobQueryService;

    public ProcessImportJobCommandHandler(IMediator mediator, IFileSaverService fileSaverService, IUnitOfWork uow, IImportJobQueryService importJobQueryService)
    {
        _uow = uow;
        _mediator = mediator;
        _fileSaverService = fileSaverService;
        _importJobQueryService = importJobQueryService;
    }

    public async Task<Unit> Handle(ProcessImportJobCommand request, CancellationToken cancellationToken)
    {
        var importJob = await _uow.ImportJobRepository.GetAsync(request.JobId);
        if (importJob is null)
            throw new NotFoundException("can not find importJob");

        if (!importJob.Status.IsCreated)
            throw new Exception($"exception for job id {request.JobId} : {importJob.Status}");

        var stream = await _fileSaverService.GetImageStream(importJob.FileAddress, "/saeed");
        if (stream.Length == 0)
        {
            throw new NotFoundException("can not find pdf file");
        }

        var fileBytes = await ReadStreamAsync(stream);
        var pages = ExtractTextFromPdf(fileBytes);

        var doc = new Document(importJob.Id, "", pages, importJob.FileName, importJob.UniqueId);
        await _mediator.Send(new StoreDocumentCommand(doc), cancellationToken);

        //importJob.SetAsInProgress();
        return Unit.Value;
    }

    private List<Page> ExtractTextFromPdf(byte[] pdfBytes)
    {
        var ocr = new IronTesseract();
        ocr.Language = OcrLanguage.Persian;

        using var input = new OcrInput(pdfBytes);
        var result = ocr.Read(input);
        var pages = new List<Page>();

        var pageIndex = 1;
        foreach (var page in result.Pages)
        {
            pages.Add(new Page(page.Text, pageIndex++));
        }

        return pages;
    }

    private static async Task<byte[]> ReadStreamAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }

}