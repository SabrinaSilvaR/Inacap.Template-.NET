---
name: architecture-guardian
description: Validates and enforces Clean Architecture and Vertical Slice boundaries, NetArchTest rules, and package version consistency.
---

# Architecture Guardian Skill

Use this skill to audit pull requests, refactorings, or new code additions to ensure architectural purity.

## Rules to Enforce

1. **Dependency Inversion**:
   - `Domain` must not reference `Application`, `Infrastructure`, or `Api`.
   - `Domain` must not reference EF Core, ASP.NET Core, or external DB libraries.
   - `Application` must not reference `Infrastructure` or `Api`.
   - `Infrastructure` must not reference `Api`.

2. **Central Package Management (CPM)**:
   - Check `Directory.Packages.props`. All package versions must be declared there.
   - Project files (`.csproj`) must NOT have `Version="..."` in `<PackageReference>`.

3. **FastEndpoints Best Practices**:
   - Endpoints must inherit from FastEndpoints base classes (`Endpoint<TReq, TRes>`, `EndpointWithoutRequest<TRes>`).
   - Do NOT use standard ASP.NET MVC controllers.
   - Validate that responses use `Send.OkAsync()`, `Send.CreatedAtAsync()`, `Send.NotFoundAsync()`, etc.

4. **Automated Verification Command**:
   ```powershell
   dotnet test tests/FastEndpointsTemplate.ArchitectureTests/FastEndpointsTemplate.ArchitectureTests.csproj
   ```
