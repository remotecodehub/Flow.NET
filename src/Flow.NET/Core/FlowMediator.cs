using Flow.Core;
using Flow.Internal;

namespace Flow;

public sealed class FlowMediator : IMediator
{
    private readonly RequestDispatcher _requestDispatcher;
    private readonly NotificationDispatcher _notificationDispatcher;
    private readonly StreamDispatcher _streamDispatcher;

    internal FlowMediator(ServiceFactory serviceFactory)
    {
        _requestDispatcher = new(serviceFactory);
        _notificationDispatcher = new(serviceFactory);
        _streamDispatcher = new(serviceFactory);
    }

    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
        => _requestDispatcher.Dispatch(request, cancellationToken);

    public Task Publish(
        INotification notification,
        CancellationToken cancellationToken = default)
        => _notificationDispatcher.Dispatch(notification, cancellationToken);

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default)
        => _streamDispatcher.Dispatch(request, cancellationToken);
}
