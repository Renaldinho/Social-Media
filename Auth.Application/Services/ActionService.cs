using Application.Context;
using Application.Interfaces;
using RabbitMQ.Client;
using SharedConfig.Messages.Email;
using SharedConfig.Messages.User;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Application.Services;

public class ActionService: IActionService
{
    private readonly IConnection _connection;

    public ActionService(RabbitMqConfiguration rabbitMqConfiguration)
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

        var recipient = new RegistrationRecipient { Email = email };
        var messageBytes = JsonSerializer.SerializeToUtf8Bytes(recipient);

        var basicProperties = channel.CreateBasicProperties();
        basicProperties.Persistent = true;

        channel.BasicPublish("", queueName, basicProperties, messageBytes);
    }

    public void SendCreateUserMessage(int id)
    {
        using var channel = _connection.CreateModel();

        const string queueName = "user_profile_registration";
        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

        var recipient = new CreateUserMessage { Id = id };
        var messageBytes = JsonSerializer.SerializeToUtf8Bytes(recipient);

        var basicProperties = channel.CreateBasicProperties();
        basicProperties.Persistent = true;

        channel.BasicPublish("", queueName, basicProperties, messageBytes);
    }
}