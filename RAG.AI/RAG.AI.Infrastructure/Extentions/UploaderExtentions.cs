using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace RAG.AI.Infrastructure.Extentions;

public static class UploaderExtentions
{

    public static string CheckFileAndCreateAddress(IFormFile file)
    {
        var extention = file.FileName[file.FileName.LastIndexOf(".")..];
        var guid = Guid.NewGuid() + extention;
        return guid;
    }

    public static string GetMimeTypeForFileExtension(string filePath)
    {
        const string DefaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(filePath, out string contentType))
        {
            contentType = DefaultContentType;
        }

        return contentType;
    }
}

