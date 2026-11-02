# 🤖 AGENTS.md — Universal Guidelines for AI Agents

> **Audience**: Autonomous AI coding agents (Antigravity, Cursor, Claude Code, GitHub Copilot Workspace, Windsurf, Devin, etc.)
> **Purpose**: Definitive architectural principles, patterns, file locations, coding rules, and verification procedures for this codebase.

---

## 1. Operating Manual: Ways of Working

Before writing code or designing features, read **[`.agent/skills/ways-of-working/SKILL.md`](.agent/skills/ways-of-working/SKILL.md)**:
- **Autonomy Policy (Bias to Action)**: Make technical decisions yourself from §3 defaults; do not stall on trivial questions.
- **Bilingual Protocol**: Mirror user language in explanations (Spanish); write all code, comments, DTOs, identifiers, and commit messages in English.
- **Definition of Done (DoD)**: Single quality gate (all tests pass, 0 warnings, no hardcoded secrets, explicit security).

---

## 2. Spec-Driven Development (OpenSpec / SDD)

This repository follows the **OpenSpec** methodology:
- **Stable Context**: Read [`specs/project.md`](specs/project.md) for architectural scope and tech stack.
- **Living Truth Capabilities**: Stored in `specs/specs/<capability>/spec.md` (e.g. `specs/specs/catalog/spec.md`).
- **Active Proposals**: Stored in `specs/changes/<change-id>/` (`proposal.md`, `design.md`, `tasks.md`).
- **Archived Decisions**: Stored in `specs/changes/archive/` as an immutable decision log.

---

## 3. Architecture Overview

This codebase uses **Clean Architecture** combined with **Vertical Slice Architecture** and the **REPR (Request-Endpoint-Response) Pattern** powered by **FastEndpoints** on **.NET 10** with **Microsoft SQL Server**.

```
┌────────────────────────────────────────────────────────┐
│               Api (FastEndpoints, Middlewares)         │
│  └── Endpoints/ [Feature] / (Request, Response, Validator, Endpoint)
└───────────┬────────────────────────────────────────────┘
            │ depends on
┌───────────▼────────────────────────────────────────────┐
│   Infrastructure (EF Core, SQL Server/SQLite, Logs)    │
└───────────┬────────────────────────────────────────────┘
            │ depends on
┌───────────▼────────────────────────────────────────────┐
│         Application (Interfaces, DTOs, Use Cases)      │
└───────────┬────────────────────────────────────────────┘
            │ depends on
┌───────────▼────────────────────────────────────────────┐
│   Domain (Entities, Value Objects, Domain Events, Error)│
└────────────────────────────────────────────────────────┘
```

### Dependency Inversion Invariants
- `Domain` **MUST NOT** reference any other project or 3rd party infrastructure libraries (except BCL).
- `Application` depends ONLY on `Domain`.
- `Infrastructure` depends on `Application` and `Domain`.
- `Api` orchestrates dependencies, configures `FastEndpoints`, OpenAPI/Swagger, and middlewares.
- **Enforcement**: Architecture tests in `tests/FastEndpointsTemplate.ArchitectureTests` use `NetArchTest.Rules` to fail the build if boundaries are breached.

---

## 4. FastEndpoints & the REPR Pattern

When creating a new API endpoint, adhere strictly to the **REPR pattern** (Request-Endpoint-Response):

1. **Location**: `src/FastEndpointsTemplate.Api/Endpoints/{FeatureName}/{ActionName}Endpoint.cs`
2. **Components**:
   - **Request DTO**: Strongly typed record or class.
   - **Response DTO**: Strongly typed record or class.
   - **Validator**: Inherits from `Validator<TRequest>` (FastEndpoints + FluentValidation).
   - **Endpoint**: Inherits from `Endpoint<TRequest, TResponse>` or `EndpointWithoutRequest<TResponse>`.
3. **Execution rules**:
   - Endpoints **DO NOT** use Controller inheritance or Minimal API lambda sprawl.
   - Use `Configure()` for route, verb, permissions, swagger metadata.
   - Use `override async Task HandleAsync(TRequest req, CancellationToken ct)` for logic.
   - Use fluent `Send` property to return responses:
     - `await Send.OkAsync(response, ct)`
     - `await Send.CreatedAtAsync<TargetEndpoint>(routeValues, response, cancellation: ct)`
     - `await Send.NotFoundAsync(ct)`
     - `await Send.NoContentAsync(ct)`
     - `ThrowError("Message", 400)` for fast validation/business failures.

---

## 5. Verification Commands

Run these commands before finishing any agent task:

```powershell
# Restore and build entire solution
dotnet build

# Run all test suites (Unit, Architecture, Integration)
dotnet test

# Run API locally (SQLite by default, 0 config needed)
dotnet run --project src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj
```

---

## 6. Prohibited Practices

- ❌ DO NOT use traditional MVC Controllers (`ControllerBase`). Use `FastEndpoints`.
- ❌ DO NOT inject `ApplicationDbContext` directly into Domain entities or value objects.
- ❌ DO NOT put database migration scripts or connection strings inside the Domain layer.
- ❌ DO NOT disable nullable warnings or ignore compiler warnings.
- ❌ DO NOT add package versions directly into `.csproj` files (use `Directory.Packages.props`).
