namespace RAG.AI.Presenter.Web.DependencyInjection;

public static class MassTransitInjection
{
    public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services,
       IConfiguration configuration)
    {
        //var rabbitOption = new RabbitMqConfig();
        //configuration.GetSection(RabbitMqConfig.Key).Bind(rabbitOption);
        //services.AddMassTransit(x =>
        //{
        //    x.SetEndpointNameFormatter(
        //        new PrefixEndpointNameFormatter("ServiceProvider"));

        //    x.UsingRabbitMq((context, config) =>
        //    {
        //        config.Host(rabbitOption.Host, rabbitOption.VirtualHost, h =>
        //        {
        //            h.Username(rabbitOption.Username);
        //            h.Password(rabbitOption.Password);
        //            if (rabbitOption.ClusterEnabled && rabbitOption.ClusterNodes != null)
        //                h.UseCluster(c =>
        //                {
        //                    foreach (var node in rabbitOption.ClusterNodes)
        //                        c.Node(node);
        //                });
        //        }
        //        );

        //        config.ConfigureEndpoints(context);
        //    });
        //}
        //);
        return services;
    }
}



