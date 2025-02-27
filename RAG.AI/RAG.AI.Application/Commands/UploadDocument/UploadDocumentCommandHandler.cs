using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Http;
using RAG.AI.Application.Commands.StoreDocument;
using RAG.AI.Application.Commands.UploadDocument;
using RAG.AI.Domain.SeedWork;
using RAG.AI.Infrastructure.Dtos.Common;
using RAG.AI.Infrastructure.ExternalServices;

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileSaverService _fileSaverService;
    private readonly IMediator _mediator;

    public UploadDocumentCommandHandler(IUnitOfWork unitOfWork, IFileSaverService fileSaverService, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _fileSaverService = fileSaverService;
        _mediator = mediator;
    }

    public async Task<string> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        //string guid = await UploadImage(request.File);

        using Stream stream = request.File.OpenReadStream();
        var fileBytes = await ReadStreamAsync(stream);

        using var pdfReader = new PdfReader(new MemoryStream(fileBytes));

        var pages = ExtractTextFromPdf(pdfReader);

        var doc = new Document("", pages, System.IO.Path.GetFileName(request.File.FileName), Guid.NewGuid());

        await _mediator.Send(new StoreDocumentCommand(doc), cancellationToken);

        return "";
    }

    private List<Page> ExtractTextFromPdf(PdfReader pdfDocument)
    {
        var pages = new List<Page>();

        for (int i = 1; i <= pdfDocument.NumberOfPages; i++)
        {
            string pageText = PdfTextExtractor.GetTextFromPage(pdfDocument,i) ?? string.Empty;
            pages.Add(new Page(FixPersianText(pageText), i));
        }

        return pages;
    }

    private async Task<string> UploadImage(IFormFile request)
    {
        if (request == null)
        {
            throw new Exception("File_Empty_Error");
        }

        return request.Length > 1024000 * 4
            ? throw new Exception("File_Size_Limit_Error")
            : await _fileSaverService.SaveImageToServer(request, "/saeed");
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
