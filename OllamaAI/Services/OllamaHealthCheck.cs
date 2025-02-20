using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class OllamaHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly string _ollamaEndpoint;

    public OllamaHealthCheck(IConfiguration configuration)
    {
        _httpClient = new HttpClient();
        _ollamaEndpoint = configuration.GetValue<string>("Ollama:Endpoint") ?? "http://localhost:11434";
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_ollamaEndpoint}/api/version", cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Ollama service is running");
            }
            return HealthCheckResult.Unhealthy("Ollama service is not responding");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Failed to connect to Ollama service", ex);
        }
    }
} 