using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RAG.AI.Infrastructure.ExternalServices;

public class FileSaverService : IFileSaverService
{
    private readonly IAwsS3Service _s3Client;

    public FileSaverService(IAwsS3Service s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task DeleteImageFromServer(string file, string folder = "")
    {
        var path = Path.Combine(folder, file).Replace("\\", "/");
        await _s3Client.DeleteFileAsync(path);
    }

    public async Task<Stream> GetImageStream(string file, string folder = "")
    {
        var path = Path.Combine(folder, file).Replace("\\", "/");
        return await _s3Client.DownloadFileAsync(path);
    }

    public async Task<string> GetImageUrl(string file, string folder = "")
    {
        var path = Path.Combine(folder, file).Replace("\\", "/");

        return await Task.FromResult(_s3Client.GetFileUrl(path));
    }

    public async Task<string> SaveImageToServer(IFormFile file, string folder = "")
    {

        var filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
        var path = Path.Combine(folder, filename).Replace("\\", "/");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
        await _s3Client.UploadFileAsync(path, ms);
        return filename;
    }
}

