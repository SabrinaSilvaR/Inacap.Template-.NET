---
name: efcore-migration-manager
description: Manages Entity Framework Core migrations, DbContext model changes, schema evolutions, and database seeding safely for Microsoft SQL Server.
---

# EF Core Migration Manager Skill

Use this skill when modifying database schemas, configuring entity mappings, or generating migrations.

## Resources Available
- **Template**: [`templates/EntityConfiguration.cs.template`](templates/EntityConfiguration.cs.template) — Entity mapping template with SQL Server conventions.
- **Reference Example**: [`examples/CustomerConfigurationExample.cs.txt`](examples/CustomerConfigurationExample.cs.txt) — Real mapping example with unique indexes and properties.

---

## Migration Workflow

1. **Modify Entity Configuration**:
   - Update entity model in `FastEndpointsTemplate.Domain/Entities/`.
   - Update or create configuration in `FastEndpointsTemplate.Infrastructure/Persistence/Configurations/` following [`templates/EntityConfiguration.cs.template`](templates/EntityConfiguration.cs.template).

2. **Add EF Core Migration**:
   Run from repository root:
   ```powershell
   dotnet ef migrations add <MigrationName> `
     --project src/FastEndpointsTemplate.Infrastructure/FastEndpointsTemplate.Infrastructure.csproj `
     --startup-project src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj `
     --output-dir Persistence/Migrations
   ```

3. **Verify Generated Code**:
   - Inspect the generated migration in `Persistence/Migrations/` to ensure no accidental table drops or column truncations.
   - For Microsoft SQL Server, ensure column names and types match schema conventions (`nvarchar(max)`, `decimal(18,2)`, `datetime2`).

4. **Apply Migration Locally**:
   ```powershell
   dotnet ef database update `
     --project src/FastEndpointsTemplate.Infrastructure/FastEndpointsTemplate.Infrastructure.csproj `
     --startup-project src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj
   ```

5. **Generate Idempotent SQL Script for CI/CD**:
   ```powershell
   dotnet ef migrations script --idempotent `
     --project src/FastEndpointsTemplate.Infrastructure/FastEndpointsTemplate.Infrastructure.csproj `
     --startup-project src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj `
     --output migration.sql
   ```
