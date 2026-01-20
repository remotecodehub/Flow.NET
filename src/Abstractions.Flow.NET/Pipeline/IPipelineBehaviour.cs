namespace Flow;

/// <summary>
/// Defines a behavior that can be executed as part of a request handling pipeline. Behaviors can perform actions before
/// or after the request handler is invoked, such as logging, validation, or exception handling.
/// </summary>
/// <remarks>Implementations of this interface can be used to add cross-cutting concerns to the request processing
/// pipeline. Multiple behaviors can be chained together, allowing for flexible composition of pre- and post-processing
/// logic around request handlers.</remarks>
/// <typeparam name="TRequest">The type of the request message being handled. Must implement <see cref="IRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the request handler.</typeparam>
public interface IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Processes the specified request and optionally invokes the next handler in the pipeline.
    /// </summary>
    /// <remarks>This method is typically used in a request handling pipeline to perform pre-processing,
    /// post-processing, or to short-circuit the pipeline. The implementation may choose to invoke <paramref
    /// name="next"/> to continue processing, or return a response directly.</remarks>
    /// <param name="request">The request message to be handled. Cannot be null.</param>
    /// <param name="next">A delegate representing the next handler in the pipeline. Invoking this delegate passes control to the
    /// subsequent handler.</param>
    /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response produced by handling
    /// the request.</returns>
    Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken);
}