using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RAG.AI.Infrastructure.ExternalServices;

public interface IAwsS3Service
{
    Task UploadFileAsync(string keyName, Stream stream);
    Task<Stream> DownloadFileAsync(string keyName);
    string GetFileUrl(string keyName);
    Task DeleteFileAsync(string keyName);
    string GetBucketName();
}

