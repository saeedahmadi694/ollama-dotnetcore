using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OllamaAI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OllamaController : ControllerBase
{
    private readonly ILogger<OllamaController> _logger;
    private readonly HttpClient _httpClient;
    private readonly string _ollamaBaseUrl = "http://localhost:11434";

    public OllamaController(ILogger<OllamaController> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatMessage message)
    {
        try
        {
            var ollamaRequest = new
            {
                model = "deepseek-r1:1.5b",
                messages = new[]
                {
                    new { role = "user", content = message.Message }
                }
            };

            var response = await SendOllamaRequest("/api/chat", ollamaRequest);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in chat endpoint");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateRequest request)
    {
        try
        {
            var response = await SendOllamaRequest("/api/generate", request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in generate endpoint");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpPost("embeddings")]
    public async Task<IActionResult> Embeddings([FromBody] EmbeddingsRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_ollamaBaseUrl}/api/embeddings", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<EmbeddingsResponse>();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in embeddings endpoint");
            return StatusCode(500, "An error occurred while processing your request");
        }
    }

    [HttpGet("tags")]
    public async Task<IActionResult> ListModels()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_ollamaBaseUrl}/api/tags");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ModelsResponse>();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing models");
            return StatusCode(500, "An error occurred while listing models");
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateModel([FromBody] CreateModelRequest request)
    {
        try
        {
            var response = await SendOllamaRequest("/api/create", request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating model");
            return StatusCode(500, "An error occurred while creating the model");
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteModel([FromBody] DeleteModelRequest request)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{_ollamaBaseUrl}/api/delete");
            response.EnsureSuccessStatusCode();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting model");
            return StatusCode(500, "An error occurred while deleting the model");
        }
    }

    [HttpPost("copy")]
    public async Task<IActionResult> CopyModel([FromBody] CopyModelRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_ollamaBaseUrl}/api/copy", request);
            response.EnsureSuccessStatusCode();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying model");
            return StatusCode(500, "An error occurred while copying the model");
        }
    }

    [HttpPost("pull")]
    public async Task<IActionResult> PullModel([FromBody] PullModelRequest request)
    {
        try
        {
            var response = await SendOllamaRequest("/api/pull", request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pulling model");
            return StatusCode(500, "An error occurred while pulling the model");
        }
    }

    [HttpPost("push")]
    public async Task<IActionResult> PushModel([FromBody] PushModelRequest request)
    {
        try
        {
            var response = await SendOllamaRequest("/api/push", request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pushing model");
            return StatusCode(500, "An error occurred while pushing the model");
        }
    }

    [HttpGet("version")]
    public async Task<IActionResult> GetVersion()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_ollamaBaseUrl}/api/version");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<VersionResponse>();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting version");
            return StatusCode(500, "An error occurred while getting version");
        }
    }

    private async Task<string> SendOllamaRequest<T>(string endpoint, T request)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(request),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync($"{_ollamaBaseUrl}{endpoint}", content);
        response.EnsureSuccessStatusCode();

        var responseStream = await response.Content.ReadAsStreamAsync();
        var reader = new StreamReader(responseStream);
        var fullResponse = new StringBuilder();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;
            fullResponse.AppendLine(line);
        }

        return fullResponse.ToString();
    }
}

// Request/Response Models
public class ChatMessage
{
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}

public class GenerateRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }

    [JsonPropertyName("system")]
    public string System { get; set; }

    [JsonPropertyName("template")]
    public string Template { get; set; }

    [JsonPropertyName("context")]
    public List<int> Context { get; set; }

    [JsonPropertyName("options")]
    public Dictionary<string, object> Options { get; set; }
}

public class EmbeddingsRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; }
}

public class EmbeddingsResponse
{
    [JsonPropertyName("embedding")]
    public List<float> Embedding { get; set; }
}

public class CreateModelRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("modelfile")]
    public string Modelfile { get; set; }
}

public class DeleteModelRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

public class CopyModelRequest
{
    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("destination")]
    public string Destination { get; set; }
}

public class PullModelRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("insecure")]
    public bool Insecure { get; set; }
}

public class PushModelRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("insecure")]
    public bool Insecure { get; set; }
}

public class ModelsResponse
{
    [JsonPropertyName("models")]
    public List<ModelInfo> Models { get; set; }
}

public class ModelInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("modified_at")]
    public DateTime ModifiedAt { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("digest")]
    public string Digest { get; set; }
}

public class VersionResponse
{
    [JsonPropertyName("version")]
    public string Version { get; set; }
}

public class OllamaResponse
{
    [JsonPropertyName("message")]
    public OllamaMessage Message { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }
}

public class OllamaMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}
