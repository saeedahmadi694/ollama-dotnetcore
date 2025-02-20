using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;


public class OllamaEmbeddingService : IEmbeddingService
{
    private readonly HttpClient _httpClient;
    private readonly string _ollamaEndpoint;
    private readonly string _modelName;

    public OllamaEmbeddingService(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _ollamaEndpoint = configuration.GetValue<string>("Ollama:Endpoint") ?? "http://localhost:11434";
        _modelName = "deepseek-r1:1.5b";  // or your preferred Deepseek model
    }

    public async Task<float[]> GenerateEmbedding(string text)
    {
        try
        {
            var request = new
            {
                model = _modelName,
                prompt = text,
                options = new { temperature = 0.0 }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_ollamaEndpoint}/api/embeddings", content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OllamaEmbeddingResponse>(jsonResponse);

            return result?.Embedding ?? throw new Exception("No embedding generated");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating embedding with Ollama: {ex.Message}", ex);
        }
    }
}

public class OllamaEmbeddingResponse
{
    [JsonPropertyName("embedding")]
    public float[] Embedding { get; set; }
} 