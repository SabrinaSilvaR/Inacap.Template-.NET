# 📐 Coding Standards & Conventions

## 1. C# Style Conventions
- **File-scoped namespaces**: Always use `namespace FastEndpointsTemplate.Foo;`
- **Primary Constructors**: Preferred for DI classes (`public class MyService(ILogger<MyService> logger)`).
- **Records**: Use `record` or `readonly record struct` for immutable DTOs and Requests/Responses.
- **Nullability**: Nullable reference types are strictly enforced. Avoid `!` null-forgiving operators unless justified.
- **Pattern Matching**: Prefer switch expressions and pattern matching over nested if-else ladders.

## 2. FastEndpoints Conventions
- Use REPR: 1 File (or folder) per endpoint containing:
  - `Request`
  - `Response`
  - `Validator` (FluentValidation)
  - `Endpoint`
- Endpoints inherit from `Endpoint<TRequest, TResponse>` or `EndpointWithoutRequest<TResponse>`.
- Use `Send.OkAsync()`, `Send.CreatedAtAsync()`, `Send.NotFoundAsync()` etc.
- Always provide OpenAPI metadata via `Summary(...)`.
