# 🏛️ Architecture Documentation

This repository follows **Clean Architecture** combined with **Vertical Slice Architecture** and the **REPR (Request-Endpoint-Response)** pattern using **FastEndpoints**.

---

## 1. System Layers Diagram

```mermaid
graph TD
    subgraph Presentation ["Presentation Layer (FastEndpoints)"]
        API["FastEndpointsTemplate.Api"]
        ENDPOINTS["Endpoints (REPR: Request, Endpoint, Response, Validator)"]
        SWAGGER["FastEndpoints.Swagger OpenAPI"]
    end

    subgraph Infrastructure ["Infrastructure Layer"]
        INFRA["FastEndpointsTemplate.Infrastructure"]
        DBCONTEXT["ApplicationDbContext (EF Core)"]
        INTERCEPTORS["AuditableEntityInterceptor"]
        DBS["SQLite (Local) / SQL Server (Enterprise/Prod)"]
    end

    subgraph Application ["Application Layer"]
        APP["FastEndpointsTemplate.Application"]
        INTERFACES["IApplicationDbContext, IDateTimeProvider"]
        DTOS["DTOs & Mappings"]
    end

    subgraph Domain ["Domain Layer (Core)"]
        DOM["FastEndpointsTemplate.Domain"]
        ENTITIES["Entities & Aggregates (Product)"]
        EVENTS["Domain Events"]
        RESULT["Result & Error Pattern"]
    end

    API --> INFRA
    API --> APP
    INFRA --> APP
    INFRA --> DOM
    APP --> DOM
    INFRA --> DBS
```

---

## 2. FastEndpoints REPR Lifecycle

```mermaid
sequenceDiagram
    autonumber
    actor Client as HTTP Client
    participant MW as ASP.NET Middlewares
    participant EP as FastEndpoint (REPR)
    participant VAL as Validator (FluentValidation)
    participant DOM as Domain Entity
    participant DB as EF Core / Database

    Client->>MW: HTTP Request (JSON)
    MW->>VAL: Execute Model Validation
    alt Validation Failed
        VAL-->>Client: 400 Bad Request (ProblemDetails)
    else Validation Succeeded
        VAL->>EP: Dispatch to HandleAsync(req, ct)
        EP->>DOM: Product.Create(...) [Factory Invariants]
        DOM-->>EP: Result<Product>
        EP->>DB: Add entity & SaveChangesAsync(ct)
        DB-->>EP: Persisted with generated ID
        EP-->>Client: 201 Created (Send.CreatedAtAsync)
    end
```

---

## 3. Layer Separation Guarantees

| Layer | Can Reference | Forbidden References |
|---|---|---|
| **Domain** | None (BCL only) | Application, Infrastructure, Api, EF Core, ASP.NET |
| **Application** | Domain | Infrastructure, Api, EF Core provider implementations |
| **Infrastructure** | Domain, Application | Api |
| **Api** | Domain, Application, Infrastructure | MVC Controllers, Direct database drivers |

These constraints are tested automatically on every build in `FastEndpointsTemplate.ArchitectureTests` using `NetArchTest.Rules`.
