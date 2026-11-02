using FastEndpoints;
using FastEndpointsTemplate.Application.Common.Interfaces;
using FastEndpointsTemplate.Application.Features.Products.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FastEndpointsTemplate.Api.Endpoints.Products;

public sealed record ListProductsRequest
{
    [QueryParam]
    public int Page { get; init; } = 1;

    [QueryParam]
    public int PageSize { get; init; } = 10;

    [QueryParam]
    public string? Search { get; init; }

    [QueryParam]
    public bool? OnlyActive { get; init; }
}

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);

public class ListProductsEndpoint(IApplicationDbContext dbContext) 
    : Endpoint<ListProductsRequest, PagedResult<ProductDto>>
{
    public override void Configure()
    {
        Get("/api/products");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Listar productos con paginación y filtros";
            s.Description = "Obtiene una lista paginada de productos con opción de búsqueda por nombre o SKU.";
        });
    }

    public override async Task HandleAsync(ListProductsRequest req, CancellationToken ct)
    {
        var page = req.Page < 1 ? 1 : req.Page;
        var pageSize = req.PageSize is < 1 or > 100 ? 10 : req.PageSize;

        var query = dbContext.Products.AsNoTracking().AsQueryable();

        if (req.OnlyActive.HasValue)
        {
            query = query.Where(p => p.IsActive == req.OnlyActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(req.Search))
        {
            var search = req.Search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(search) || p.Sku.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
            .ToListAsync(ct);

        await Send.OkAsync(new PagedResult<ProductDto>(items, totalCount, page, pageSize), ct);
    }
}
