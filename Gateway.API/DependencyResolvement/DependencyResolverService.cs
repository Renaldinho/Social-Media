using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;

namespace Gateway.Application.DependencyResolvement;

public static class DependencyResolverService
{

    public static void RegisterApplicationLayer(IServiceCollection services)
    {
        services.AddOcelot();
    }

}