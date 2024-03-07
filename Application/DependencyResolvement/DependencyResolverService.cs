using Application.Context;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyResolvement;

public static class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfig = new JwtConfig
        {
            Secret = configuration["Secrets:JwtSecret"]
        };
        services.AddSingleton(jwtConfig); // Make JwtConfig available for DI
        
        var connectionString = Environment.GetEnvironmentVariable("RabbitMQ__ConnectionString");
        var rabbitMqConfig = new RabbitMqConfiguration()
        {
            ConnectionString = connectionString
        };
        services.AddSingleton(rabbitMqConfig);
        
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
    }
}
