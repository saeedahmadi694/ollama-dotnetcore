using Microsoft.AspNetCore.Mvc;
using Hangfire;
using System.Threading.Tasks;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class SourceController : ControllerBase
{
    private readonly IDocumentProcessingService _documentProcessingService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IVectorStore _vectorStore;
    private readonly string[] ALLOWED_EXTENSIONS = { ".doc", ".docx", ".xls", ".xlsx", ".csv", ".pdf" };

    public SourceController(
        IDocumentProcessingService documentProcessingService,
        IBackgroundJobClient backgroundJobClient,
        IVectorStore vectorStore)
    {
        _documentProcessingService = documentProcessingService;
        _backgroundJobClient = backgroundJobClient;
        _vectorStore = vectorStore;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadDocument(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!ALLOWED_EXTENSIONS.Contains(extension))
                return BadRequest("Invalid file type");

            // Save file to temporary storage
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(Path.GetTempPath(), fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Enqueue background job
            var jobId = _backgroundJobClient.Enqueue(() =>
                _documentProcessingService.ProcessDocumentAsync(filePath, extension));

            return Ok(new
            {
                message = "Document uploaded successfully and queued for processing",
                jobId = jobId,
                fileName = file.FileName
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("status/{jobId}")]
    public IActionResult GetProcessingStatus(string jobId)
    {
        var job = JobStorage.Current.GetMonitoringApi().JobDetails(jobId);

        if (job == null)
            return NotFound("Job not found");

        return Ok(new
        {
            //status = job.State,
            createdAt = job.CreatedAt,
            //completedAt = job.CompletedAt
        });
    }

    [HttpGet("DevAsync")]
    public async Task<IActionResult> DevAsync()
    {

        await _vectorStore.StoreDocuments(
            new List<string> { "test" },
            new[] { new float[] { 1, 2, 3 } });

        return Ok(new
        {
            //status = job.State,

            //completedAt = job.CompletedAt
        });
    }
}