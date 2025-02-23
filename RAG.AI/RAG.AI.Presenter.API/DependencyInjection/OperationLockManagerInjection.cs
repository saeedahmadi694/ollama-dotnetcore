namespace RAG.AI.Presenter.API.DependencyInjection;

public static class OperationLockManagerInjection
{

    // It uses Redis Cache setting, so should be called after adding the StackExchangeRedisCache

    public static IServiceCollection AddOperationLockManager(this IServiceCollection services)
    {
        //services.AddSingleton<IOperationLockManager, OperationLockManager>();

        return services;
    }
}



