namespace RAG.AI.Presenter.API.DependencyInjection;

public static class AddConfiguredHostedServicesInjection
{
    public static IServiceCollection AddConfiguredHostedServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddHostedService<SampleBackgroundService>();
        return services;
    }
}



