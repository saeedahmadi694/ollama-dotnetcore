using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.Settings.EditSetting;
using RAG.AI.Application.Queries.Settings.GetSettingDetail;
using RAG.AI.Application.Queries.Settings.GetSettingDetailForEdit;
using Microsoft.AspNetCore.Authorization;
using RAG.AI.Application.Commands.Users.ChangePassword;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SettingController : Controller
{
    private readonly IMediator _mediator;

    public SettingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var setting = await _mediator.Send(new GetSettingDetailForEditQuery(), cancellationToken);
        return View(setting);
    }

    [HttpPut]
    public async Task<IActionResult> Index(EditSettingCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}


