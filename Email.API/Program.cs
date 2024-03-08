using Consumers.Consumers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Consumers;

class Program
{

    static async Task Main(string[] args)
    {
        
        var serviceCollection = new ServiceCollection();

        ServiceRegistration.AddRabbitMQServices(serviceCollection);
        
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
    
        // Resolve the consumer instead of manually creating it
        var consumer = serviceProvider.GetRequiredService<RegistrationConsumer>();
        await consumer.StartConsumingAsync();
    }

}