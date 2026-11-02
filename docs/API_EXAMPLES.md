# 📖 Guía Didáctica: Anatomía de Endpoints, DTOs y Validadores

Esta guía explica en detalle cómo están construidos los ejemplos del repositorio y cómo cualquier desarrollador o IA debe crear nuevos endpoints siguiendo el **Patrón REPR (Request-Endpoint-Response)** de FastEndpoints y Clean Architecture.

---

## 1. El Ciclo REPR en un Solo Archivo

En FastEndpoints, cada endpoint representa un caso de uso específico ("Vertical Slice"). En lugar de fragmentar un controlador en 5 métodos dispersos, cada endpoint agrupa su contrato:

```
[Request DTO]  -->  [Validator]  -->  [Endpoint Handler]  -->  [Response DTO]
```

### Ejemplo Real: `POST /api/products`

Ubicación: [`src/FastEndpointsTemplate.Api/Endpoints/Products/CreateProductEndpoint.cs`](file:///C:/Users/sabri/source/Proyecto-Integrador/template-csharp-net10-fastendpoints/src/FastEndpointsTemplate.Api/Endpoints/Products/CreateProductEndpoint.cs)

#### 1. Request DTO (Contrato de Entrada)
```csharp
public sealed record CreateProductRequest(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    int StockQuantity
);
```

#### 2. Response DTO (Contrato de Salida)
```csharp
public sealed record CreateProductResponse(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    int StockQuantity,
    bool IsActive,
    DateTime CreatedAtUtc
);
```

#### 3. Validador Automático con FluentValidation
FastEndpoints ejecuta este validador antes de entrar al handler. Si falla, corta el flujo y responde automáticamente con un `400 Bad Request` formateado con RFC 7807 ProblemDetails:
```csharp
public sealed class CreateProductValidator : Validator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("El SKU es obligatorio.")
            .MaximumLength(50).WithMessage("El SKU no puede exceder 50 caracteres.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("El precio debe ser mayor o igual a 0.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
    }
}
```

#### 4. Endpoint Handler
```csharp
public class CreateProductEndpoint(IApplicationDbContext dbContext) 
    : Endpoint<CreateProductRequest, CreateProductResponse>
{
    public override void Configure()
    {
        Post("/api/products");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Crear un nuevo producto";
            s.Description = "Crea un producto en el catálogo validando invariantes del dominio.";
            s.ExampleRequest = new CreateProductRequest("Laptop Pro 16", "TECH-LAP-001", "Notebook para desarrollo", 1499.99m, 25);
            s.Responses[201] = "Producto creado exitosamente";
            s.Responses[400] = "Error de validación";
        });
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        // 1. Invocar invariantes del Dominio (Result pattern)
        var productResult = Product.Create(req.Name, req.Sku, req.Description, req.Price, req.StockQuantity);

        if (productResult.IsFailure)
        {
            ThrowError(productResult.Error.Description, StatusCodes.Status400BadRequest);
            return;
        }

        // 2. Persistir entidad
        var product = productResult.Value;
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(ct);

        // 3. Responder con Send.CreatedAtAsync apuntando al endpoint de consulta
        var response = new CreateProductResponse(
            product.Id,
            product.Name,
            product.Sku,
            product.Price,
            product.StockQuantity,
            product.IsActive,
            product.CreatedAtUtc
        );

        await Send.CreatedAtAsync<GetProductByIdEndpoint>(
            new { id = product.Id },
            response,
            cancellation: ct);
    }
}
```

---

## 2. DTOs de Aplicación y Proyecciones EF Core

Ubicación: [`src/FastEndpointsTemplate.Application/Features/Products/Dtos/ProductDto.cs`](file:///C:/Users/sabri/source/Proyecto-Integrador/template-csharp-net10-fastendpoints/src/FastEndpointsTemplate.Application/Features/Products/Dtos/ProductDto.cs)

Para consultas de lectura (`GET`), se utiliza un DTO inmutable proyectado directamente con `AsNoTracking()` y `.Select()` para máximo rendimiento y evitar tracking innecesario en memoria:

```csharp
var product = await dbContext.Products
    .AsNoTracking()
    .Where(p => p.Id == req.Id)
    .Select(p => new ProductDto(
        p.Id,
        p.Name,
        p.Sku,
        p.Description,
        p.Price,
        p.StockQuantity,
        p.IsActive,
        p.CreatedAtUtc,
        p.UpdatedAtUtc))
    .FirstOrDefaultAsync(ct);
```

---

## 3. Modelo Paginado Genérico Reutilizable

Ubicación: [`src/FastEndpointsTemplate.Api/Endpoints/Products/ListProductsEndpoint.cs`](file:///C:/Users/sabri/source/Proyecto-Integrador/template-csharp-net10-fastendpoints/src/FastEndpointsTemplate.Api/Endpoints/Products/ListProductsEndpoint.cs#L22)

```csharp
public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);
```

---

## 4. Cómo Probar Todo con 1 Clic

Abre el archivo [`requests.http`](file:///C:/Users/sabri/source/Proyecto-Integrador/template-csharp-net10-fastendpoints/requests.http) en VS Code (con la extensión **REST Client**) y haz clic en **`Send Request`** sobre cualquiera de las peticiones preparadas.
