namespace Flow;

/// <summary>
/// Represents a delegate that handles a request and returns a response asynchronously.
/// </summary>
/// <typeparam name="TResponse">The type of the response returned by the delegate.</typeparam>
/// <returns>A task that represents the asynchronous operation. The task result contains the response to the request.</returns>
public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();