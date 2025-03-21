﻿using Microsoft.AspNetCore.Mvc;
using RAG.AI.Infrastructure.Exceptions.BaseExceptions;
using RAG.AI.Infrastructure.ExternalServices;
using System.Threading.Tasks;

namespace RAG.AI.Presenter.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DevController : ControllerBase
{
    private readonly IVectorSearchService _vectorSearchService;

    public DevController(IVectorSearchService vectorSearchService)
    {
        _vectorSearchService = vectorSearchService;
    }

    [HttpGet(Name = "TEST")]
    public async Task<IActionResult> Test()
    {
        var ss = await _vectorSearchService.GetCollection();
        throw new DuplicateException("Something is duplicated");
        return Ok("TEST SUCCESS");
    }
    [HttpGet("stream-data")]
    public async IAsyncEnumerable<string> StreamData()
    {
        for (int i = 1; i <= 10; i++)
        {
            yield return $"Data {i}\n";
            await Task.Delay(1000); // Simulate delay
        }
    }
}



