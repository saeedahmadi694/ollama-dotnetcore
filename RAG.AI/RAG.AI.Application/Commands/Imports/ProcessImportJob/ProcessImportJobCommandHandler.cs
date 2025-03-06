using RAG.AI.Application.Commands.StoreDocument;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.Exceptions.BaseExceptions;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Infrastructure.ExternalServices;
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
        {
            throw new NotFoundException("can not find importJob");
        }

        if (!importJob.Status.IsCreated)
        {
            throw new Exception($"exception for job id {request.JobId} : {importJob.Status}");
        }

        var fileExtension = System.IO.Path.GetExtension(importJob.FileName);
        var stream = await _fileSaverService.GetImageStream(importJob.FileAddress, "/saeed");

        if (stream.Length == 0)
        {
            throw new NotFoundException("can not find the file");
        }

        var fileBytes = await ReadStreamAsync(stream);

        // Get the appropriate text extraction strategy
        var textExtractionStrategy = TextExtractionStrategyFactory.GetStrategy(fileExtension);
        var pages = textExtractionStrategy.ExtractText(fileBytes);

        var doc = new Document(importJob.Id, "", pages, importJob.FileName, importJob.UniqueId);
        await _mediator.Send(new StoreDocumentCommand(doc), cancellationToken);

        return Unit.Value;
    }

    private static async Task<byte[]> ReadStreamAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}
