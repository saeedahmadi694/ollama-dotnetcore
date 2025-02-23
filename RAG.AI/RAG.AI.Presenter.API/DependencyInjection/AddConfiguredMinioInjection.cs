using Amazon;
using Amazon.S3;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.ExternalServices;

namespace RAG.AI.Presenter.API.DependencyInjection;

public static class AddConfiguredMinioInjection
{
    public static IServiceCollection AddConfiguredMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var minioOptions = new MinioConfig();
        minioOptions = configuration.GetSection(MinioConfig.Key).Get<MinioConfig>();
        var config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USEast1,
            ServiceURL = minioOptions.Connection,
        };
        var amazonS3Client = new AmazonS3Client(minioOptions.AccessKey, minioOptions.SecretKey, config);
        services.AddSingleton<IAmazonS3>(sp =>
        {
            return amazonS3Client;
        });

        services.AddSingleton<IAwsS3Service>(sp =>
        {
            var defaultBucketName = minioOptions.RootBucketName;
            return new AwsS3Service(defaultBucketName, amazonS3Client, minioOptions.Prefix);
        });

        services.AddScoped<IFileSaverService, FileSaverService>();
        return services;
    }
}



