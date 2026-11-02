using FastEndpoints;
using FastEndpointsTemplate.Application.Common.Interfaces;
using FastEndpointsTemplate.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FastEndpointsTemplate.Api.Endpoints.Products;

public sealed record CreateProductRequest(
    string Name,
    string Sku,
    string Description,
    decimal Price,
    int StockQuantity
);

public sealed record CreateProductResponse(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    int StockQuantity,
    bool IsActive,
    DateTime CreatedAtUtc
);

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
        var productResult = Product.Create(req.Name, req.Sku, req.Description, req.Price, req.StockQuantity);

        if (productResult.IsFailure)
        {
            ThrowError(productResult.Error.Description, StatusCodes.Status400BadRequest);
            return;
        }

        var product = productResult.Value;
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(ct);

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
