using Microsoft.AspNetCore.Mvc;
using RAG.AI.Infrastructure.Exceptions.BaseExceptions;

namespace RAG.AI.Presenter.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class DevController : ControllerBase
{
    [HttpGet(Name = "TEST")]
    public IActionResult Test()
    {
        throw new DuplicateException("Something is duplicated");
        return Ok("TEST SUCCESS");
    }

}



