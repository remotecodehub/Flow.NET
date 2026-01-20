namespace Flow;

/// <summary>
/// Handles a notification.
/// </summary>
/// <typeparam name="TNotification">The notification type.</typeparam>
public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    /// <summary>
    /// Handles the specified notification.
    /// </summary>
    /// <param name="notification">The notification instance.</param>
    /// <param name="cancellationToken">A token to observe cancellation.</param>
    Task Handle(
        TNotification notification,
        CancellationToken cancellationToken);
}
