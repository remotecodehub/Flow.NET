# Flow.NET

Flow.NET is a **lightweight, high-performance mediator library for .NET**, inspired by the Mediator pattern and created as a free and open alternative to MediatR.

It enables clean separation of concerns, promotes maintainable architectures (such as Clean Architecture and CQRS), and focuses on **performance, simplicity, and modern .NET practices**.

---

## ✨ Key Features

- ✅ Clean and intuitive Mediator implementation
- 🚀 High-performance, low-allocation design
- 🧩 Supports **Commands**, **Queries**, **Notifications**, and **Pipelines**
- 🔌 First-class integration with `Microsoft.Extensions.DependencyInjection`
- 🧠 Minimal abstractions, easy to reason about
- 🧪 Fully testable and mock-friendly
- 📦 No external dependencies (besides .NET base libraries)
- 📄 MIT licensed

---

## 🎯 Why Flow.NET?

Recent changes in licensing of popular mediator libraries have created a need for a **free, transparent, and community-driven** alternative.

Flow.NET was created with the following principles:

- **Open by default** – permissive MIT license
- **Performance-first** – avoid reflection where possible
- **Modern .NET** – designed for .NET 8+ and .NET 10
- **Predictable behavior** – no hidden magic
- **Drop-in friendly** – familiar API for MediatR users

---

## 📦 Installation

```bash
dotnet add package Flow.NET
```

---

## 🚀 Basic Usage

### Define a Request

```csharp
public sealed record GetUserQuery(int Id) : IRequest<User>;
```

### Create a Handler

```csharp
public sealed class GetUserHandler : IRequestHandler<GetUserQuery, User>
{
    public Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new User(request.Id, "John Doe"));
    }
}
```

### Register Flow.NET

```csharp
services.AddFlow();
```

### Send a Request

```csharp
var user = await mediator.Send(new GetUserQuery(1));
```

---

## 📣 Notifications

```csharp
public sealed record UserCreatedNotification(int UserId) : INotification;

public sealed class UserCreatedHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"User created: {notification.UserId}");
        return Task.CompletedTask;
    }
}
```

---

## 🔗 Pipeline Behaviors

Flow.NET supports pipeline behaviors, enabling cross-cutting concerns such as:

- [x] Logging

- [x] Validation

- [x] Transactions

- [x] Caching

- [x] Metrics

```csharp
public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling {typeof(TRequest).Name}");
        var response = await next();
        Console.WriteLine($"Handled {typeof(TRequest).Name}");
        return response;
    }
}
```

---

## 🧱 Architectural Fit

Flow.NET fits naturally into:

- [x] Clean Architecture

- [x] Hexagonal Architecture

- [x] CQRS

- [x] Modular Monoliths

- [x] Microservices

It helps enforce use-case-driven application design while keeping dependencies pointing inward.

---

## ⚡ Performance Philosophy

Flow.NET is designed with performance in mind:

- [x] Minimal allocations

- [x] Cached delegates

- [x] No runtime scanning during execution

- [ ] Optional Source Generator support (planned)

Benchmarks will be provided comparing Flow.NET with other mediator implementations.

---

## 🧪 Testing

Flow.NET is easy to test:

- [x]Handlers are plain classes

- [x] No static state

- [x] No hidden service locators

```csharp
var handler = new GetUserHandler();
var result = await handler.Handle(new GetUserQuery(1), CancellationToken.None);
```

---

## 🛣️ Roadmap

- [ ] Source Generator for handler registration

- [ ] NativeAOT compatibility

- [ ] Benchmark suite

- [ ] Diagnostics and tracing hooks

- [ ] Community extensions

---

## 🤝 Contributing

Contributions are welcome!

Open issues for bugs or feature requests

Submit pull requests with clear descriptions

Follow existing coding and architectural guidelines

---

## 📄 License

This project is licensed under the MIT License.

---

## ⭐ Acknowledgements

Flow.NET is inspired by the Mediator pattern and the .NET community’s long-standing work on clean and maintainable software architectures.

---
