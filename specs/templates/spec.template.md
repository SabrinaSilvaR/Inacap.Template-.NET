# Capability Specification: [Capability Name]

## Status: ACTIVE (Living Truth)

---

## 1. Overview
Summary of the capability's purpose and boundaries in the system.

---

## 2. Requirements & Contracts

### REQ-[CAP]-001: [Requirement Title]
- **Endpoint**: `[METHOD] /api/[path]`
- **Request**:
  - `field`: type (Constraints)
- **Validation**: Rules enforced via FluentValidation.
- **Domain Invariants**: Business rules checked before persisting.
- **Response**:
  - `200 OK` or `201 Created`: Description.
  - `400 Bad Request`: ProblemDetails validation failure.
