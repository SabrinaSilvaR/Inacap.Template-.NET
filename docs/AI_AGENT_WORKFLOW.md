# 🤖 AI Agent Workflow Guide

This document details how autonomous agents and AI pair programmers should interact with this repository.

## 1. Context Ingestion Order
When an AI agent starts a session, it should review:
1. `AGENTS.md` (master rules)
2. `Directory.Build.props` & `Directory.Packages.props` (dependencies)
3. The relevant skill under `.agent/skills/`

## 2. Agent Skills Directory
- **`fastendpoints-feature-generator`**: Scaffolds complete vertical slice features.
- **`architecture-guardian`**: Validates layer dependencies and NetArchTest compliance.
- **`efcore-migration-manager`**: Handles database schema migrations safely.
- **`test-generator`**: Creates Unit, Integration, and Architecture tests.

## 3. Tooling Compatibility
- **Antigravity**: Native support for `.agent/skills/` and `AGENTS.md`.
- **Cursor**: Native support for `.cursorrules` and `.cursor/rules/*.mdc`.
- **Claude Code**: Native support for `CLAUDE.md`.
- **GitHub Copilot**: Native support for `.github/copilot-instructions.md`.
