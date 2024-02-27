using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyResolvement;

public static class DependencyResolverService
{
    public static void RegisterApplicationLayer(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

    }
}