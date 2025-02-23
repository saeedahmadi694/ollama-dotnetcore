using RAG.AI.Presenter.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RAG.AI.Infrastructure.ExternalServices;

namespace RAG.AI.Presenter.Web.Controllers;

public class HomeController : Controller
{
    private readonly IFileSaverService _fileSaverService;

    public HomeController( IFileSaverService fileSaverService)
    {
        _fileSaverService = fileSaverService;
    }


    [HttpGet("/GetFile/{folder}/{file}")]
    public async Task<IActionResult> GetFile(string folder, string file)
    {
        var fileUrl = "";

        //if (folder.Contains(AwsFolder.Book))
        //    fileUrl = $"https://cdn.unibook.shop/{file}.png";

        fileUrl = await _fileSaverService.GetImageUrl(file, folder);


        if (fileUrl == null) return Redirect("/assets/imgs/page/notfound-book.png");

        //var response = await _httpClient.GetAsync(fileUrl);
        //if (!response.IsSuccessStatusCode)
        //{
        //    fileUrl = "/assets/imgs/page/notfound-book.png";
        //}
        return Redirect(fileUrl);
    }

}


