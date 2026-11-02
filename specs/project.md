# Project Context: FastEndpoints Clean Architecture Template

> **Audience**: AI Agents, LLM Pair Programmers, and Engineering Teams.
> **Scope**: Architecture, Domain Boundaries, Technical Standards, and Capabilities.

---

## 1. System Identity & Purpose
This project is an enterprise-grade, production-ready backend API template designed for **.NET 10** and **C# (latest)**.
It demonstrates the union of:
- **Clean Architecture** (Dependency Inversion, Domain Isolation).
- **Vertical Slice Architecture & REPR Pattern** (Request-Endpoint-Response via **FastEndpoints v8.3**).
- **Relational Persistence** powered by **Microsoft SQL Server** via **EF Core 10/9** (with SQLite fallback for local zero-config runs).
- **Autonomous AI Agent Infrastructure** (OpenSpec SDD, Antigravity Skills, Cursor rules, Claude Code, GitHub Copilot).

---

## 2. Technical Stack Matrix

| Layer / Concern | Technology | Standard / Version |
|---|---|---|
| **Runtime & SDK** | .NET 10 / C# 13-14 | `net10.0` (MSBuild 18.x) |
| **API Pattern** | FastEndpoints | v8.3.0 (REPR: Request-Endpoint-Response) |
| **Validation** | FluentValidation | Automatic pipeline integration |
| **Persistence** | EF Core + SQL Server | Microsoft.EntityFrameworkCore.SqlServer v9/10 |
| **Audit & Interceptors** | EF Core SaveChangesInterceptor | Automatic UTC Timestamp auditing |
| **API Documentation** | OpenAPI 3.0 / Swagger | `FastEndpoints.Swagger` at `/swagger` |
| **Logging** | Serilog | Structured JSON / Console logging |
| **Error Handling** | RFC 7807 ProblemDetails | `IExceptionHandler` global middleware |
| **Health Checks** | ASP.NET Core HealthChecks | `/health` (DB + App health) |
| **Unit Testing** | xUnit + FluentAssertions + Bogus | Test project `FastEndpointsTemplate.UnitTests` |
| **Architecture Testing** | NetArchTest.Rules | Test project `FastEndpointsTemplate.ArchitectureTests` |
| **Integration Testing** | FastEndpoints.Testing | WebApplicationFactory + TestBase |
| **CI / CD Pipeline** | Azure DevOps Pipelines | `deploy/azure-pipelines.yml` |
| **Containerization** | Docker | Multi-stage Dockerfile + Docker Compose |

---

## 3. Core Capabilities (`specs/specs/`)

1. **`catalog`**: Product Catalog Management (CRUD, soft-delete, inventory updates, search and pagination).
2. **`health`**: Real-time system and database health monitoring at `/health`.
3. **`documentation`**: Interactive OpenAPI Swagger documentation at `/swagger`.

---

## 4. Spec-Driven Development (SDD) Protocol
Any new feature, refactor, or capability must follow the OpenSpec loop:
1. Review this context (`specs/project.md`).
2. Propose changes in `specs/changes/<change-id>/proposal.md`.
3. Break down tasks in `specs/changes/<change-id>/tasks.md`.
4. Implement using tests first (`dotnet test`).
5. Update living truth in `specs/specs/<capability>/spec.md` and archive proposal.
