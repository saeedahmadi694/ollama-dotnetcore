

using RAG.AI.Infrastructure.ExternalServices;

namespace RAG.AI.Infrastructure.Extentions;

public class TextExtractionStrategyFactory
{
    public static ITextExtractionStrategy GetStrategy(string fileExtension)
    {
        return fileExtension.ToLower() switch
        {
            //".pdf" => new PdfTextExtractionStrategy(),
            ".docx" => new DocxTextExtractionStrategy(),
            _ => throw new NotSupportedException($"Unsupported file type: {fileExtension}")
        };
    }
}

