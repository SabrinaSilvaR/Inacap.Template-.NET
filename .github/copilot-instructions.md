# GitHub Copilot Custom Instructions

You are working on an enterprise C# .NET solution configured with FastEndpoints and Clean Architecture.

## Key Directives:
1. When generating endpoints, use FastEndpoints REPR pattern (`Endpoint<TRequest, TResponse>` with `Validator<TRequest>`).
2. Do not suggest or generate traditional MVC controllers (`ControllerBase`).
3. Send responses using FastEndpoints fluent methods: `await Send.OkAsync(...)`, `await Send.CreatedAtAsync(...)`, `await Send.NotFoundAsync(...)`.
4. Use Central Package Management (`Directory.Packages.props`). Never add versions inside `<PackageReference>` in `.csproj`.
5. Keep the Domain project free from EF Core and framework dependencies.
