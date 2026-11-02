---
name: ways-of-working
description: Operating manual for all AI agents in this codebase. Defines the autonomy policy (bias to action), Definition of Done (DoD), technical decision defaults, and communication rules.
---

# Ways of Working — Autonomy, Quality & AI Guardrails

The operating manual every agent follows in this repository, on top of `AGENTS.md`.
Its purpose is to ensure you deliver **high-quality, production-ready** changes with **as little back-and-forth as possible**.

---

## 1. Persona & Communication

- **Assume the requester may be non-technical or focused on product outcomes**: They describe outcomes (*"que no se puedan crear productos con precio negativo"*), not implementation minutiae. It is **your** job to translate intent into this template's patterns.
- **Bilingual Protocol**:
  - **Mirror the user's language** in all communications with the user (summaries, explanations, questions, test instructions in Spanish).
  - **Write all repository artifacts** (code, C# identifiers, variables, comments, docs, commit messages, PRs) in **English**.
- **Plain terms**: Explain technical decisions clearly without unnecessary jargon dumps.

---

## 2. Default to Action (Autonomy Policy)

- **Bias to doing, not asking.** Make technical implementation decisions yourself from the established patterns in this repository.
- Ask the user **only** when a product / functional decision cannot be inferred. Then ask **1–3 short, concrete questions** with a recommended default.
- For everything else: pick the senior default, **state the assumption** in your close-out message, and proceed.
- Never block on a decision you are equipped to make.

---

## 3. Technical Decision Matrix (Defaults)

| Fork / Decision | Default in this Codebase |
|---|---|
| **API Pattern** | FastEndpoints (REPR: 1 class per endpoint). Never MVC controllers. |
| **Endpoint Location** | `src/FastEndpointsTemplate.Api/Endpoints/<Feature>/<Action>Endpoint.cs` |
| **Response Sending** | Use fluent methods: `await Send.OkAsync(...)`, `await Send.CreatedAtAsync(...)`, `await Send.NotFoundAsync(...)`. |
| **Validation** | FluentValidation inheriting `Validator<TRequest>`. |
| **Domain Logic** | Entities in `Domain/Entities/` with private setters and factory methods returning `Result<T>`. |
| **Data Access** | EF Core `ApplicationDbContext` in `Infrastructure/`. Endpoints inject `IApplicationDbContext`. |
| **Testing** | xUnit tests written alongside code: Unit (`UnitTests`), Architecture (`ArchitectureTests`), Integration (`IntegrationTests`). |
| **Database Engine** | Microsoft SQL Server (production / Docker Compose) & SQLite (zero-config local run / test runs). |

---

## 4. Definition of Done (Single Quality Gate)

A task or change is considered **DONE** only when **all** of the following hold:

- ☑️ **All tests passing**: `dotnet test` succeeds (Unit, Architecture, and Integration).
- ☑️ **Build clean**: `dotnet build` with 0 errors and 0 warnings.
- ☑️ **Zero secret leakage**: Connection strings, API keys, and credentials flow through `appsettings.json` / environment variables.
- ☑️ **Architecture boundaries respected**: No direct leak of Infrastructure/EF Core into Domain. Verified by `NetArchTest`.
- ☑️ **Nullable reference types respected**: No reckless use of `!` null-forgiving operators.
- ☑️ **Endpoints documented**: `Summary(...)` configured for OpenAPI / Swagger.
- ☑️ **Close-out delivered**: Clear, polite summary to the user in their language.
