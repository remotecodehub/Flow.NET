namespace Flow.Core;

/// <summary>
/// Executes a stream request through a pipeline of behaviors and a final handler, producing an asynchronous sequence of
/// responses.
/// </summary>
/// <remarks>This class is intended for internal use to coordinate the execution of stream request pipelines.
/// Behaviors can modify, observe, or short-circuit the request and response sequence. The pipeline is constructed such
/// that each behavior wraps the next, ending with the handler.</remarks>
/// <typeparam name="TRequest">The type of the stream request to be processed. Must implement <see cref="IStreamRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response elements produced by the pipeline.</typeparam>
/// <param name="behaviors">The collection of pipeline behaviors to apply to the request. Behaviors are invoked in the order provided.</param>
/// <param name="handler">The delegate that handles the request after all pipeline behaviors have been applied. Returns an asynchronous
/// sequence of responses.</param>
internal sealed class StreamPipelineExecutor<TRequest, TResponse>(
    IEnumerable<IStreamPipelineBehavior<TRequest, TResponse>> behaviors,
    Func<TRequest, CancellationToken, IAsyncEnumerable<TResponse>> handler)
    where TRequest : IStreamRequest<TResponse>
{
    private readonly IReadOnlyList<IStreamPipelineBehavior<TRequest, TResponse>> _behaviors = behaviors.ToList();

    /// <summary>
    /// Executes the request through the configured pipeline of behaviors and returns an asynchronous stream of
    /// responses.
    /// </summary>
    /// <remarks>Behaviors are invoked in reverse order of registration, allowing each to process the request
    /// and optionally modify the response stream. The returned sequence is not materialized until iterated. This method
    /// is thread-safe if the underlying behaviors and handler are thread-safe.</remarks>
    /// <param name="request">The request object to be processed by the pipeline. Cannot be null.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>An asynchronous sequence of responses produced by processing the request through the pipeline.</returns>
    public IAsyncEnumerable<TResponse> Execute(
        TRequest request,
        CancellationToken cancellationToken)
    {
        StreamHandlerDelegate<TResponse> next = () =>
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
