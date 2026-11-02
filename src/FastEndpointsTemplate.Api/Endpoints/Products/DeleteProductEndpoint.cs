using FastEndpoints;
using FastEndpointsTemplate.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FastEndpointsTemplate.Api.Endpoints.Products;

public sealed record DeleteProductRequest
{
    public Guid Id { get; init; }
}

public class DeleteProductEndpoint(IApplicationDbContext dbContext) 
    : Endpoint<DeleteProductRequest>
{
    public override void Configure()
    {
        Delete("/api/products/{id:guid}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Desactivar o eliminar producto";
            s.Description = "Realiza un soft-delete desactivando el producto en el catálogo.";
            s.Responses[204] = "Producto desactivado correctamente";
            s.Responses[404] = "Producto no encontrado";
        });
    }

    public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(p => p.Id == req.Id, ct);

        if (product is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        product.Deactivate();
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}
