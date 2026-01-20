using Flow.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Flow.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFlow(this IServiceCollection services)
    {
        services.AddSingleton<IMediator, FlowMediator>();

        services.AddSingleton<ServiceFactory>(sp => sp.GetService);

        return services;
    }
}