using Flow.Internal;

namespace Flow.Core;

/// <summary>
/// Dispatches request messages to their corresponding handlers.
/// </summary>
internal sealed class RequestDispatcher(ServiceFactory serviceFactory)
{
    /// <summary>
    /// Dispatches the specified request.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <param name="request">The request instance.</param>
    /// <param name="cancellationToken">A token to observe cancellation.</param>
    public Task<TResponse> Dispatch<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();

        var handler = HandlerCache.GetOrAdd(
            requestType,
            () => CreateHandlerDelegate<TResponse>(requestType));

        return ((Func<IRequest<TResponse>, CancellationToken, Task<TResponse>>)handler)
            (request, cancellationToken);
    }
    
    /// <summary>
    /// Creates a delegate that handles a request of the specified type and returns a response asynchronously, applying
    /// all registered pipeline behaviors.
    /// </summary>
    /// <remarks>The returned delegate resolves the appropriate IRequestHandler and any IPipelineBehavior
    /// instances from the service factory. All pipeline behaviors are executed in order before invoking the request
    /// handler. This method is typically used to construct the request handling pipeline dynamically at
    /// runtime.</remarks>
    /// <typeparam name="TResponse">The type of the response returned by the request handler.</typeparam>
    /// <param name="requestType">The type of the request to be handled. Must implement IRequest<TResponse>.</param>
    /// <returns>A delegate that accepts an IRequest<TResponse> and a CancellationToken, and returns a Task representing the
    /// asynchronous operation that produces a response of type TResponse.</returns>
    private Func<IRequest<TResponse>, CancellationToken, Task<TResponse>> CreateHandlerDelegate<TResponse>(Type requestType)
    {
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        var behaviorType = typeof(IPipelineBehavior<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        return async (request, ct) =>
        {
            var handler = serviceFactory(handlerType)!;

            var behaviors = (IEnumerable<object>)
                (serviceFactory(typeof(IEnumerable<>)
                    .MakeGenericType(behaviorType)) ?? Enumerable.Empty<object>());

            var executorType = typeof(PipelineExecutor<,>)
                .MakeGenericType(requestType, typeof(TResponse));

            var executor = Activator.CreateInstance(
                executorType,
                behaviors,
                (Func<object, CancellationToken, Task<TResponse>>)
                ((req, token) =>
                    ((dynamic)handler).Handle((dynamic)req, token)))!;

            return await ((dynamic)executor)
                .Execute((dynamic)request, ct)
                .ConfigureAwait(false);
        };
    }

}
