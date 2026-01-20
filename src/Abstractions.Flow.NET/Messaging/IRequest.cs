namespace Flow;
/// <summary>
/// Represents a request that returns a response.
/// </summary>
/// <typeparam name="TResponse">
/// The type of the response returned by the request.
/// </typeparam>
/// <remarks>
/// Requests are typically used to model commands or queries in CQRS-based architectures.
/// </remarks>
public interface IRequest<out TResponse>
{
}