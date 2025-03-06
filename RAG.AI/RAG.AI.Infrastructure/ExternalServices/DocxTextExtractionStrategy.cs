
namespace RAG.AI.Infrastructure.ExternalServices;

using DocumentFormat.OpenXml.Packaging;
using RAG.AI.Infrastructure.Dtos.Common;
using System.Text;

public class DocxTextExtractionStrategy : ITextExtractionStrategy
{
    public List<Page> ExtractText(byte[] fileBytes)
    {
        using var memoryStream = new MemoryStream(fileBytes);
        using var doc = WordprocessingDocument.Open(memoryStream, false);

        var text = new StringBuilder();
        var body = doc.MainDocumentPart.Document.Body;
        text.Append(body.InnerText);
        var pages = text.ToString().Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select((pageText, index) => new Page(pageText, index + 1))
                                   .ToList();
        return pages;
    }
}
