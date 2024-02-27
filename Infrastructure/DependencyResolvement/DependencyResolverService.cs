using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyResolvement;

public static class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}