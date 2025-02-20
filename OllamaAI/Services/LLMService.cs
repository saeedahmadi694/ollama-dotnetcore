using OpenAI;
using OpenAI.Chat;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILLMService
{
    Task<string> GenerateResponse(string query, string context);
}

public class LLMService : ILLMService
{
    private readonly OpenAIClient _openAIClient;
    private readonly IConfiguration _configuration;

    public LLMService(IConfiguration configuration)
    {
        _configuration = configuration;
        _openAIClient = new OpenAIClient(new OpenAIAuthentication(_configuration["OpenAI:ApiKey"]));
    }

    public async Task<string> GenerateResponse(string query, string context)
    {
        try
        {
            var chatMessages = new List<ChatMessage>
            {
                new ChatMessage(ChatRole.System, 
                    "You are a helpful assistant. Use the provided context to answer questions. " +
                    "If you cannot find the answer in the context, say so."),
                new ChatMessage(ChatRole.User, 
                    $"Context: {context}\n\nQuestion: {query}\n\nAnswer based on the context provided:"),
            };

            var chatRequest = new ChatRequest(chatMessages, model: "gpt-3.5-turbo");
            var response = await _openAIClient.ChatEndpoint.GetCompletionAsync(chatRequest);

            return response.FirstChoice.Message.Content;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating LLM response: {ex.Message}", ex);
        }
    }
} 