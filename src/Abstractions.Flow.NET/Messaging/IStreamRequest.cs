namespace Flow;
/// <summary>
/// Represents a request that produces a stream of responses.
/// </summary>
/// <typeparam name="TResponse">
/// The type of the streamed response items.
/// </typeparam>
/// <remarks>
/// Stream requests are processed lazily and are suitable for large or infinite result sets.
/// </remarks>
public interface IStreamRequest<out TResponse>
{
}
