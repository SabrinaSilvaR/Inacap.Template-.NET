# CLAUDE.md - Instructions for Claude Code

## Commands
- Build: `dotnet build`
- Run API: `dotnet run --project src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj`
- Run All Tests: `dotnet test`
- Run Unit Tests: `dotnet test tests/FastEndpointsTemplate.UnitTests/FastEndpointsTemplate.UnitTests.csproj`
- Run Architecture Tests: `dotnet test tests/FastEndpointsTemplate.ArchitectureTests/FastEndpointsTemplate.ArchitectureTests.csproj`
- Run Integration Tests: `dotnet test tests/FastEndpointsTemplate.IntegrationTests/FastEndpointsTemplate.IntegrationTests.csproj`

## Architecture Rules
- Pattern: Clean Architecture + Vertical Slice + FastEndpoints REPR pattern.
- Api layer: FastEndpoints (`Endpoint<TReq, TRes>`). No MVC controllers.
- Domain layer: Pure C#, zero external dependencies.
- Package Management: Central Package Management via `Directory.Packages.props`. Do not put package versions in `.csproj` files.
- Coding Style: C# latest, primary constructors, file-scoped namespaces, nullable reference types enabled.
- Responses: Use `await Send.OkAsync(...)`, `await Send.CreatedAtAsync(...)`, `await Send.NotFoundAsync(...)`.
