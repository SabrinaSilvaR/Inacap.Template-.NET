using FastEndpointsTemplate.Domain.Common;
using FastEndpointsTemplate.Domain.Events;
using FastEndpointsTemplate.Domain.Exceptions;

namespace FastEndpointsTemplate.Domain.Entities;

public class Product : AggregateRoot
{
    private Product() { } // Para EF Core

    private Product(string name, string sku, string description, decimal price, int stockQuantity)
    {
        Name = name;
        Sku = sku;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
        IsActive = true;

        AddDomainEvent(new ProductCreatedDomainEvent(Id, Sku, Price));
    }

    public string Name { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }

    public static Result<Product> Create(string name, string sku, string description, decimal price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Product>(Error.Validation("Product.EmptyName", "El nombre del producto no puede estar vacío."));

        if (string.IsNullOrWhiteSpace(sku))
            return Result.Failure<Product>(Error.Validation("Product.EmptySku", "El SKU no puede estar vacío."));

        if (price < 0)
            return Result.Failure<Product>(Error.Validation("Product.NegativePrice", "El precio no puede ser negativo."));

        if (stockQuantity < 0)
            return Result.Failure<Product>(Error.Validation("Product.NegativeStock", "El inventario no puede ser negativo."));

        var product = new Product(name.Trim(), sku.Trim().ToUpperInvariant(), description?.Trim() ?? string.Empty, price, stockQuantity);
        return Result.Success(product);
    }

    public Result UpdateDetails(string name, string description, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(Error.Validation("Product.EmptyName", "El nombre del producto no puede estar vacío."));

        if (price < 0)
            return Result.Failure(Error.Validation("Product.NegativePrice", "El precio no puede ser negativo."));

        Name = name.Trim();
        Description = description?.Trim() ?? string.Empty;
        Price = price;
        UpdatedAtUtc = DateTime.UtcNow;

        return Result.Success();
    }

    public Result UpdateStock(int quantityChange)
    {
        if (StockQuantity + quantityChange < 0)
        {
            return Result.Failure(Error.Conflict(
                "Product.InsufficientStock",
                $"Inventario insuficiente. Stock disponible: {StockQuantity}, solicitado cambio: {quantityChange}"));
        }

        StockQuantity += quantityChange;
        UpdatedAtUtc = DateTime.UtcNow;
        return Result.Success();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}
