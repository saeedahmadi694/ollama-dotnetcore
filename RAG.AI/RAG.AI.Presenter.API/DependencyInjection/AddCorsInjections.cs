using BookHouse.Core.Utilities;
using RAG.AI.Presenter.API.DependencyInjection;

namespace RAG.AI.Presenter.API.DependencyInjection;

public static class AddCorsInjections
{
    public static IServiceCollection AddCors(this IServiceCollection services, ConfigurationManager configuration, IWebHostEnvironment env, string CorsPolicy)
    {
        if (!env.IsProduction())
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            return services;
        }

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, builder =>
            {
                //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                builder
                    .WithOrigins(
                      // App: CorsOrigins in appsettings.json can contain more than one address separated by comma.
                      configuration["App:CorsOrigins"]
                           .Split(",", StringSplitOptions.RemoveEmptyEntries)
                           .Select(o => o.RemovePostFix("/"))
                           .ToArray()
                    )
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return services;
    }
}


