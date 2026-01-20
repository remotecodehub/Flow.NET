namespace Flow;

/// <summary>
/// Represents a method that asynchronously produces a stream of response elements.
/// </summary>
/// <typeparam name="TResponse">The type of elements yielded by the asynchronous stream.</typeparam>
/// <returns>An <see cref="IAsyncEnumerable{TResponse}"/> that provides the streamed response elements.</returns>
public delegate IAsyncEnumerable<TResponse> StreamHandlerDelegate<TResponse>();
