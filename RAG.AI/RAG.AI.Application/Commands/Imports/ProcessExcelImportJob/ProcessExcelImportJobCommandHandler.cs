using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Extensions.Options;
using RAG.AI.Application.Commands.StoreDocument;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.Exceptions.BaseExceptions;
using RAG.AI.Infrastructure.ExternalServices;

namespace RAG.AI.Application.Commands.Imports.ProcessExcelImportJob;
public class ProcessExcelImportJobCommandHandler : IRequestHandler<ProcessExcelImportJobCommand, Unit>
{
    private readonly MinioConfig _minioConfig;
    private readonly IFileSaverService _fileSaverService;
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public ProcessExcelImportJobCommandHandler(IOptions<MinioConfig> minioConfig, IUnitOfWork uow, IMediator mediator, ILogger logger, IFileSaverService fileSaverService)
    {
        _minioConfig = minioConfig.Value;
        _uow = uow;
        _mediator = mediator;
        _logger = logger;
        _fileSaverService = fileSaverService;
    }

    public async Task<Unit> Handle(ProcessExcelImportJobCommand request, CancellationToken cancellationToken)
    {
        var importJob = await _uow.ImportJobRepository.GetAsync(request.JobId);
        if (importJob is null)
            throw new NotFoundException("can not find importJob");

        if (!importJob.Status.IsCreated)
            throw new Exception($"exception for job id {request.JobId} : {importJob.Status}");


        var stream = await _fileSaverService.GetImageStream(importJob.FileAddress, "/saeed");
        if (stream.Length == 0)
        {
            throw new NotFoundException("can not find excel file");
        }


        var fileBytes = await ReadStreamAsync(stream);

        using var pdfReader = new PdfReader(new MemoryStream(fileBytes));

        var pages = ExtractTextFromPdf(pdfReader);

        var doc = new Document("", pages, importJob.FileName, importJob.UniqueId);

        await _mediator.Send(new StoreDocumentCommand(doc), cancellationToken);

        return Unit.Value;
    }

    private List<Page> ExtractTextFromPdf(PdfReader pdfDocument)
    {
        var pages = new List<Page>();

        for (int i = 1; i <= pdfDocument.NumberOfPages; i++)
        {
            string pageText = PdfTextExtractor.GetTextFromPage(pdfDocument, i) ?? string.Empty;
            pages.Add(new Page(FixPersianText(pageText), i));
        }

        return pages;
    }


    private static async Task<byte[]> ReadStreamAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }



    private static string FixPersianText(string text)
    {
        char[] charArray = text.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

}