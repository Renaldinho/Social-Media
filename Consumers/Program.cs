using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

class Program
{

    static async Task Main(string[] args)
    {
        
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IConnectionFactory>(provider =>
        {
            var connectionString = Environment.GetEnvironmentVariable("EASYNETQ_CONNECTION_STRING");
            return new ConnectionFactory()
            {
                Uri = new Uri(connectionString)
            };
        });
        
        await Task.Delay(20000); // 20 seconds in milliseconds
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var consumer = new RegistrationConsumer("registration_emails", serviceProvider.GetRequiredService<IConnectionFactory>());
        consumer.StartConsuming(); // Assuming this method is async

        while (true)
        {
            
        }
    }

}