using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Application.Commands.Sliders.ChangeDisplayStatus;
using RAG.AI.Application.Commands.Sliders.CreateSlider;
using RAG.AI.Application.Commands.Sliders.DeleteSlider;
using RAG.AI.Application.Commands.Sliders.EditSlider;
using RAG.AI.Application.Queries.Sliders.GetAllSlidersForAdmin;
using RAG.AI.Application.Queries.Sliders.GetSliderForAdminEdit;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.SliderViewModels;
using Microsoft.AspNetCore.Authorization;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SliderController : Controller
{
    private readonly IMediator _mediator;
    public SliderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task<IActionResult> Index(GetAllSlidersForAdminQuery query, CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(query, cancellationToken);
        return View(new SliderViewModel(query, items));
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSliderCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        int result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        EditSliderCommand slider = await _mediator.Send(new GetSliderForAdminEditQuery(id), cancellationToken);
        return View(slider);
    }

    [HttpPut]
    public async Task<IActionResult> Edit(EditSliderCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Unit result = await _mediator.Send(new DeleteSliderCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeDisplayStatus(ChangeSliderDisplayStatusCommand command, CancellationToken cancellationToken)
    {
        bool result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

}


