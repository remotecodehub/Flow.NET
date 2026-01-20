using System.Collections.Concurrent;

namespace Flow.Internal;

internal static class HandlerCache
{
    private static readonly ConcurrentDictionary<Type, object> _cache = new();

    public static T GetOrAdd<T>(Type key, Func<T> factory)
        where T : notnull
    {
        return (T)_cache.GetOrAdd(key, _ => factory()!);
    }
}
