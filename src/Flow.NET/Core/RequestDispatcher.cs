using Flow.Internal;

namespace Flow.Core;

internal sealed class RequestDispatcher(ServiceFactory serviceFactory)
{
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

    private Func<IRequest<TResponse>, CancellationToken, Task<TResponse>>
        CreateHandlerDelegate<TResponse>(Type requestType)
    {
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(requestType, typeof(TResponse));

        return async (request, ct) =>
        {
            var handler = (IRequestHandler<IRequest<TResponse>, TResponse>)
                serviceFactory(handlerType)!;

            return await handler.Handle((dynamic)request, ct)
                                .ConfigureAwait(false);
        };
    }
}
