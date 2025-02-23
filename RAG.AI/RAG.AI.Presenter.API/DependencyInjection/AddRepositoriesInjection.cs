using RAG.AI.Domain.SeedWork;
using RAG.AI.Domain.SeedWork.RepositoryInterfaces;
using RAG.AI.Infrastructure.Persistent;
using RAG.AI.Infrastructure.Persistent.Repositories;

namespace RAG.AI.Presenter.API.DependencyInjection;

public static class AddRepositoriesInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddSingleton<IDbConnectionFactory, SqlServerDbConnectionFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IScheduledJobLogRepository, ScheduledJobLogRepository>();


        return services;
    }
}



