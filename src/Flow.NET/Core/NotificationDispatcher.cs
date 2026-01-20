using Flow.Internal;

namespace Flow.Core;

/// <summary>
/// Dispatches notifications to all registered notification handlers.
/// </summary>
internal sealed class NotificationDispatcher(ServiceFactory serviceFactory)
{
    /// <summary>
    /// Asynchronously dispatches the specified notification to all registered handlers.
    /// </summary>
    /// <remarks>Each handler registered for the notification type will be invoked in turn. Handlers are
    /// resolved from the service factory at the time of dispatch. If no handlers are registered for the notification
    /// type, the method completes without invoking any handlers.</remarks>
    /// <param name="notification">The notification to be published. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous dispatch operation.</returns>
    public async Task Dispatch(
        INotification notification,
        CancellationToken cancellationToken)
    {
        var notificationType = notification.GetType();
        var handlerType = typeof(INotificationHandler<>)
            .MakeGenericType(notificationType);

        var handlers = (IEnumerable<object>)
            (serviceFactory(typeof(IEnumerable<>)
                .MakeGenericType(handlerType)) ?? Enumerable.Empty<object>());

        foreach (var handler in handlers)
        {
            await ((dynamic)handler)
                .Handle((dynamic)notification, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}