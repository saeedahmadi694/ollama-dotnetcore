using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAG.AI.Infrastructure.Extentions.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace RAG.AI.Presenter.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }


    public IActionResult Index()
    {
        return View();
    }



    //public async Task<ReturnedDto> GetCities(int id, CancellationToken cancellationToken)
    //{
    //    try
    //    {
    //        var items = await _mediator.Send(new GetAllCitiesSelectListByProvinceIdQuery() { ProvinceId = id }, cancellationToken);
    //        return Messages.SuccessState(items);
    //    }
    //    catch (WebAppException e)
    //    {
    //        return Messages.FailExceptionState(e);
    //    }

    //}


    //[HttpPost]
    //[Route("file-upload")]
    //public async Task<IActionResult> UploadImage(IFormFile upload)
    //{
    //    if (upload.Length <= 0) return null;

    //    var imageName = await _fileSaverService.SaveImageToServer(upload, AwsFolder.TextEditor);

    //    var url = await _fileSaverService.GetImageUrl(imageName, AwsFolder.TextEditor);

    //    return Json(new { uploaded = true, fileName = imageName, url });
    //}
}


