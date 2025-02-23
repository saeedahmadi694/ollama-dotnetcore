using Amazon.Auth.AccessControlPolicy;
using RAG.AI.Application.Commands.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NPOI.Util.Collections;

namespace RAG.AI.Presenter.Web.Controllers;


public class AccountController : Controller
{
    private readonly IMediator _mediator;
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region For Login     


    [HttpGet("/Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("/Login")]
    public async Task<IActionResult> Login(LogInCommand command, CancellationToken cancellationToken)
    {
        var cookie = await _mediator.Send(command, cancellationToken);
        await HttpContext.SignInAsync(cookie.Item1, cookie.Item2);
        return Ok();
    }



    #endregion

    [HttpGet("/Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}

