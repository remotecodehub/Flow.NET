using Flow.Internal;

namespace Flow.Core;

internal sealed class NotificationDispatcher(ServiceFactory serviceFactory)
{
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