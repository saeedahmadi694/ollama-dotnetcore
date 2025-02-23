using Amazon.S3;
using Amazon.S3.Model;

namespace RAG.AI.Infrastructure.ExternalServices;

public class AwsS3Service : IAwsS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _prefix;

    public AwsS3Service(string bucketName, IAmazonS3 s3Client, string prefix = "")
    {
        _bucketName = bucketName;
        _s3Client = s3Client;
        _prefix = prefix;
    }

    public async Task UploadFileAsync(string keyName, Stream stream)
    {
        using MemoryStream memoryStream = new();
        await stream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        if (!string.IsNullOrEmpty(_prefix))
        {
            keyName = Path.Combine(_prefix, keyName).Replace("\\", "/");
        }
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName,
            InputStream = memoryStream
        };

        await _s3Client.PutObjectAsync(request);
    }

    public async Task<Stream> DownloadFileAsync(string keyName)
    {
        if (!string.IsNullOrEmpty(_prefix))
        {
            keyName = Path.Combine(_prefix, keyName).Replace("\\", "/");
        }
        GetObjectRequest request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        };

        GetObjectResponse response = await _s3Client.GetObjectAsync(request);

        return response.ResponseStream;
    }

    public string GetFileUrl(string keyName)
    {
        if (!string.IsNullOrEmpty(_prefix))
        {
            keyName = Path.Combine(_prefix, keyName).Replace("\\", "/");
        }
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = keyName,
            Expires = DateTime.Now.AddHours(1)
        };

        string url = _s3Client.GetPreSignedURL(request);

        return url;
    }

    public async Task DeleteFileAsync(string keyName)
    {
        if (!string.IsNullOrEmpty(_prefix))
        {
            keyName = Path.Combine(_prefix, keyName).Replace("\\", "/");
        }
        DeleteObjectRequest request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        };

        await _s3Client.DeleteObjectAsync(request);
    }

    public string GetBucketName()
    {
        return _bucketName;
    }
}

