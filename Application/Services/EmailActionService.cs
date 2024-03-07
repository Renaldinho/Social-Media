using Application.Context;
using Application.Interfaces;
using RabbitMQ.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Services;

public class EmailActionService: IEmailActionService
{
    private readonly IConnection _connection;

    public EmailActionService(RabbitMqConfiguration rabbitMqConfiguration)
    {
        // Establish connection to RabbitMQ using connection string
        var factory = new ConnectionFactory() { Uri = new Uri(rabbitMqConfiguration.ConnectionString)};
        
        _connection = factory.CreateConnection();
        Console.WriteLine($"Using connection string: {rabbitMqConfiguration.ConnectionString}");
    }


    public void SendSuccessfulRegistrationEmail(string email)
    {
        using var channel = _connection.CreateModel();

        const string queueName = "registration_emails";
        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

        var message = new { Email = email };
        var messageBytes = JsonSerializer.SerializeToUtf8Bytes(message);

        var basicProperties = channel.CreateBasicProperties();
        basicProperties.Persistent = true;

        channel.BasicPublish("", queueName, basicProperties, messageBytes);
    }
}