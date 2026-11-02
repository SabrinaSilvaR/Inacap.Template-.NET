---
name: fastendpoints-feature-generator
description: Generates a complete vertical slice feature in FastEndpoints adhering to Clean Architecture and REPR pattern (Request, Endpoint, Response, Validator, Entity, Tests).
---

# FastEndpoints Feature Generator Skill

Use this skill when you need to scaffold a new API feature or endpoint in this template repository.

## Resources Available
- **Template**: [`templates/EndpointSlice.cs.template`](templates/EndpointSlice.cs.template) — Fill-in template for a full REPR endpoint slice.
- **Reference Example**: [`examples/CreateCustomerEndpointExample.cs.txt`](examples/CreateCustomerEndpointExample.cs.txt) — Production-ready reference implementation.

---

## Workflow

1. **Ask/Determine Requirements**:
   - Entity name (e.g., `Customer`, `Order`, `Invoice`)
   - HTTP Verb and Route (e.g., `POST /api/orders`)
   - Properties of Request & Response
   - Validation rules (required, min/max, email, regex)

2. **Step 1: Domain Entity**:
   - Check if the entity exists in `src/FastEndpointsTemplate.Domain/Entities/`.
   - If not, create it inheriting from `AggregateRoot` or `Entity`.
   - Provide private constructor and a factory method `Create(...)` returning `Result<TEntity>`.

3. **Step 2: DbContext Mapping**:
   - In `src/FastEndpointsTemplate.Infrastructure/Persistence/Configurations/{Entity}Configuration.cs`, implement `IEntityTypeConfiguration<TEntity>`.
   - Expose `DbSet<TEntity>` in `IApplicationDbContext` and `ApplicationDbContext`.

4. **Step 3: FastEndpoints REPR Slice**:
   - Copy structure from [`templates/EndpointSlice.cs.template`](templates/EndpointSlice.cs.template).
   - Create `src/FastEndpointsTemplate.Api/Endpoints/{Entities}/{Action}Endpoint.cs`.
   - Include:
     - `public sealed record {Action}Request(...)`
     - `public sealed record {Action}Response(...)`
     - `public sealed class {Action}Validator : Validator<{Action}Request>`
     - `public class {Action}Endpoint(...) : Endpoint<{Action}Request, {Action}Response>`
   - Use modern FastEndpoints v8.3 fluent send methods: `await Send.OkAsync(...)` or `await Send.CreatedAtAsync(...)`.

5. **Step 4: Integration Test**:
   - Create `tests/FastEndpointsTemplate.IntegrationTests/Endpoints/{Action}EndpointTests.cs`.
   - Inherit from `TestBase<AppFixture>`.
   - Test both success (200/201) and validation failure (400).

6. **Verification**:
   - Execute `dotnet test` and ensure all tests pass.
