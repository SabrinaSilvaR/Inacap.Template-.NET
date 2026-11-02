namespace FastEndpointsTemplate.Application.Features.Products.Dtos;

public sealed record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    string Description,
    decimal Price,
    int StockQuantity,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);
