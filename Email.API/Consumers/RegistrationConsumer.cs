using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services.Interfaces;

namespace Consumers.Consumers;

public class RegistrationConsumer
{
    private readonly string _queueName = "registration_emails";
    private readonly IConnectionFactory _connectionFactory;
    private readonly IEmailService _emailService;

    public RegistrationConsumer(IConnectionFactory connectionFactory, IEmailService emailService)
    {
        _connectionFactory = connectionFactory;
        _emailService = emailService;
    }
    
    public async Task StartConsumingAsync()
    {
        try
        {
            var connection = await TryConnectAsync();
            SetupMessageConsumption(connection);
            
            await Task.Delay(Timeout.Infinite);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
    }

    private void SetupMessageConsumption(IConnection connection)
    {
        var channel = connection.CreateModel();
        channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += SendEmail;

        channel.BasicConsume(_queueName, autoAck: false, consumer);
        Console.WriteLine($"Listening for messages on queue '{_queueName}'...");
    }

    private async Task SendEmail(object sender, BasicDeliverEventArgs @event)
    {
        var message = Encoding.UTF8.GetString(@event.Body.ToArray());
        await _emailService.SendEmail("recipient@example.com", message);
    }

    public async Task<IConnection> TryConnectAsync(int maxRetries = 5, int retryInterval = 10)
    {
        for (int retryCount = 0; retryCount < maxRetries; retryCount++)
        {
            try
            {
                Console.WriteLine($"Attempting to connect to RabbitMQ, attempt {retryCount + 1}");
                var connection = _connectionFactory.CreateConnection();
                Console.WriteLine("Connected to RabbitMQ successfully.");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error occurred: {ex.Message}");
                if (retryCount < maxRetries - 1)
                {
                    await Task.Delay(TimeSpan.FromSeconds(retryInterval));
                }
            }
        }

        throw new Exception("Failed to connect to RabbitMQ after retries.");
    }

}