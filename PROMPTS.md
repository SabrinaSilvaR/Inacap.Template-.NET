# 📋 PROMPTS.md — AI Task Playbooks & Prompt Recipes

A collection of prompt recipes and playbooks to copy and paste into any AI model (ChatGPT, Claude, Gemini, DeepSeek, Cursor, Copilot) when working on this repository.

---

## 🛠️ Playbook 1: Create a New Vertical Slice Endpoint
```text
Role: Senior .NET Developer.
Repository: FastEndpoints Clean Architecture .NET 10.
Task: Create a new endpoint for [ENTITY_NAME] doing [ACTION_NAME].
Rules:
1. Place in src/FastEndpointsTemplate.Api/Endpoints/[FEATURE]/[ACTION]Endpoint.cs.
2. Define:
   - Request DTO (sealed record).
   - Response DTO (sealed record).
   - Validator inheriting Validator<TRequest> using FluentValidation rules.
   - Endpoint inheriting Endpoint<TRequest, TResponse> or EndpointWithoutRequest<TResponse>.
3. In Configure(): Set Route, HTTP Verb, Summary metadata, and permissions.
4. In HandleAsync(): Execute domain logic or service call, respond using:
   - await Send.OkAsync(...) OR
   - await Send.CreatedAtAsync<TargetEndpoint>(...) OR
   - await Send.NotFoundAsync(...)
5. Create a corresponding integration test in tests/FastEndpointsTemplate.IntegrationTests using TestBase<AppFixture>.
Follow AGENTS.md conventions.
```

---

## 🛠️ Playbook 2: Create a Domain Entity & EF Core Mapping
```text
Role: DDD & EF Core Specialist.
Task: Create a new Aggregate Root / Entity named [ENTITY_NAME].
Rules:
1. Create Entity in src/FastEndpointsTemplate.Domain/Entities/[ENTITY_NAME].cs inheriting from AggregateRoot.
2. Invariants:
   - Private parameterless constructor for EF Core.
   - Private parameterized constructor.
   - Public static Result<[ENTITY_NAME]> Create(...) factory method enforcing non-empty/positive invariants.
3. If applicable, define a Domain Event in src/FastEndpointsTemplate.Domain/Events/[EVENT_NAME].cs.
4. Create EF Core configuration in src/FastEndpointsTemplate.Infrastructure/Persistence/Configurations/[ENTITY_NAME]Configuration.cs.
5. Add DbSet to IApplicationDbContext and ApplicationDbContext.
6. Add unit test in tests/FastEndpointsTemplate.UnitTests/Domain/[ENTITY_NAME]Tests.cs.
```

---

## 🛠️ Playbook 3: Security & Code Review Checklist
```text
Role: Architecture & Security Reviewer.
Task: Review the recent changes against the Definition of Done in AGENTS.md.
Checklist:
- [ ] No database secrets or credentials hardcoded.
- [ ] No unhandled exception leaks raw stack trace or message to HTTP client.
- [ ] Explicit AllowAnonymous() or Authorization policy on each endpoint.
- [ ] Domain layer has 0 references to Infrastructure or EF Core.
- [ ] Central Package Management respected (no Version attribute in csproj).
- [ ] All 15+ automated tests pass with dotnet test.
```

---

## 🛠️ Playbook 4: Add Query with Pagination and Filtering
```text
Task: Implement a paginated list endpoint for [ENTITY_NAME].
Rules:
- Route: GET /api/[ENTITIES]
- Request: Paging request with Page (default 1) and PageSize (default 10, max 100), optional Search.
- Use AsNoTracking() for EF Core read queries.
- Return PagedResult<T> with Items, TotalCount, Page, PageSize.
- Include OpenAPI Summary documenting query parameters.
```
