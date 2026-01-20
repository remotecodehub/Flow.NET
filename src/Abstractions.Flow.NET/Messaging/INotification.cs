namespace Flow;
/// <summary>
/// Represents a notification that can be published to multiple handlers.
/// </summary>
/// <remarks>
/// Notifications are dispatched in a fan-out manner and do not return a response.
/// </remarks>
public interface INotification
{
}