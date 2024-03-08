using Consumers.Consumers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Services;
using Services.Interfaces;

namespace Consumers;

public static class ServiceRegistration
{
    public static void AddRabbitMQServices(IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("EASYNETQ_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("EASYNETQ_CONNECTION_STRING environment variable is not set or empty.");
        }

        services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
        {
            Uri = new Uri(connectionString),
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
            DispatchConsumersAsync = true,
            TopologyRecoveryEnabled = true
        });

        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<RegistrationConsumer>();
    }
}