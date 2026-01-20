namespace Flow;
/// <summary>
/// Defines a mediator for sending requests, publishing notifications,
/// and creating response streams.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Sends a request and returns a response.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="request">The request to send.</param>
    /// <param name="cancellationToken">A token to observe cancellation.</param>
    Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a notification to all registered handlers.
    /// </summary>
    /// <param name="notification">The notification to publish.</param>
    /// <param name="cancellationToken">A token to observe cancellation.</param>
    Task Publish(
        INotification notification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a stream of responses for the specified stream request.
    /// </summary>
    /// <typeparam name="TResponse">The response item type.</typeparam>
    /// <param name="request">The stream request.</param>
    /// <param name="cancellationToken">A token to observe cancellation.</param>
    IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}