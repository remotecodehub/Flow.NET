using Flow.Internal;

namespace Flow.Core;

/// <summary>
/// Executes a request through a pipeline of behaviors and a final handler, allowing each behavior to process or modify
/// the request and response.
/// </summary>
/// <remarks>The pipeline executor composes multiple <see cref="IPipelineBehavior{TRequest, TResponse}"/>
/// instances and a request handler into a single execution chain. Each behavior can inspect, modify, or short-circuit
/// the request and response. This pattern is commonly used to implement cross-cutting concerns such as logging,
/// validation, or authorization in a decoupled manner.</remarks>
/// <typeparam name="TRequest">The type of the request message to be processed by the pipeline. Must implement <see cref="IRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the pipeline after processing the request.</typeparam>
internal sealed class PipelineExecutor<TRequest, TResponse>(
    IEnumerable<IPipelineBehavior<TRequest, TResponse>> behaviors,
    Func<TRequest, CancellationToken, Task<TResponse>> handler)
    where TRequest : IRequest<TResponse>
{
    private readonly IReadOnlyList<IPipelineBehavior<TRequest, TResponse>> _behaviors = behaviors.ToList();

    /// <summary>
    /// Executes the request through the configured pipeline of behaviors and returns the response asynchronously.
    /// </summary>
    /// <remarks>The request is processed by each behavior in the pipeline in order, allowing for
    /// cross-cutting concerns such as logging, validation, or authorization to be applied. The operation is performed
    /// asynchronously and may be canceled by the provided cancellation token.</remarks>
    /// <param name="request">The request message to be processed by the pipeline. Cannot be null.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response produced by the
    /// pipeline.</returns>
    public Task<TResponse> Execute(
        TRequest request,
        CancellationToken cancellationToken)
    {
        RequestHandlerDelegate<TResponse> next = () =>
            handler(request, cancellationToken);

        for (var i = _behaviors.Count - 1; i >= 0; i--)
        {
            var behavior = _behaviors[i];
            var currentNext = next;

            next = () => behavior.Handle(request, currentNext, cancellationToken);
        }

        return next();
    }
}
