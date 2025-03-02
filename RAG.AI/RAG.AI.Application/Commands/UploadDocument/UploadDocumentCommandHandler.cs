﻿using Microsoft.AspNetCore.Http;
using RAG.AI.Application.Commands.UploadDocument;
using RAG.AI.Domain.Aggregates.ImportAggregate;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.ExternalServices;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileSaverService _fileSaverService;
    public UploadDocumentCommandHandler(IUnitOfWork unitOfWork, IFileSaverService fileSaverService)
    {
        _unitOfWork = unitOfWork;
        _fileSaverService = fileSaverService;
    }

    public async Task<Guid> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        string fileAddress = await UploadImage(request.File);

        var newJob = new ImportJob(1, fileAddress, request.File.FileName);
        await _unitOfWork.ImportJobRepository.InsertAsync(newJob);
        newJob.SetAsCreated();
        await _unitOfWork.SaveChangesAsync();
        return newJob.UniqueId;
    }

    private async Task<string> UploadImage(IFormFile request)
    {
        return request == null
            ? throw new Exception("File_Empty_Error")
            : request.Length > 1024000 * 4
            ? throw new Exception("File_Size_Limit_Error")
            : await _fileSaverService.SaveImageToServer(request, "/saeed");
    }

}
