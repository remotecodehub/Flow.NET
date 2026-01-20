using Flow.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Flow.DependencyInjection;
/// <summary>
/// Extension methods for registering Flow.NET services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers Flow mediator services and handler implementations from the specified assemblies into the dependency
    /// injection container.
    /// </summary>
    /// <remarks>This method scans the provided assemblies for types implementing Flow handler interfaces such
    /// as <see cref="IRequestHandler{TRequest, TResponse}"/>, <see cref="INotificationHandler{TNotification}"/>, <see
    /// cref="IStreamRequestHandler{TRequest, TResponse}"/>, <see cref="IPipelineBehavior{TRequest, TResponse}"/>, and
    /// <see cref="IStreamPipelineBehavior{TRequest, TResponse}"/>. Discovered handlers are registered as transient
    /// services. This method is intended to be called during application startup as part of service
    /// configuration.</remarks>
    /// <param name="services">The service collection to which Flow services and handlers will be added. Cannot be null.</param>
    /// <param name="assemblies">An array of assemblies to scan for handler implementations. Each assembly is searched for types implementing
    /// Flow handler interfaces.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance, enabling method chaining.</returns>
    public static IServiceCollection AddFlow(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddSingleton<IMediator, FlowMediator>();
        services.AddSingleton<ServiceFactory>(sp => sp.GetService);

        var types = assemblies.SelectMany(a => a.DefinedTypes);

        foreach (var type in types)
        {
            foreach (var iface in type.ImplementedInterfaces)
            {
                if (!iface.IsGenericType) continue;

                var def = iface.GetGenericTypeDefinition();

                if (def == typeof(IRequestHandler<,>) ||
                    def == typeof(INotificationHandler<>) ||
                    def == typeof(IStreamRequestHandler<,>) ||
                    def == typeof(IPipelineBehavior<,>) ||
                    def == typeof(IStreamPipelineBehavior<,>))
                {
                    services.AddTransient(iface, type);
                }
            }
        }

        return services;
    }

}