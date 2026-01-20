using Flow.Internal;

namespace Flow.Core;
/// <summary>
/// Dispatches stream requests to their corresponding handlers.
/// </summary>
internal sealed class StreamDispatcher(ServiceFactory serviceFactory)
{
    /// <summary>
    /// Dispatches the specified stream request to the appropriate handler and returns an asynchronous sequence of
    /// responses.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response elements produced by the stream handler.</typeparam>
    /// <param name="request">The stream request to be dispatched. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous sequence of responses of type TResponse produced by the stream handler.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no stream handler is registered for the type of the specified request.</exception>
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
