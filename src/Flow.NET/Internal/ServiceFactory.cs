namespace Flow.Internal;

/// <summary>
/// Represents a method that creates or retrieves a service instance for the specified type.
/// </summary>
/// <param name="serviceType">The type of service to create or retrieve. Cannot be null.</param>
/// <returns>An instance of the requested service type, or null if the service cannot be provided.</returns>
internal delegate object? ServiceFactory(Type serviceType);