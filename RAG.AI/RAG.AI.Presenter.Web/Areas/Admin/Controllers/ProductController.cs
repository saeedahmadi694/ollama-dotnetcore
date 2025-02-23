using RAG.AI.Application.Commands.Products.Bullions.CreateBullion;
using RAG.AI.Application.Commands.Products.Bullions.EditBullion;
using RAG.AI.Application.Commands.Products.ChangeProductStatus;
using RAG.AI.Application.Commands.Products.DeleteProduct;
using RAG.AI.Application.Commands.Products.Frames.CreateFrame;
using RAG.AI.Application.Commands.Products.Frames.EditFrame;
using RAG.AI.Application.Queries.Products.ExportAllProductsForAdmin;
using RAG.AI.Application.Queries.Products.GetAllProduct;
using RAG.AI.Application.Queries.Products.GetFrameForEdit;
using RAG.AI.Domain.Aggregates.ProductAggregate;
using RAG.AI.Infrastructure.Extentions;
using RAG.AI.Presenter.Web.Areas.Admin.ViewModels.ProductViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController : Controller
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Frames(GetAllProductsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = ProductType.Frame.Id };
        var items = await _mediator.Send(query, cancellationToken);
        return View(new ProductViewModel(query, items));
    }

    public async Task<IActionResult> Bullions(GetAllProductsForAdminQuery query, CancellationToken cancellationToken)
    {
        query = query with { Type = ProductType.Bullion.Id };
        var items = await _mediator.Send(query, cancellationToken);
        return View(new ProductViewModel(query, items));
    }

    #region Frames
    public IActionResult CreateFrame()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateFrame(CreateFrameCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    public async Task<IActionResult> EditFrame(int id, CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(new GetFrameForEditQuery(id), cancellationToken);
        return View(item);
    }

    [HttpPut]
    public async Task<IActionResult> EditFrame(EditFrameCommand command, CancellationToken cancellationToken)
    {
        Unit result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    #endregion



    //#region Coins
    //public IActionResult CreateCoin()
    //{
    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> CreateCoin(CreateCoinCommand command, CancellationToken cancellationToken)
    //{
    //    var result = await _mediator.Send(command, cancellationToken);
    //    return Ok(result);
    //}


    //public async Task<IActionResult> EditCoin(int id, CancellationToken cancellationToken)
    //{
    //    var item = await _mediator.Send(new GetCoinForEditQuery(id), cancellationToken);
    //    return View(item);
    //}

    //[HttpPut]
    //public async Task<IActionResult> EditCoin(EditCoinCommand command, CancellationToken cancellationToken)
    //{
    //    Unit result = await _mediator.Send(command, cancellationToken);
    //    return Ok(result);
    //}
    //#endregion



    #region Bullions
    public IActionResult CreateBullion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateBullion(CreateBullionCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }


    public async Task<IActionResult> EditBullion(int id, CancellationToken cancellationToken)
    {
        var item = await _mediator.Send(new GetBullionForEditQuery(id), cancellationToken);
        return View(item);
    }

    [HttpPut]
    public async Task<IActionResult> EditBullion(EditBullionCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Unit result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
    #endregion



    [HttpDelete]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Unit result = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> ChangeDisplayStatus(ChangeProductStatusCommand command, CancellationToken cancellationToken)
    {
        bool result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Produces("text/csv")]
    public async Task<IActionResult> ExportFrames([FromQuery] ExportAllProductsForAdminQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, new CancellationToken());
        return result != null && result.Length > 0 ? File(result, "text/csv", $"frames-{DateTime.Now.ToShamsi()}.csv") : BadRequest();
    }
}


