using FastEndpoints;
using FastEndpointsTemplate.Application.Common.Interfaces;
using FastEndpointsTemplate.Application.Features.Products.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FastEndpointsTemplate.Api.Endpoints.Products;

public sealed record GetProductByIdRequest
{
    public Guid Id { get; init; }
}

public class GetProductByIdEndpoint(IApplicationDbContext dbContext) 
    : Endpoint<GetProductByIdRequest, ProductDto>
{
    public override void Configure()
    {
        Get("/api/products/{id:guid}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Obtener producto por ID";
            s.Description = "Recupera los detalles de un producto existente por su identificador único.";
            s.Responses[200] = "Producto encontrado";
            s.Responses[404] = "Producto no encontrado";
        });
    }

    public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
    {
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

        if (product is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(product, ct);
    }
}
