using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Services;

public class RegistrationConsumer
{
    private readonly string _queueName;
    private readonly IConnectionFactory _connectionFactory;
    private readonly EmailService _emailService;

    public RegistrationConsumer(string queueName, IConnectionFactory connectionFactory)
    {
        _queueName = queueName;
        _connectionFactory = connectionFactory;
        _emailService = new EmailService();
    }

    public void StartConsuming()
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine($"Received message on queue '{_queueName}': {messageBody}");

            _emailService.SendEmail("Hello!", "Hi"); // Replace with desired subject and body

            // Acknowledge receipt (important!)
            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(_queueName, autoAck: false, consumer);

        Console.WriteLine($"Listening for messages on queue '{_queueName}'...");
        Console.ReadLine(); // Block the main thread until user presses Enter
    }
}