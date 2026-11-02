# ⚡ FastEndpoints + Clean Architecture .NET 10 Ready Template

> **Enterprise-grade, AI-native C# template** powered by **FastEndpoints (v8.3)**, **.NET 10 ready** (runs out of the box on .NET 9 & .NET 10), **Clean Architecture**, **Vertical Slice REPR Pattern**, and full autonomous **AI Agent Infrastructure** (Antigravity Skills, Cursor rules, Claude Code, GitHub Copilot).

---

## 🌟 Features

- **🚀 FastEndpoints v8.3**: High-performance, REPR (Request-Endpoint-Response) pattern endpoints replacing bulky MVC controllers.
- **🏗️ Clean Architecture + Vertical Slice**: Clear separation into `Domain`, `Application`, `Infrastructure`, and `Api`.
- **🛡️ Architecture Tests**: Built-in `NetArchTest.Rules` verifying layer boundaries on every build.
- **⚡ Zero-Config Local Run**: Automatic SQLite database creation on launch. Microsoft SQL Server ready with 1 config switch.
- **📦 Central Package Management (CPM)**: Controlled via `Directory.Packages.props`.
- **🤖 AI-Native Agent Ecosystem**:
  - `AGENTS.md`: Universal instructions for any AI coding agent.
  - `.agent/skills/`: Custom Antigravity skills for feature generation, architecture guardianship, migrations, and testing.
  - `.cursor/rules/` & `.cursorrules`: Strict rules for Cursor users.
  - `CLAUDE.md`: Optimized instructions for Claude Code.
  - `.github/copilot-instructions.md`: Custom prompts for GitHub Copilot.
  - `PROMPTS.md`: Copy-paste prompt templates for ChatGPT, Claude, Gemini, DeepSeek.
- **📑 OpenAPI & Swagger**: Interactive documentation generated with `FastEndpoints.Swagger`.
- **🩺 Observability**: Serilog structured logging + Health Checks endpoint (`/health`).
- **🧪 Complete Testing Suite**:
  - Unit Tests with `xUnit` + `FluentAssertions`.
  - Integration Tests with `FastEndpoints.Testing` + `WebApplicationFactory`.
  - Architecture Tests with `NetArchTest.Rules`.
- **🐳 Container Ready**: Multi-stage `Dockerfile` + `docker-compose.yml` with Microsoft SQL Server 2022.
- **🧩 Dotnet CLI Template**: Ready to install globally via `dotnet new install .`.

---

## 📁 Repository Structure

```
template-csharp-net10-fastendpoints/
├── .agent/
│   └── skills/
│       ├── fastendpoints-feature-generator/
│       ├── architecture-guardian/
│       ├── efcore-migration-manager/
│       └── test-generator/
├── .cursor/
│   └── rules/
├── .github/
│   └── copilot-instructions.md
├── .template.config/
│   └── template.json           <- Enables 'dotnet new fastendpoints-ca'
├── docs/
│   ├── ARCHITECTURE.md         <- Mermaid diagrams & layer docs
│   ├── AI_AGENT_WORKFLOW.md    <- Guide for AI pair programming
│   └── CODING_STANDARDS.md     <- C# and FastEndpoints conventions
├── src/
│   ├── FastEndpointsTemplate.Domain/
│   ├── FastEndpointsTemplate.Application/
│   ├── FastEndpointsTemplate.Infrastructure/
│   └── FastEndpointsTemplate.Api/
├── tests/
│   ├── FastEndpointsTemplate.UnitTests/
│   ├── FastEndpointsTemplate.IntegrationTests/
│   └── FastEndpointsTemplate.ArchitectureTests/
├── AGENTS.md                   <- Universal master instructions for AI agents
├── CLAUDE.md                   <- Claude Code guidelines
├── .cursorrules                <- Cursor editor rules
├── PROMPTS.md                  <- Prompt recipes for LLMs
├── Directory.Build.props       <- Central MSBuild & .NET 9/.NET 10 switcher
├── Directory.Packages.props    <- Central Package Management (CPM)
├── Dockerfile                  <- Multi-stage production container
├── docker-compose.yml          <- API + PostgreSQL compose stack
└── FastEndpointsTemplate.sln   <- Visual Studio solution
```

---

## 🚀 Quick Start

### 1. Build and Run Tests
```powershell
# Restore and run all unit, integration, and architecture tests
dotnet test
```

### 2. Run the API Locally (SQLite default)
```powershell
dotnet run --project src/FastEndpointsTemplate.Api/FastEndpointsTemplate.Api.csproj
```
Open your browser at:
- **Swagger UI**: `http://localhost:5000/swagger`
- **Health Checks**: `http://localhost:5000/health`

### 3. Run with Docker Compose (Microsoft SQL Server)
```powershell
docker-compose up --build
```

---

## 🎯 Switching to .NET 10

This template is **100% .NET 10 ready**. Because machines may currently have the .NET 9 SDK installed, it defaults to `net9.0` so it can compile immediately.

To activate **.NET 10**:
1. Ensure the .NET 10 SDK is installed (`dotnet --list-sdks`).
2. Open `Directory.Build.props` and edit:
   ```xml
   <DefaultTargetFramework>net10.0</DefaultTargetFramework>
   ```
3. Or pass it from the command line:
   ```powershell
   dotnet build -p:TargetFrameworkOverride=net10.0
   ```

---

## 📦 Installing as a `dotnet new` Template

To generate new projects from this template from anywhere on your terminal:

```powershell
# 1. Install template from local directory
dotnet new install .

# 2. Create a new service anywhere
dotnet new fastendpoints-ca -n MyCompany.OrderService

# 3. Uninstall when needed
dotnet new uninstall .
```

---

## 🤖 Working with AI Agents

This repository is built from the ground up to be **AI-agent friendly**:

| Tool | Integration File |
|---|---|
| **Antigravity** | `AGENTS.md` + `.agent/skills/` |
| **Cursor** | `.cursorrules` + `.cursor/rules/*.mdc` |
| **Claude Code** | `CLAUDE.md` |
| **GitHub Copilot** | `.github/copilot-instructions.md` |
| **ChatGPT / DeepSeek** | `PROMPTS.md` |

Any agent reading `AGENTS.md` will instantly understand the REPR patterns, forbidden architectural mistakes, dependency limits, and testing conventions!
