using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Infrastructure.Data;
using static FastEndpoints.Ep;

namespace ProductsSolution.Features.Products.V1.GetProductsById;
public class GetProductsByIdEndpoint : EndpointWithoutRequest<object>
{
    private readonly AppDbContext DbContext;

    public GetProductsByIdEndpoint(AppDbContext db)
    {
        DbContext = db;
    }

    public override void Configure()
    {
        Get("products/{id}");
        AllowAnonymous();
        Version(1);
    }

    public override async Task<object> ExecuteAsync(CancellationToken ct)
    {
        var id = Route<int>("id");

        var product = await DbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (product == null)
            return TypedResults.NotFound();

        return new GetProductByIdResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}
