using System.Collections.Concurrent;

namespace Flow.Internal;
/// <summary>
/// Provides a thread-safe cache for storing and retrieving handler instances by type.
/// </summary>
/// <remarks>This class is intended for internal use to optimize handler lookup and reuse. All members are static
/// and thread-safe.</remarks>
internal static class HandlerCache
{
    private static readonly ConcurrentDictionary<Type, object> _cache = new();

    /// <summary>
    /// Retrieves the value associated with the specified type from the cache, or adds a new value created by the
    /// provided factory if none exists.
    /// </summary>
    /// <remarks>If multiple threads attempt to add a value for the same key concurrently, only one factory
    /// invocation will succeed; subsequent calls will return the cached value. The factory function should be
    /// thread-safe if it has side effects.</remarks>
    /// <typeparam name="T">The type of the value to retrieve or add. Must be a non-nullable type.</typeparam>
    /// <param name="key">The type used as the key to identify the cached value. Cannot be null.</param>
    /// <param name="factory">A function that creates a new value of type T if the cache does not contain an entry for the specified key.
    /// Cannot be null.</param>
    /// <returns>The cached value associated with the specified type, or the newly created value if no entry exists.</returns>
    public static T GetOrAdd<T>(Type key, Func<T> factory)
        where T : notnull
    {
        return (T)_cache.GetOrAdd(key, _ => factory()!);
    }
}
