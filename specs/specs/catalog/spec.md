# Capability Specification: Product Catalog

## Status: ACTIVE (Living Truth)

---

## 1. Overview
The Product Catalog capability provides enterprise-grade management of catalog items (products), including creation with domain invariants, paginated search, retrieval by identifier, and safe soft-deletion.

---

## 2. Requirements & Contracts

### REQ-CAT-001: Product Creation
- **Endpoint**: `POST /api/products`
- **Request**:
  - `name`: string (Required, max 200 chars)
  - `sku`: string (Required, max 50 chars, unique)
  - `description`: string (Optional, max 1000 chars)
  - `price`: decimal (Required, >= 0)
  - `stockQuantity`: int (Required, >= 0)
- **Validation**: Enforced via FluentValidation (`CreateProductValidator`). Returns `400 Bad Request` ProblemDetails if violated.
- **Domain Invariants**: Enforced via `Product.Create(...)`. Emits `ProductCreatedDomainEvent`.
- **Response**: `201 Created` with Location header pointing to `GET /api/products/{id}`.

### REQ-CAT-002: Product Retrieval by ID
- **Endpoint**: `GET /api/products/{id:guid}`
- **Response**:
  - `200 OK`: Returns `ProductDto` (Id, Name, Sku, Description, Price, StockQuantity, IsActive, CreatedAtUtc, UpdatedAtUtc).
  - `404 Not Found`: If product does not exist.

### REQ-CAT-003: Paginated Product Listing & Filtering
- **Endpoint**: `GET /api/products`
- **Query Parameters**:
  - `page`: int (Default 1)
  - `pageSize`: int (Default 10, Max 100)
  - `search`: string (Optional - searches by Name or SKU)
  - `onlyActive`: bool (Optional - filters active status)
- **Response**: `200 OK` with `PagedResult<ProductDto>` (Items, TotalCount, Page, PageSize).

### REQ-CAT-004: Soft Deletion
- **Endpoint**: `DELETE /api/products/{id:guid}`
- **Behavior**: Marks product as inactive (`IsActive = false`). Does not drop row from database.
- **Response**:
  - `204 No Content`: Successful deactivation.
  - `404 Not Found`: If product does not exist.
