using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace User.API.MessageConsumers;

public class RabbitMQListenerService : BackgroundService
{
    
    private readonly IConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private readonly string _queueName = "user_profile_registration";
    private readonly ILogger<RabbitMQListenerService> _logger;

    public RabbitMQListenerService(IConnectionFactory connectionFactory, ILogger<RabbitMQListenerService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
        consumer.Received += CreateUser;

        channel.BasicConsume(_queueName, autoAck: false, consumer);
        Console.WriteLine($"Listening for messages on queue '{_queueName}'...");
    }

    private async Task CreateUser(object sender, BasicDeliverEventArgs @event)
    {
        var body = @event.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        _logger.LogInformation($"Received message: {message}");

        _logger.LogInformation("User creation request processed. Acknowledging message.");
        var channel = ((AsyncEventingBasicConsumer)sender).Model;
        channel.BasicAck(@event.DeliveryTag, false);
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
