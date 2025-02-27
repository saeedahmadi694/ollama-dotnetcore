using RAG.AI.Infrastructure.Configurations;

namespace RAG.AI.Presenter.API.DependencyInjection;

public static class AddConfigurations
{
    public static IServiceCollection AddAppConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BackgroundServicesConfig>(options => configuration.GetSection(BackgroundServicesConfig.Key).Bind(options));
        services.Configure<MinioConfig>(options => configuration.GetSection(MinioConfig.Key).Bind(options));
        services.Configure<JwtConfig>(options => configuration.GetSection(JwtConfig.Key).Bind(options));
        services.Configure<ApplicationConfig>(configuration.GetSection(ApplicationConfig.Key));
        services.Configure<RAGConfig>(configuration.GetSection(RAGConfig.Key));

        return services;
    }
}


