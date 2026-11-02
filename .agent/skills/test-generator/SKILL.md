---
name: test-generator
description: Generates Unit Tests (xUnit + FluentAssertions), Integration Tests (FastEndpoints.Testing), and Architecture Tests.
---

# Test Generator Skill

Use this skill to generate robust tests covering happy paths, edge cases, and architectural boundaries.

## Resources Available
- **Integration Test Template**: [`templates/IntegrationTest.cs.template`](templates/IntegrationTest.cs.template) — FastEndpoints.Testing with WebApplicationFactory.
- **Domain Unit Test Template**: [`templates/DomainUnitTest.cs.template`](templates/DomainUnitTest.cs.template) — xUnit unit test testing business invariants and Result pattern.
- **Reference Example**: [`examples/CustomerEndpointIntegrationTestsExample.cs.txt`](examples/CustomerEndpointIntegrationTestsExample.cs.txt) — Working example of integration tests.

---

## Testing Standards

1. **Unit Tests (`tests/FastEndpointsTemplate.UnitTests`)**:
   - Focus on Domain entities, Value Objects, and FluentValidation validators.
   - Use `FluentAssertions` for fluent assertions (`result.Should().BeTrue()`).
   - Follow [`templates/DomainUnitTest.cs.template`](templates/DomainUnitTest.cs.template).

2. **Integration Tests (`tests/FastEndpointsTemplate.IntegrationTests`)**:
   - Use `FastEndpoints.Testing` with `TestBase<AppFixture>`.
   - Use `fixture.Client.POSTAsync<TEndpoint, TReq, TRes>(request)` for type-safe endpoint invocation.
   - Assert response status code and JSON payload structure.
   - Test validation errors (e.g., verifying 400 Bad Request with ProblemDetails).
   - Follow [`templates/IntegrationTest.cs.template`](templates/IntegrationTest.cs.template).

3. **Architecture Tests (`tests/FastEndpointsTemplate.ArchitectureTests`)**:
   - Use `NetArchTest.Rules` to prevent illegal dependencies across Clean Architecture layers.

## Command Execution
```powershell
dotnet test
```
