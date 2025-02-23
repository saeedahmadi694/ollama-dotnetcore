using Amazon.Util;
using RAG.AI.Infrastructure.Configurations;
using RAG.AI.Infrastructure.Dtos.Kyc.Constant;

namespace RAG.AI.Presenter.Web.DependencyInjection;

public static class AddConfigurations
{
    public static IServiceCollection AddAppConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        //services.Configure<MinioConfig>(options => configuration.GetSection(MinioConfig.Key).Bind(options));
        services.Configure<BackgroundServicesConfig>(options => configuration.GetSection(BackgroundServicesConfig.Key).Bind(options));
        services.Configure<MinioConfig>(options => configuration.GetSection(MinioConfig.Key).Bind(options));
        services.Configure<KycClientOptions>(options => configuration.GetSection(nameof(KycClientOptions)).Bind(options));
        services.Configure<ApplicationConfig>(configuration.GetSection(ApplicationConfig.Key));
        services.Configure<KavenegarConfig>(configuration.GetSection(KavenegarConfig.Key));

        return services;
    }
}


