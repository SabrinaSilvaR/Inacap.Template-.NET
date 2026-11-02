# Tasks Breakdown: [Change ID]

> **Execution Rule**: Tests first where applicable; complete tasks in order. Run `dotnet test` after each phase.

---

## Phase 1: Domain & Application
- [ ] 1.1 Create/Update Entity in `Domain/Entities/[Entity].cs` with factory method and invariants.
- [ ] 1.2 Add Domain Unit Tests in `UnitTests/Domain/[Entity]Tests.cs`.
- [ ] 1.3 Add DTO in `Application/Features/[Feature]/Dtos/[Entity]Dto.cs`.

## Phase 2: Infrastructure & Persistence
- [ ] 2.1 Create EF Core configuration in `Infrastructure/Persistence/Configurations/[Entity]Configuration.cs`.
- [ ] 2.2 Expose `DbSet<[Entity]>` in `IApplicationDbContext` and `ApplicationDbContext`.

## Phase 3: FastEndpoints API Slice
- [ ] 3.1 Create `Api/Endpoints/[Feature]/[Action]Endpoint.cs` with Request, Response, Validator, and Endpoint.
- [ ] 3.2 Add Validator Unit Test in `UnitTests/Validators/[Action]ValidatorTests.cs`.

## Phase 4: Integration Testing & Verification
- [ ] 4.1 Create Integration Test in `IntegrationTests/Endpoints/[Action]EndpointTests.cs`.
- [ ] 4.2 Verify all tests pass: `dotnet test`.
- [ ] 4.3 Update living spec in `specs/specs/[capability]/spec.md`.
