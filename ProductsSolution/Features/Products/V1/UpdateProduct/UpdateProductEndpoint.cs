using FastEndpoints;
using ProductsSolution.Infrastructure.Data;

namespace ProductsSolution.Features.Products.V1.UpdateProduct
{
    public class UpdateProductEndpoint : Endpoint<UpdateProductRequest, object>
    {
        private readonly AppDbContext DbContext;

        public UpdateProductEndpoint(AppDbContext db)
        {
            DbContext = db;
        }

        public override void Configure()
        {
            Put("products/{id}");
            AllowAnonymous();
            Version(1);
        }
        public override async Task<object> ExecuteAsync(UpdateProductRequest req, CancellationToken ct)
        {
            var id = Route<int>("id");
            var product = await DbContext.Products.FindAsync(new object[] { id }, ct);

            if (product == null)
                return TypedResults.NotFound();

            product.Name = req.Name;
            product.Price = req.Price;
            product.IsAvailable = req.IsAvailable;

            await DbContext.SaveChangesAsync(ct);

            return new UpdateProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }
    }
}
