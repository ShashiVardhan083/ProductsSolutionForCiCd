using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using ProductsSolution.Common.PreProcessors;
using ProductsSolution.Domain.Entities;
using ProductsSolution.Infrastructure.Data;

namespace ProductsSolution.Features.Products.V1.CreateProduct
{
    public class CreateProductEndpoint : Endpoint<CreateProductRequest, object>
    {
        private readonly AppDbContext DbContext;

        public CreateProductEndpoint(AppDbContext db)
        {
            DbContext = db;
        }

        public override void Configure()
        {
            Post("products");
            AllowAnonymous();
            Version(1);

            PreProcessor<RequestLoggingPreProcessor<CreateProductRequest>>();
            PostProcessor<ExecutionTimePostProcessor<CreateProductRequest, object>>();
        }

        public override async Task<object> ExecuteAsync(CreateProductRequest req, CancellationToken ct)
        {

            var exists = await DbContext.Products.AnyAsync(x => x.Name == req.Name, ct);

            if (exists)
                return TypedResults.Conflict("Product with same name already exists.");

            var product = new Product
            {
                Name = req.Name,
                Price = req.Price,
                IsAvailable = req.IsAvailable
            };
            DbContext.Products.Add(product);
            await DbContext.SaveChangesAsync(ct);

            return TypedResults.Created($"/api/v1/products/{product.Id}", product);
        }
    }
}
