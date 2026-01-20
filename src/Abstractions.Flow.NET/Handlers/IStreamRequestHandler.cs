namespace Flow;

/// <summary>
/// Handles a stream request and produces a stream of responses.
/// </summary>
/// <typeparam name="TRequest">The stream request type.</typeparam>
/// <typeparam name="TResponse">The response item type.</typeparam>
public interface IStreamRequestHandler<in TRequest, out TResponse>
    where TRequest : IStreamRequest<TResponse>
{
    /// <summary>
    /// Handles the specified stream request.
    /// </summary>
    /// <param name="request">The stream request instance.</param>
    /// <param name="cancellationToken">A token to observe cancellation.</param>
    /// <returns>A stream of response items.</returns>
    IAsyncEnumerable<TResponse> Handle(
        TRequest request,
        CancellationToken cancellationToken);
}