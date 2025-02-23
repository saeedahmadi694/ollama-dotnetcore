using DocumentFormat.OpenXml.Packaging;
using OfficeOpenXml;
using System.Text;
using UglyToad.PdfPig;
using Hangfire;

public interface IDocumentProcessingService
{
    Task ProcessDocumentAsync(string filePath, string extension);
}

public class DocumentProcessingService : IDocumentProcessingService
{
    private readonly IVectorStore _vectorStore;
    private readonly IEmbeddingService _embeddingService;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public DocumentProcessingService(
        IVectorStore vectorStore,
        IEmbeddingService embeddingService,
        IBackgroundJobClient backgroundJobClient)
    {
        _vectorStore = vectorStore;
        _embeddingService = embeddingService;
        _backgroundJobClient = backgroundJobClient;
    }

    public async Task ProcessDocumentAsync(string filePath, string extension)
    {
        try
        {

            await ExtractContentAsync(filePath, extension);


            //await GenerateEmbeddingsAsync(filePath);

            // Enqueue content extraction job
            //var jobId = _backgroundJobClient.Enqueue<IDocumentProcessingService>(
            //    service => ExtractContentAsync(filePath, extension));

            //// Continue with embedding generation when content extraction is complete
            //_backgroundJobClient.ContinueJobWith<IDocumentProcessingService>(
            //    jobId,
            //    service => GenerateEmbeddingsAsync(filePath),
            //    JobContinuationOptions.OnlyOnSucceededState);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error processing document: {ex.Message}", ex);
        }
    }

    [Queue("documents")]
    public async Task ExtractContentAsync(string filePath, string extension)
    {
        try
        {
            string content = await ExtractContent(filePath, extension);

            // Save extracted content temporarily
            await File.WriteAllTextAsync(
                Path.ChangeExtension(filePath, ".txt"),
                content);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error extracting content: {ex.Message}", ex);
        }
    }

    [Queue("embeddings")]
    public async Task GenerateEmbeddingsAsync(string filePath)
    {
        try
        {
            // Read extracted content
            var contentPath = Path.ChangeExtension(filePath, ".txt");
            var content = await File.ReadAllTextAsync(contentPath);

            // Generate embedding
            var embedding = await _embeddingService.GenerateEmbedding(content);

            // Store in vector database
            await _vectorStore.StoreDocuments(
                new List<string> { content },
                new[] { embedding });

            // Cleanup temporary files
            File.Delete(filePath);
            File.Delete(contentPath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating embeddings: {ex.Message}", ex);
        }
    }

    private async Task<string> ExtractContent(string filePath, string extension)
    {
        return extension.ToLower() switch
        {
            ".pdf" => ExtractFromPdf(filePath),
            ".doc" or ".docx" => ExtractFromWord(filePath),
            ".xls" or ".xlsx" => ExtractFromExcel(filePath),
            ".csv" => await ExtractFromCsv(filePath),
            _ => throw new NotSupportedException($"File type {extension} is not supported"),
        };
    }

    private string ExtractFromPdf(string filePath)
    {
        using var document = PdfDocument.Open(filePath);
        var text = new StringBuilder();
        foreach (var page in document.GetPages())
        {
            text.AppendLine(page.Text);
        }
        return text.ToString();
    }

    private string ExtractFromWord(string filePath)
    {
        using var document = WordprocessingDocument.Open(filePath, false);
        var body = document.MainDocumentPart.Document.Body;
        return body.InnerText;
    }

    private string ExtractFromExcel(string filePath)
    {
        using var package = new ExcelPackage(new FileInfo(filePath));
        var text = new StringBuilder();
        foreach (var worksheet in package.Workbook.Worksheets)
        {
            text.AppendLine($"Sheet: {worksheet.Name}");
            var dimension = worksheet.Dimension;
            if (dimension != null)
            {
                for (int row = 1; row <= dimension.Rows; row++)
                {
                    for (int col = 1; col <= dimension.Columns; col++)
                    {
                        text.Append(worksheet.Cells[row, col].Text + "\t");
                    }
                    text.AppendLine();
                }
            }
        }
        return text.ToString();
    }

    private async Task<string> ExtractFromCsv(string filePath)
    {
        var text = new StringBuilder();
        using (var reader = new StreamReader(filePath, Encoding.UTF8))
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                text.AppendLine(line);
            }
        }
        return text.ToString();
    }
}