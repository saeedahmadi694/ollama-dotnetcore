using RAG.AI.Application.Behaviors;
using RAG.AI.Application.Commands.Sample;
using FluentValidation;
using MediatR;

namespace RAG.AI.Presenter.Web.DependencyInjection;

public static class MediatRInjection
{
    public static IServiceCollection AddConfiguredMediatR(this IServiceCollection services)
    {
        // Handlers

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(SampleCommand).Assembly);
        });

        // Generic behaviors

        // Validation behaviors
        services.AddValidatorsFromAssemblyContaining<SampleCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TranasactionBehavior<,>));

        services.AddScoped<CommandHelper>();

        return services;
    }
}



