using RabbitMQ.Client;
using User.API.MessageConsumers;

namespace User.API.DependencyResolvement;

public static class DependencyResolverService
{
    public static void ConfigureServices(IServiceCollection services)
    {
        // Other service registrations...

        // Register RabbitMQ ConnectionFactory directly using environment variable
        services.AddSingleton<IConnectionFactory>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetValue<string>("EASYNETQ_CONNECTION_STRING");

            return new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                DispatchConsumersAsync = true,
                TopologyRecoveryEnabled = true
            };
        });

        services.AddHostedService<RabbitMQListenerService>();
    }
}