using Flow.Core;
using Flow.Internal;

namespace Flow;

/// <summary>
/// Default implementation of <see cref="IMediator"/>.
/// </summary>
/// <remarks>
/// This class is intended to be resolved via dependency injection
/// and should not be instantiated manually.
/// </remarks>

public sealed class FlowMediator : IMediator
{
    private readonly RequestDispatcher _requestDispatcher;
    private readonly NotificationDispatcher _notificationDispatcher;
    private readonly StreamDispatcher _streamDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="FlowMediator"/> class.
    /// </summary>
    /// <param name="serviceFactory">
    /// Internal service factory used to resolve handlers and behaviors.
    /// </param>
    internal FlowMediator(ServiceFactory serviceFactory)
    {
        _requestDispatcher = new(serviceFactory);
        _notificationDispatcher = new(serviceFactory);
        _streamDispatcher = new(serviceFactory);
    }

    /// <summary>
    /// Sends the specified request to the appropriate handler and returns the response asynchronously.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected from the request.</typeparam>
    /// <param name="request">The request to be sent. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
    /// <returns>A task that represents the asynchronous send operation. The task result contains the response returned by the
    /// handler.</returns>
    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
        => _requestDispatcher.Dispatch(request, cancellationToken);

    /// <summary>
    /// Publishes the specified notification to all registered handlers asynchronously.
    /// </summary>
    /// <param name="notification">The notification message to be published. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the publish operation.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    public Task Publish(
        INotification notification,
        CancellationToken cancellationToken = default)
        => _notificationDispatcher.Dispatch(notification, cancellationToken);

    /// <summary>
    /// Creates an asynchronous stream of responses for the specified streaming request.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response elements returned by the stream.</typeparam>
    /// <param name="request">The streaming request that defines the parameters and context for the operation. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the streaming operation. The default value is <see
    /// cref="CancellationToken.None"/>.</param>
    /// <returns>An asynchronous stream of response elements of type <typeparamref name="TResponse"/>. The stream yields
    /// responses as they become available.</returns>
    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default)
        => _streamDispatcher.Dispatch(request, cancellationToken);
}
