using Microsoft.Extensions.Configuration;

namespace Application.Context;

public class RabbitMqConfiguration
{
    public string? ConnectionString { get; set; }
}
