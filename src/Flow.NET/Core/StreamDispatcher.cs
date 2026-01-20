using Flow.Internal;

namespace Flow.Core;

internal sealed class StreamDispatcher(ServiceFactory serviceFactory)
{
    public IAsyncEnumerable<TResponse> Dispatch<TResponse>(
        IStreamRequest<TResponse> request,
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();

        var handlerType = typeof(IStreamRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var handler = serviceFactory(handlerType)
            ?? throw new InvalidOperationException(
                $"No stream handler registered for {requestType.Name}");

        return ((dynamic)handler)
            .Handle((dynamic)request, cancellationToken);
    }
}
