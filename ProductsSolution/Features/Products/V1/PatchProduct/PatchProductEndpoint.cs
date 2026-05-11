using FastEndpoints;
using ProductsSolution.Infrastructure.Data;

namespace ProductsSolution.Features.Products.V1.PatchProduct
{
    public class PatchProductEndpoint : Endpoint<PatchProductRequest, object>
    {
        private readonly AppDbContext DbContext;

        public PatchProductEndpoint(AppDbContext db)
        {
            DbContext = db;
        }

        public override void Configure()
        {
            Patch("products/{id}");
            AllowAnonymous();
            Version(1);
        }
        public override async Task<object> ExecuteAsync(PatchProductRequest patchProductRequest, CancellationToken ct)
        {
            var id = Route<int>("id");

            var product = await DbContext.Products.FindAsync(new object[] { id }, ct);

            if (product == null)
                return TypedResults.NotFound();

            if (patchProductRequest.Name != null)
                product.Name = patchProductRequest.Name;

            if (patchProductRequest.Price.HasValue)
                product.Price = patchProductRequest.Price.Value;

            if (patchProductRequest.IsAvailable.HasValue)
                product.IsAvailable = patchProductRequest.IsAvailable.Value;

            await DbContext.SaveChangesAsync(ct);

            return new PatchProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}
